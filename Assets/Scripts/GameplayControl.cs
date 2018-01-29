using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

#pragma warning disable
public class GameplayControl : MonoBehaviour
{
	bool isCallLoginGPG = false;
	public GameObject bottomPoint, blockBgPos, blockBg;
	SafeInt _score;
	public SpriteRenderer textEffectRender;
	public GameObject LosePopup;
	public RateControl RatePopup;
	public bool isFailed = false;
	public List<Grid> listAllGrids;
	int _comboMatchCount;
	public GameObject getScoreEffect;
	public Text tGetScoreEffect;
	public GameObject board;
	int socreCheck = 1542;
	float focusTime;

	public int comboMatchCount {
		get {
			return _comboMatchCount;
		}
		set {
			_comboMatchCount = value;
			GameManager.dataSave.comboMatchCount = value;
		}
	}

	public bool isGetNewHighScore = false;

	public SafeInt score {
		get { return _score; }
		set {
			
			_score = value;

			UITopControl.instance.tScore.text = value.ToString ();
			if (bestScore != null && bestScore.GetValue () < value.GetValue ()) {
				
				bestScore = new SafeInt (value.GetValue ());
				if (isGetNewHighScore == false) {
					isGetNewHighScore = true;
				}

			}

		}
	}

	SafeInt _bestScore;
	int lastBest;

	public SafeInt bestScore {
		get { return _bestScore; }
		set {			
			_bestScore = value;
			UITopControl.instance.tBest.text = value.GetValue ().ToString ();
			 

		}
	}

	public Block moveBlock;
	Block _startBlock;
	public List<Block> listStartBlock;

	public Block startBlock {
		get { return _startBlock; }
		set {
			if (value != null) {
				_startBlock = value;
				moveBlockId = value.id;
				if (blockMoveOutCoroutine == null) {
					blockMoveOutCoroutine = MoveBlockMoveOut ();
					StartCoroutine (blockMoveOutCoroutine);
				}
			}
		}
	}

	public GameObject gridPrefap;
	public Grid[,] grids = new Grid[8, 8];
	int _moveBlockId;
	public static GameplayControl instance;

	public int moveBlockId {
		get { return _moveBlockId; }
		set {
			
			_moveBlockId = value;

		}
	}

	bool canMoveBlock = false;

	void Awake ()
	{



		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		Application.targetFrameRate = 60;
		instance = this;
	}

	public void CreateBoard ()
	{
		SUGame.Get<SUAnalytics> ().LastLogin ();
		listAllGrids = new List<Grid> ();
		score = GameManager.dataSave.currentScore;
		bestScore = GameManager.dataSave.bestScore;
		comboMatchCount = GameManager.dataSave.comboMatchCount;
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				GameObject gridObj = (GameObject)Instantiate (gridPrefap, new Vector3 (board.transform.position.x - 3.5F + x, board.transform.position.y + 3.5F - y, 0), Quaternion.Euler (0, 0, 0));
				gridObj.transform.parent = board.transform;
				Grid _grid = gridObj.GetComponent<Grid> ();
				listAllGrids.Add (_grid);
				_grid.X = x;
				_grid.Y = y;
				grids [x, y] = _grid;

				if (GameManager.dataSave.board [x * 8 + y] == 1) {
					_grid.subGrid.SetActive (true);
					_grid.haveBlockCell = true;
					_grid.subRender.color = new Color (1, 1, 1, 1);
					_grid.subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor ((BlockColor)GameManager.dataSave.boardColor [x * 8 + y]);
					_grid.gridCol = (BlockColor)GameManager.dataSave.boardColor [x * 8 + y];
				}

			}
		}
		moveBlock.gameObject.SetActive (false);

		// load from save 
		if (GameManager.dataSave.bestScore.GetValue () > 0) {
			for (int i = 0; i < 3; i++) {
				if (GameManager.dataSave.startBlockVisible [i] == 1) {
					if (GameManager.dataSave.startIds [i] >= 0) {
						listStartBlock [i].InitBlock (GameManager.dataSave.startIds [i]);
						listStartBlock [i].blockColor = (BlockColor)GameManager.dataSave.startBlockColor [i];
					} else {
						int rnd = GameManager.GetRandomBlockId ();
						listStartBlock [i].InitBlock (rnd);
						listStartBlock [i].blockColor = (BlockColor)Random.Range (0, BlockColorDefine.instance.listBlockColorStruct.Count);
					}
					listStartBlock [i].NewBlockEffect ();
				} else {
					listStartBlock [i].gameObject.SetActive (false);
				}
			}
			CheckHaveStartBlockMatch ();
		} else {
			
			for (int i = 0; i < 3; i++) {
				listStartBlock [i].gameObject.SetActive (false);
			}

		}
		if (GameManager.dataSave.bestScore.GetValue () == 0) {
			TutorialControl.instance.ShowTutorial ();
		}
		//CheckHaveStartBlockMatch ();
		if (GameManager.dataSave.bestScore.GetValue () > 0) {
			// show ads at open
			SUGame.Get<SUAdmob> ().ShowIads ();
		} else {
			Debug.Log ("Khong show fullAd vi la lan dau mo game");
		}




	}

	public void CreateBoardEffect ()
	{
		StartCoroutine (CreateBoardEffectCoroutine ());
	}

	IEnumerator CreateBoardEffectCoroutine ()
	{
		Application.targetFrameRate = 30;
		for (int i = 0; i < 3; i++) {
			listStartBlock [i].gameObject.SetActive (false);
		}
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				grids [x, y].AppearEffect ();
				grids [7 - x, 7 - y].AppearEffect ();
				grids [x, y].subGrid.SetActive (false);
				grids [x, y].haveBlockCell = false;
				grids [x, y].render.sprite = BlockColorDefine.instance.sprGrid;
				grids [x, y].subRender.color = new Color (1, 1, 1, 1);
				grids [x, y].hintGrid.SetActive (false);

			}
			yield return null;
		}
		for (int i = 0; i < 3; i++) {
			int rnd = GameManager.GetRandomBlockId ();
			listStartBlock [i].InitBlock (rnd);
			listStartBlock [i].blockColor = (BlockColor)Random.Range (0, BlockColorDefine.instance.listBlockColorStruct.Count);
			listStartBlock [i].haveMatchPos = true;
			listStartBlock [i].gameObject.SetActive (true);
			listStartBlock [i].NewBlockEffect ();
		}
		Application.targetFrameRate = 30;
		// login google play 
		if (GameManager.isNetworkConnected == true) {
			if (GameManager.dataSave.isLoginedGoogle == true) {
				if (GPG_LeaderboardManager.instance.isLoginSuccess == false) {
					GPG_LeaderboardManager.instance.LogIn (false);
				}
			} else if (GameManager.replayCount == 2 && GameManager.dataSave.isLoginedGoogle == false) {
				if (GPG_LeaderboardManager.instance.isLoginSuccess == false) {
					isCallLoginGPG = true;
					GPG_LeaderboardManager.instance.LogIn (false);
				}
			}
		}
	}

	void ClearDataSave ()
	{
		GameManager.dataSave.currentScore = new SafeInt (0);
		for (int i = 0; i < 64; i++) {
			GameManager.dataSave.board [i] = 0;
			GameManager.dataSave.boardColor [i] = 0;
			if (i < 3) {
				GameManager.dataSave.startIds [i] = -1;
				GameManager.dataSave.startBlockColor [i] = -1;
				GameManager.dataSave.startBlockVisible [i] = 1;
			}
		}
		GameManager.instance.SaveData ();
	}

	void Start ()
	{


		UITopControl.instance.ani.SetFloat ("Speed", 1);
		UITopControl.instance.ani.SetInteger ("State", 0);
		UITopControl.instance.ani.Play (0);
		InitCamera ();
		CreateBoard ();
		StartCoroutine (InitCameraCoroutine ());
		SUGame.Get<SUAdmob> ().ShowBanner ();
	}

	void OnApplicationFocus (bool hasFocus)
	{
		if (hasFocus == true) {			
			if (LosePopup.activeInHierarchy == false) {
				if (isCallLoginGPG == false) {
					Debug.Log ("Show ads by focus");
					SUGame.Get<SUAdmob> ().ShowIads ();
				} else {
					isCallLoginGPG = false;
					Debug.Log ("Khong show ads khi focus vi vua goi ham login GPG");
				}
			} else {
				Debug.Log ("Khong show fullAd khi focus vi dang hien popup lose");
			}
		}
	}

	IEnumerator InitCameraCoroutine ()
	{
		socreCheck = 3425;
		yield return null;
		InitCamera ();
		yield return null;
		InitCamera ();
	}

	Grid lastNearlestGrid = null;
	Grid nearlestGrid = null;

	void ClearHighLight ()
	{
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {				
				if (GameplayControl.instance.grids [x, y].beforeBlockSprite != null) {
					GameplayControl.instance.grids [x, y].subRender.sprite = GameplayControl.instance.grids [x, y].beforeBlockSprite;
					GameplayControl.instance.grids [x, y].beforeBlockSprite = null;
				}
			}
		}
	}

	void Update ()
	{
		
		if (focusTime > 0) {
			focusTime -= Time.deltaTime;
		}
		if (moveBlock.gameObject.activeInHierarchy == true && Input.GetMouseButton (0) && canMoveBlock == true) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			moveBlock.gameObject.transform.position = mousePos + new Vector3 (0, 3, 10);
			//moveBlock.gameObject.transform.localScale = new Vector3 (1, 1, 1);
			float minDistance = GetNearlestGrid ();
			if (minDistance > 1) {
				ClearHighLight ();
				if (lastNearlestGrid != null) {
					lastNearlestGrid.ClearInit ();
				}
			} else {
				if (nearlestGrid != lastNearlestGrid) {
					if (lastNearlestGrid != null) {
						lastNearlestGrid.ClearInit ();
					}
					lastNearlestGrid = nearlestGrid;
					ClearHighLight ();
					nearlestGrid.Init (moveBlock.id, false, false, false);
				}
			}
			if (TutorialControl.instance.isShowingTutorial == true) {
				if (TutorialControl.instance.tutorialHand.gameObject.activeInHierarchy == true) {
					TutorialControl.instance.tutorialHand.render.enabled = false;
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if (TutorialControl.instance.isShowingTutorial == true) {
				if (TutorialControl.instance.tutorialHand.gameObject.activeInHierarchy == true) {
					TutorialControl.instance.tutorialHand.render.enabled = true;
				}
			}
			if (startBlock != null) {
				if (canMoveBlock == true) {
					ClearHighLight ();
					//Debug.Log ("set grid");
					//moveBlock.gameObject.SetActive (false);
					canMoveBlock = false;
					AudioManager.instance.PlaySound (AudioClipType.AC_PUT_BLOCK);
					StartCoroutine (OnMouseRelease ());
				} else {
					ClearHighLight ();
					//moveBlock.gameObject.SetActive (false);
					if (blockMoveOutCoroutine != null) {
						StopCoroutine (blockMoveOutCoroutine);
						blockMoveOutCoroutine = null;
						if (moveBlockMoveIn == null) {
							moveBlockMoveIn = MoveBlockMoveIn ();
						}
						StartCoroutine (moveBlockMoveIn);
					}
					//startBlock = null;
				}
			}
		}
	}
	/*
	void LateUpdate ()
	{
		
		if (score > 0 && lastBest != (bestScore + socreCheck)) {
			bestScore = lastBest - socreCheck;
		}
	}
	*/
	IEnumerator moveBlockMoveIn;

	IEnumerator OnMouseRelease ()
	{
		float minDistance = GetNearlestGrid ();
		nearlestGrid.ClearInit ();

		if (minDistance <= 1) {	
			if (TutorialControl.instance.isShowingTutorial == false || (TutorialControl.instance.isShowingTutorial == true && TutorialControl.instance.tutorialGridPos.X == nearlestGrid.X && TutorialControl.instance.tutorialGridPos.Y == nearlestGrid.Y)) {
				bool[] createBlock = nearlestGrid.Init (moveBlock.id, true, false, false);
				if (createBlock [0] == false) {
					// neu khong dat xuong duoc
					if (moveBlockMoveIn == null) {
						moveBlockMoveIn = MoveBlockMoveIn ();
					}
					StartCoroutine (moveBlockMoveIn);
					//Application.targetFrameRate = 30;
					//startBlock.gameObject.SetActive (true);

				} else {
					moveBlock.gameObject.SetActive (false);
					// neu dat xuong duoc 
					if (createBlock [1] == true) {
						if (TutorialControl.instance.isShowingTutorial == true) {
							TutorialControl.instance.count++;
						}
						// neu dat xuong duoc va co an ngang hoac doc 
						comboMatchCount++;
						if (GameplayControl.instance.comboMatchCount == 2) {
							AudioManager.instance.PlaySound (AudioClipType.AC_COOL);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_COOL);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						} else if (GameplayControl.instance.comboMatchCount == 3) {
							AudioManager.instance.PlaySound (AudioClipType.AC_GOOD);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_GOOD);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						} else if (GameplayControl.instance.comboMatchCount >= 4) {
							AudioManager.instance.PlaySound (AudioClipType.AC_EXCELENT);
							var spr = TextEffectDefine.instance.GetTextEffectSpriteByType (TextEffectType.TE_EXCELENT);
							if (spr != null) {
								textEffectRender.gameObject.SetActive (true);
								textEffectRender.sprite = spr;
							}
						}

					} else {
						comboMatchCount = 0;
					}
				}
		
			} else {
				
				//startBlock.gameObject.SetActive (true);
				if (moveBlockMoveIn == null) {
					moveBlockMoveIn = MoveBlockMoveIn ();
				}
				StartCoroutine (moveBlockMoveIn);
			}
		
		} else {
			//Application.targetFrameRate = 30;
			//startBlock.gameObject.SetActive (true);
			if (moveBlockMoveIn == null) {
				moveBlockMoveIn = MoveBlockMoveIn ();
			}
			StartCoroutine (moveBlockMoveIn);
		}

		//startBlock = null;
		if (moveBlockMoveIn == null && (TutorialControl.instance.isShowingTutorial == false || (TutorialControl.instance.isShowingTutorial == true && TutorialControl.instance.count > 2))) {
			CreateNewStartBlock ();
		}
		yield return new WaitForSeconds (0.05F * 9);
		bool isAllStartBlockVisible = true;
		for (int i = 0; i < listStartBlock.Count; i++) {
			if (listStartBlock [i].gameObject.activeInHierarchy == true) {
				isAllStartBlockVisible = false;
			}
		}
		if (isAllStartBlockVisible == false && moveBlock.gameObject.activeInHierarchy == false) {
			CheckHaveStartBlockMatch ();
			yield return null;
			// save data 
			InitDataAndSave ();
		}

	}

	public void InitNewGame ()
	{
		//Debug.Log ("list percent truoc" + GameManager.listPercent.Count);

		isGetNewHighScore = false;
		isFailed = false;
		score = new SafeInt (0);


		CreateBoardEffect ();



	}


	public void InitDataAndSave ()
	{
		if (isFailed == true) {
			//ClearDataSave ();
			//return;
		}
		GameManager.dataSave.currentScore = score;
		GameManager.dataSave.bestScore = bestScore;
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				GameManager.dataSave.board [x * 8 + y] = grids [x, y].haveBlockCell == true ? 1 : 0;
				GameManager.dataSave.boardColor [x * 8 + y] = (int)grids [x, y].gridCol;
			}
		}
		for (int i = 0; i < 3; i++) {
			GameManager.dataSave.startIds [i] = listStartBlock [i].id;
			GameManager.dataSave.startBlockColor [i] = (int)listStartBlock [i].blockColor;
			GameManager.dataSave.startBlockVisible [i] = listStartBlock [i].gameObject.activeInHierarchy == true ? 1 : 0;
		}
		GameManager.instance.SaveData ();
	}

	List<Grid> listMatchH = new List<Grid> ();
	List<Grid> listMatchV = new List<Grid> ();

	public List<Grid> CheckMatch (Grid _grid)
	{
		// check ngang 
		listMatchH.Clear ();
		listMatchV.Clear ();
		for (int x = 0; x < 8; x++) {
			if (grids [x, _grid.Y].subGrid.activeInHierarchy == true) {
				listMatchH.Add (grids [x, _grid.Y]);
			}
		}
		for (int y = 0; y < 8; y++) {
			if (grids [_grid.X, y].subGrid.activeInHierarchy == true) {
				listMatchV.Add (grids [_grid.X, y]);
			}
		}

		if (listMatchH.Count == 8 && listMatchV.Count == 8) {
			for (int i = 0; i < listMatchH.Count; i++) {
				if (listMatchH [i] != _grid) {
					listMatchV.Add (listMatchH [i]);
				}
			}
			listMatchV.Sort ((a, b) => (Mathf.Abs ((a.Y + a.X) - (_grid.Y + _grid.X)).CompareTo (Mathf.Abs ((b.Y + b.X) - (_grid.Y + _grid.X)))));
			return listMatchV;
		}
		if (listMatchH.Count == 8) {
			listMatchH.Sort ((a, b) => (Mathf.Abs (a.X - _grid.X)).CompareTo (Mathf.Abs (b.X - _grid.X)));
			return listMatchH;
		}
		if (listMatchV.Count == 8) {
			listMatchV.Sort ((a, b) => (Mathf.Abs (a.Y - _grid.Y)).CompareTo (Mathf.Abs (b.Y - _grid.Y)));
			return listMatchV;
		}

		listMatchH.Clear ();
		return listMatchH;
	}

	void CheckHaveStartBlockMatch ()
	{
		for (int i = 0; i < listStartBlock.Count; i++) {
			
			Block _startBlock = listStartBlock [i];
			_startBlock.haveMatchPos = true;
			_startBlock.listGridsCanMatch.Clear ();
			bool haveMatchPos = false;
			int gridMatchCount = 0;
			for (int x = 0; x < 8; x++) {
				for (int y = 0; y < 8; y++) {
					if (grids [x, y].Init (_startBlock.id, true, true, false) [0] == true) {
						haveMatchPos = true;
						_startBlock.listGridsCanMatch.Add (grids [x, y]);
						gridMatchCount++;
						if (gridMatchCount > 2) {
							break;
						}
					}
				}
				if (gridMatchCount > 2) {
					break;
				}
			}
			_startBlock.haveMatchPos = haveMatchPos;

		}



		// check Lost 

		bool isLose = true;
		int startBlockShowCount = 0;
		int blockShowOnlyPos = 0;
		for (int i = 0; i < listStartBlock.Count; i++) {
			if (listStartBlock [i].gameObject.activeInHierarchy == true && listStartBlock [i].haveMatchPos == true) {
				isLose = false;
				startBlockShowCount++;
				blockShowOnlyPos = i;
			}
		}

		if (startBlockShowCount == 1) {
			Block blockShowOnly = listStartBlock [blockShowOnlyPos];
			if (blockShowOnly.listGridsCanMatch.Count == 1) {
				blockShowOnly.listGridsCanMatch [0].Init (blockShowOnly.id, false, false, true);
			}
		}

		if (isLose == true && isFailed == false) {
			// thua 
			isFailed = true;
			Debug.Log ("Thua cuoc");
			StartCoroutine (LoseCoroutine ());
			//ClearDataSave ();
		}
	}

	void CreateNewStartBlock ()
	{
		bool allStartBlockIsDisable = true;
		for (int i = 0; i < listStartBlock.Count; i++) {
			Block _startBlock = listStartBlock [i];
			if (_startBlock.gameObject.activeInHierarchy == true) {
				allStartBlockIsDisable = false;
				break;
			}
		}

		if (allStartBlockIsDisable == true) {
			for (int i = 0; i < listStartBlock.Count; i++) {
				Block _startBlock = listStartBlock [i];
				_startBlock.gameObject.SetActive (true);
				_startBlock.InitBlock (GameManager.GetRandomBlockId ());
				_startBlock.NewBlockEffect ();

			}
		}
	}


	float  GetNearlestGrid ()
	{
		float minDistance = 100;
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				float distance = (moveBlock.transform.position - grids [x, y].transform.position).magnitude;
				if (distance < minDistance) {
					minDistance = distance;
					nearlestGrid = grids [x, y];
				}
			}
		}
		return minDistance;
	}

	IEnumerator LoseCoroutine ()
	{
		Application.targetFrameRate = 60;
		//yield return new WaitForSeconds (1F);
		AudioManager.instance.PlaySound (AudioClipType.AC_LOSE_EFFECT);
		float time = 0;
		listAllGrids.Shuffle ();
		for (int i = 0; i < listAllGrids.Count; i++) {
			if (listAllGrids [i].subGrid.activeInHierarchy == true) {
				listAllGrids [i].SetGridLose ();
				yield return new WaitForSeconds (0.01F);
				time += 0.01F;
			}
		}
		yield return new WaitForSeconds (time + 1F);

		if (GameManager.dataSave.isRate == false && GameManager.isPopupRateShowed == false) {
			if (GameManager.dataSave.openGameCount > 0 && (GameManager.dataSave.openGameCount % SUGame.Get<SURemoteConfig> ().GetBackNumber ()) == 0) {
				if (GameManager.replayCount > 0 && SUGame.Get<SURemoteConfig> ().initCompleted == true && GameManager.replayCount % SUGame.Get<SURemoteConfig> ().GetReplayNumber () == 0) {
					GameManager.isPopupRateShowed = true;
					yield return StartCoroutine (OutAnimation (EndOutAnimationType.EOT_SHOW_RATE_POPUP));
				}
			}
		}

		while (RatePopup.gameObject.activeInHierarchy == true || RatePopup.ratePopup2.activeInHierarchy == true) {
			yield return new WaitForSeconds (0.1F);
		}


		//
		yield return new WaitForSecondsRealtime (0.5F);
		StartCoroutine (OutAnimation (EndOutAnimationType.EOT_SHOW_LOSE_POPUP));

	}

	public IEnumerator blockMoveOutCoroutine;

	IEnumerator MoveBlockMoveOut ()
	{
		Application.targetFrameRate = 60;
		moveBlock.gameObject.SetActive (true);
		//Debug.Log ("id is : " + startBlock.id);
		moveBlock.InitBlock (startBlock.id);
		moveBlock.blockColor = startBlock.blockColor;
		Vector3 startScale = new Vector3 (0.4F, 0.4F, 1);
		Vector3 endScale = new Vector3 (0.9F, 0.9F, 1);
		Vector3 startPos = startBlock.transform.position;
		float t = 0; 
		moveBlock.gameObject.transform.position = startPos;
		moveBlock.gameObject.transform.localScale = startScale;
		while (t < 1) {
			yield return null;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0, 3, 10);
			t += Time.deltaTime * 20;
			moveBlock.gameObject.transform.position = Vector3.Lerp (startPos, mousePos, t);
			moveBlock.gameObject.transform.localScale = Vector3.Lerp (startScale, endScale, t);
		}

		moveBlock.gameObject.transform.localScale = endScale;
		canMoveBlock = true;
		blockMoveOutCoroutine = null;
		Application.targetFrameRate = 30;
	}

	IEnumerator MoveBlockMoveIn ()
	{
		
		canMoveBlock = false;
		Application.targetFrameRate = 60;
		Vector3 startScale = new Vector3 (0.9F, 0.9F, 1);
		Vector3 endScale = new Vector3 (0.4F, 0.4F, 1);
		Vector3 endPos = startBlock.transform.position;
		Vector3 startPos = moveBlock.transform.position;
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime * 10;
			moveBlock.transform.position = Vector3.Lerp (startPos, endPos, t);
			moveBlock.transform.localScale = Vector3.Lerp (startScale, endScale, t);
			yield return null;
		}
		moveBlock.gameObject.SetActive (false);
		startBlock.gameObject.SetActive (true);
		startBlock = null;
		moveBlockMoveIn = null;
		Application.targetFrameRate = 30;
	}

	public void InitCamera ()
	{
		//float standard = (720F / 1280F) * 8.4F;
		float defaultCameraSize = 7.45F;
		float standard = (720F / 1280F) * defaultCameraSize;
		float aspect = (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight;
		float size = standard / aspect; 
		if (size < 7.1F) {
			size = 7.1F;
		}
		/*

		if (size < 8.2F) {
			size = 8.2F;
		}
		*/
		Camera.main.orthographicSize = size;

		// 
		//bottomPoint
		blockBg.transform.position = blockBgPos.transform.position;
		//RectTransform blockBgRect = blockBgPos.GetComponent<RectTransform> ();
		//Debug.Log (blockBgRect.transform.position.y + "_" + blockBg.transform.position.y);
		//int adsize = SUGame.Get<SUAdmob> ().GetBannerHeight ();
		//int adsize = 90;
		//Debug.Log ("adsize : " + adsize);
		//blockBgRect.anchoredPosition = new Vector2 (0, adsize);
		//blockBg.transform.position = blockBgRect.transform.position;

		//Debug.Log (blockBgRect.transform.position.y + "_" + blockBg.transform.position.y);

	}

	public void StartNewGame ()
	{
		StartCoroutine (InAnimation (EndInAnimationType.EIT_NEW_GAME));
	}

	public void ShowRatePopup ()
	{
		StartCoroutine (OutAnimation (EndOutAnimationType.EOT_SHOW_RATE_POPUP));
	}

	IEnumerator InAnimation (EndInAnimationType type)
	{
		Application.targetFrameRate = 60;
		UITopControl.instance.ani.SetFloat ("Speed", 1);
		UITopControl.instance.ani.SetInteger ("State", 0);
		UITopControl.instance.ani.Play (0);

		Vector3 bottomStartPos = blockBg.transform.position;
		Vector3 bottomEndPos = blockBgPos.transform.position;
		Vector3 boardStartScle = new Vector3 (0, 0, 0);
		Vector3 boardEndScale = new Vector3 (1, 1, 1);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 3;
			yield return null;
			blockBg.transform.position = Vector3.Lerp (bottomStartPos, bottomEndPos, t);
			board.transform.localScale = Vector3.Lerp (boardStartScle, boardEndScale, t);
		}
		switch (type) {
		case EndInAnimationType.EIT_NEW_GAME: 
			InitNewGame ();
			break;
		}
		Application.targetFrameRate = 30;
	}

	IEnumerator OutAnimation (EndOutAnimationType type)
	{
		Application.targetFrameRate = 60;
		UITopControl.instance.ani.SetFloat ("Speed", -1);
		UITopControl.instance.ani.SetInteger ("State", 0);
		UITopControl.instance.ani.Play (0);

		Vector3 bottomStartPos = blockBg.transform.position;
		Vector3 bottomEndPos = new Vector3 (0, blockBg.transform.position.y - 6, 0);
		Vector3 boardStartScle = board.transform.localScale;
		Vector3 boardEndScale = new Vector3 (0, 0, 0);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 3;
			yield return null;
			blockBg.transform.position = Vector3.Lerp (bottomStartPos, bottomEndPos, t);
			board.transform.localScale = Vector3.Lerp (boardStartScle, boardEndScale, t);
		}
		switch (type) {
		case EndOutAnimationType.EOT_SHOW_LOSE_POPUP: 
			if (SUGame.Get<SURemoteConfig> ().GetDisplayAdLevel () - 1 <= GameManager.replayCount) {
				SUGame.Get<SUAdmob> ().ShowIads ();
				yield return new WaitForSeconds (0.5F);
				while (SUGame.Get<SUAdmob> ().isShowingFullAds == true) {
					yield return new WaitForSecondsRealtime (0.1F);
				}
			} else {
				//Debug.Log (" SUGame.Get<SURemoteConfig> ().GetDisplayAdLevel () + " : " + GameManager.replayCount);
				Debug.Log ("Khong show ad vi getdisplayadlevle - 1 < so lan choi lai");
			}
			AudioManager.instance.PlaySound (AudioClipType.AC_LOSE);
			LosePopup.SetActive (true);
			break;
		case EndOutAnimationType.EOT_SHOW_RATE_POPUP:
			RatePopup.gameObject.SetActive (true);
			break;
		}

		 
		Application.targetFrameRate = 30;
	}

}

[System.Serializable]
public enum EndOutAnimationType
{
	EOT_SHOW_LOSE_POPUP,
	EOT_SHOW_RATE_POPUP
}

[System.Serializable]
public enum EndInAnimationType
{
	EIT_NEW_GAME
}
