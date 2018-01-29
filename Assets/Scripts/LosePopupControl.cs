using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LosePopupControl : MonoBehaviour
{

	public Button bPlay, bRank, bRate, bShare;
	public Text tScore, tBest;
	Animator ani;
	float score;
	bool canReplay = false;
	public static LosePopupControl instance;

	void Awake ()
	{
		instance = this;
		ani = GetComponent<Animator> ();
	}

	public void UpdateNewBestFromLeaderboard (int _score)
	{
		GameplayControl.instance.bestScore = new SafeInt (_score);
		GameManager.dataSave.bestScore = GameplayControl.instance.bestScore;
		tBest.text = GameplayControl.instance.bestScore.ToString ();
		GameManager.instance.SaveData ();
	}

	void OnEnable ()
	{
		Application.targetFrameRate = 60;
		canReplay = false;
		score = 0;
		ani.SetFloat ("Speed", 1);
		ani.Play (0);
		StartCoroutine (IncreaseScore ());
		tBest.text = GameplayControl.instance.bestScore.ToString ();
		if (GPG_LeaderboardManager.instance.isLoginSuccess == true) {
			GPG_LeaderboardManager.instance.OnAddScoreToLeaderBoard (GameplayControl.instance.bestScore.GetValue (), false);
		}
		//SUGame.Get<SUAnalytics> ().LevelFail (GameplayControl.instance.score.GetValue ());
		//StartCoroutine (ShowFullAds ());
		if (GameManager.dataSave.isRate5Star == true) {
			bRate.image.color = new Color (1, 1, 1, 0.5F);
		}

	}

	void ClearGameData ()
	{
		GameplayControl.instance.score = new SafeInt (0);

		for (int i = 0; i < 3; i++) {
			int rnd = GameManager.GetRandomBlockId ();
			GameplayControl.instance.listStartBlock [i].InitBlock (rnd);
			GameplayControl.instance.listStartBlock [i].blockColor = (BlockColor)Random.Range (0, BlockColorDefine.instance.listBlockColorStruct.Count);
			GameplayControl.instance.listStartBlock [i].haveMatchPos = true;
			GameplayControl.instance.listStartBlock [i].gameObject.SetActive (true);
			GameplayControl.instance.listStartBlock [i].NewBlockEffect ();
			//GameplayControl.instance.score = 0;
		}
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {				
				GameplayControl.instance.grids [x, y].subGrid.SetActive (false);
				GameplayControl.instance.grids [x, y].haveBlockCell = false;
				GameplayControl.instance.grids [x, y].render.sprite = BlockColorDefine.instance.sprGrid;
				GameplayControl.instance.grids [x, y].subRender.color = new Color (1, 1, 1, 1);
				GameplayControl.instance.grids [x, y].hintGrid.SetActive (false);
			}
		}
		GameplayControl.instance.InitDataAndSave ();
	}

	IEnumerator IncreaseScore ()
	{
		int _score = GameplayControl.instance.score.GetValue ();
		ClearGameData ();
		yield return new WaitForSeconds (1F);
		while (Mathf.RoundToInt (score) < _score) {
			score += ((float)_score - score) / 8;
			tScore.text = Mathf.RoundToInt (score).ToString ();
			yield return null;
		}
		canReplay = true;
		Application.targetFrameRate = 30;

	}

	IEnumerator ShowFullAds ()
	{
		yield return new WaitForSeconds (1F);
		SUGame.Get<SUAdmob> ().ShowIads ();
	}

	public void bPlayClick ()
	{
		if (canReplay == true) {
			GameManager.replayCount++;
			AudioManager.instance.PlaySound (AudioClipType.AC_BUTTON);
			StartCoroutine (StartNewGame ());
		}
	}

	IEnumerator StartNewGame ()
	{
		Application.targetFrameRate = 60;
		ani.SetFloat ("Speed", -1);
		//ani.Play (0);
		yield return new WaitForSeconds (ani.GetCurrentAnimatorClipInfo (0) [0].clip.averageDuration);
		GameplayControl.instance.StartNewGame ();

		/*
		UITopControl.instance.ani.SetFloat ("Speed", 1);
		UITopControl.instance.ani.SetInteger ("State", 0);
		UITopControl.instance.ani.Play (0);
		*/
		//SUGame.Get<SUAnalytics> ().LevelStart (1);
		gameObject.SetActive (false);
		Application.targetFrameRate = 30;
	}

	public void bLeaderboardClick ()
	{
		GPG_LeaderboardManager.instance.OnShowLeaderBoard ();
	}

	public void bRateClick ()
	{
		if (GameManager.dataSave.isRate5Star == true) {
			return;
		}
		AudioManager.instance.PlaySound (AudioClipType.AC_BUTTON);
		//GameManager.dataSave.isRate = true;
		//GameManager.instance.SaveData ();
		//Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);
		gameObject.SetActive (false);
		GameplayControl.instance.RatePopup.isOpenFromLosePopup = true;
		GameplayControl.instance.ShowRatePopup ();

	}


	public void bShareClick ()
	{
		AudioManager.instance.PlaySound (AudioClipType.AC_BUTTON);
		//NativeShare.instance.Share ();
		string ScreenshotName = "screenShot.png";
		string text = "Cái này là text mặc định";
		string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
		if (File.Exists (screenShotPath))
			File.Delete (screenShotPath);
		Application.CaptureScreenshot (ScreenshotName);
		//ScreenCapture.CaptureScreenshot (ScreenshotName);

		StartCoroutine (delayedShare (screenShotPath, text));
	}

	IEnumerator delayedShare (string screenShotPath, string text)
	{
		Application.targetFrameRate = 60;
		while (!File.Exists (screenShotPath)) {
			yield return new WaitForSecondsRealtime (0.05f);
		}
		SUGame.Get<SUAnalytics> ().UserShare ();
		NativeShare.Share (text, screenShotPath, "", "", "image/png", true, "");
		Application.targetFrameRate = 30;
	}

	public void StopAni ()
	{
		ani.SetFloat ("Speed", 0F);
	}
}
