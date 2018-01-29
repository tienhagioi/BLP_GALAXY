using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

	public int X, Y;
	public SpriteRenderer render;
	public GameObject subGrid, hintGrid;
	public bool haveBlockCell = false;
	public GameObject breakEffectPrefap;
	public ParticleSystem breackEffect;

	Color col1 = new Color (1, 1, 1, 1);
	Color col2 = new Color (1, 1, 1, 0.3F);
	public BlockColor gridCol;
	public SpriteRenderer subRender;
	public Sprite beforeBlockSprite;

	void Awake ()
	{
		render = GetComponent<SpriteRenderer> ();
		subRender = subGrid.GetComponent<SpriteRenderer> ();
	}

	void Start ()
	{
		gameObject.name = "Gird_X" + X + "_Y" + Y;
	}

	public void Clear (float time)
	{
		StartCoroutine (ClearCoroutine (time));
	}

	IEnumerator ClearCoroutine (float time)
	{
		yield return new WaitForSeconds (time);
		GameObject breakEffect = (GameObject)Instantiate (BlockColorDefine.instance.GetBreakEffectByColor (GameplayControl.instance.moveBlock.blockColor), transform.position, Quaternion.Euler (0, 0, 0));
		Destroy (breakEffect, 0.9f);
		haveBlockCell = false;
		subGrid.gameObject.SetActive (false);
		yield return new WaitForSeconds (2f);

	}

	public void ClearInit ()
	{
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				int _x = X - 2 + x;
				int _y = Y - 2 + y;
				if (_x >= 0 && _x < 8 && _y >= 0 && _y < 8) {
					if (GameplayControl.instance.grids [_x, _y].haveBlockCell == false) {						
						GameplayControl.instance.grids [_x, _y].subGrid.SetActive (false);
					}
				}
			}
		}

	}

	public bool[] Init (int _id, bool _createCell, bool isCheck, bool isCreateHint)
	{
		bool canCreateCell = true;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				bool isActive = GameManager.listBlockData [_id] [x, y] == 1 ? true : false;
				if (isActive == true) {
					int _x = X - 2 + x;
					int _y = Y - 2 + y;

					if (_x >= 0 && _x < 8 && _y >= 0 && _y < 8) {
						if (GameplayControl.instance.grids [_x, _y].haveBlockCell == true) {
							canCreateCell = false;
							break;
						}

					} else {						
						canCreateCell = false;
						break;
					}
				}
			}
			if (canCreateCell == false) {
				break;
			}
		}
		if (isCheck == true) {
			return new bool[]{ canCreateCell, false };
		}

		bool createCompleted = false;

		int comboScore = 0;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				bool isActive = GameManager.listBlockData [_id] [x, y] == 1 ? true : false;
				if (isActive == true) {
					int _x = X - 2 + x;
					int _y = Y - 2 + y;
					if (_x >= 0 && _x < 8 && _y >= 0 && _y < 8 && GameplayControl.instance.grids [_x, _y].subGrid.activeInHierarchy == false) {
						if (canCreateCell == true) {
							if (_createCell == true) {
								createCompleted = true;
								GameplayControl.instance.score++;
								GameplayControl.instance.grids [_x, _y].subGrid.SetActive (true);
								GameplayControl.instance.grids [_x, _y].haveBlockCell = true;
								GameplayControl.instance.grids [_x, _y].subRender.color = col1;
								GameplayControl.instance.grids [_x, _y].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
								GameplayControl.instance.grids [_x, _y].gridCol = GameplayControl.instance.moveBlock.blockColor;
								GameplayControl.instance.grids [_x, _y].hintGrid.SetActive (false);
								List<Grid> listGridMatch = GameplayControl.instance.CheckMatch (GameplayControl.instance.grids [_x, _y]);
								if (listGridMatch.Count > 0) {
									Application.targetFrameRate = 60;
									comboScore += listGridMatch.Count > 8 ? 2 : 1;
									for (int i = 0; i < listGridMatch.Count; i++) {
										listGridMatch [i].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
										listGridMatch [i].Clear (i * 0.05F);
									}
								}
							} else {
								// neu co tao subGrid
								createCompleted = false;
								if (isCreateHint == false) {
									// neu la ko hint 
									GameplayControl.instance.grids [_x, _y].subGrid.SetActive (true);
									GameplayControl.instance.grids [_x, _y].subRender.color = col2;
									GameplayControl.instance.grids [_x, _y].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
									List<Grid> listGridMatch = GameplayControl.instance.CheckMatch (GameplayControl.instance.grids [_x, _y]);
									if (listGridMatch.Count > 0) {
										for (int i = 0; i < listGridMatch.Count; i++) {
											listGridMatch [i].beforeBlockSprite = listGridMatch [i].subRender.sprite;
											listGridMatch [i].subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (GameplayControl.instance.moveBlock.blockColor);
										}
									}
								} else {
									// neu la hint 
									GameplayControl.instance.grids [_x, _y].hintGrid.SetActive (true);
								}
							}
						}
					}
				}

			}
		}

		SafeInt scoreAdd = new SafeInt (0);
		switch (comboScore) {
		case 1: 
			scoreAdd = new SafeInt (10);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_1);
			break;
		case 2: 
			scoreAdd = new SafeInt (30);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_2);
			break;
		case 3: 
			scoreAdd = new SafeInt (60);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_3);
			break;
		case 4: 
			scoreAdd = new SafeInt (100);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_4);
			break;
		case 5: 
			scoreAdd = new SafeInt (150);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_5);
			break;
		case 6: 
			scoreAdd = new SafeInt (250);
			AudioManager.instance.PlaySound (AudioClipType.AC_MATCH_5);
			break;
		}
		if (scoreAdd.GetValue () > 0) {
			CreateScoreEffect (scoreAdd.GetValue ());
		}
		bool haveMatch = comboScore > 0 ? true : false;
		bool[] returnList = new bool[2];
		returnList [0] = createCompleted;
		returnList [1] = haveMatch;
		return returnList;
	}

	void CreateScoreEffect (int scoreAdd)
	{
		GameplayControl.instance.score += new SafeInt (scoreAdd);
		GameplayControl.instance.tGetScoreEffect.text = "+" + scoreAdd;
		GameplayControl.instance.getScoreEffect.transform.position = transform.position;
		GameplayControl.instance.tGetScoreEffect.gameObject.SetActive (true);
	}

	public void AppearEffect ()
	{
		StartCoroutine (AppearEffectCoroutine ());
	}

	IEnumerator AppearEffectCoroutine ()
	{
		Vector3 startScale = new Vector3 (0, 0, 0);
		Vector3 endScale = new Vector3 (1F, 1F, 1F);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 5;
			transform.localScale = Vector3.Lerp (startScale, endScale, t);
			yield return null;
		}
		transform.localScale = endScale;
	}

	public void SetGridLose ()
	{
		StartCoroutine (LoseEffect ());
	}

	IEnumerator LoseEffect ()
	{
		render.sprite = BlockColorDefine.instance.sprCellLose;
		float t = 0;
		Color startCol = new Color (1, 1, 1, 1);
		Color endCol = new Color (1, 1, 1, 0);
		while (t < 1) {
			t += Time.deltaTime;
			if (subRender.gameObject.activeInHierarchy == true) {
				subRender.color = Color.Lerp (startCol, endCol, t);
				yield return null;
			}
		}
	}
}
