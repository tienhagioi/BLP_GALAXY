using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateControl : MonoBehaviour
{
	public Toggle tgNeverShowAgain;
	public GameObject ratePopup2;
	Animator ani, ratePopup2Ani;
	public static RateControl instance;
	public bool isOpenFromLosePopup = false;
	bool isCloseAll = true;
	string logFrom = "popup rating";

	void Awake ()
	{
		instance = this;
		ani = GetComponent<Animator> ();
		ratePopup2Ani = ratePopup2.GetComponent<Animator> ();
	}

	void Enable ()
	{
		isCloseAll = true;
		Application.targetFrameRate = 60;
		ani.SetBool ("Show", true);
		Time.timeScale = 0;
		tgNeverShowAgain.isOn = false;

	}

	public void Close (Animator _ani)
	{
		if (tgNeverShowAgain.isOn == true) {
			GameManager.dataSave.isRate = true;
			GameManager.instance.SaveData ();
			logFrom = isOpenFromLosePopup == true ? "popup gameover" : "popup rating";
			Debug.Log ("LogFrom : " + logFrom);
			SUGame.Get<SUAnalytics> ().Rating (logFrom, "Don't ask me again");
		}
		Time.timeScale = 1;
		if (_ani == ratePopup2Ani) {
			// neu la dong popup 2
			ratePopup2Ani.SetBool ("Show", false);
			isCloseAll = true;
		} 
		_ani.SetBool ("Show", false);
		if (isCloseAll == true) {
			if (isOpenFromLosePopup == true) {				
				Invoke ("ShowLosePopup", 1F);
			}
		}
	}

	void ShowLosePopup ()
	{

		LosePopupControl.instance.gameObject.SetActive (true);
		isOpenFromLosePopup = false;
	}

	public void Rate ()
	{
		Application.OpenURL (SUGame.Get<SURemoteConfig> ().GetRateUrl ());
		GameManager.dataSave.isRate = true;
		GameManager.dataSave.isRate5Star = true;
		GameManager.instance.SaveData ();
		Close (ani);
		logFrom = isOpenFromLosePopup == true ? "popup gameover" : "popup rating";
		Debug.Log ("LogFrom : " + logFrom);
		SUGame.Get<SUAnalytics> ().Rating (logFrom, "5 star");

	}

	public void FeedBack ()
	{
		isCloseAll = false;
		ratePopup2.SetActive (true);
		ratePopup2Ani.SetBool ("Show", true);
		Close (ani);

	}

	public void NotNow ()
	{
		GameManager.dataSave.isRate = true;
		//tgNeverShowAgain.isOn = false;
		Close (ratePopup2Ani);

	}

	public void EmailUs ()
	{
		logFrom = isOpenFromLosePopup == true ? "popup gameover" : "popup rating";
		Debug.Log ("LogFrom : " + logFrom);
		SUGame.Get<SUAnalytics> ().Rating (logFrom, "1-4 star");
		//email Id to send the mail to
		//SoundBase.instance.Click();
		string email = "sugame293@gmail.com";
		//subject of the mail
		string subject = MyEscapeURL ("FEEDBACK");
		//body of the mail which consists of Device Model and its Operating System
		string body = MyEscapeURL ("Please leave your feedback here: \n\n\n\n\n" +
		              "_____________________" +
		              "\n\nPlease Do Not Modify This:\n\n" +
		              "Model: " + SystemInfo.deviceModel + "\n" +
		              "OS: " + SystemInfo.operatingSystem + "\n" +
		              "_____________________");
		//Open the Default Mail App
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
		GameManager.dataSave.isRate = false;
		GameManager.dataSave.openGameCount = 0;
		tgNeverShowAgain.isOn = false;
		GameManager.instance.SaveData ();
		//GetComponent<ManagePopupEffect>().Close();
		//PopupSystem.instance.Win();
		Close (ratePopup2Ani);

	}

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL (url).Replace ("+", "%20");
	}

}
