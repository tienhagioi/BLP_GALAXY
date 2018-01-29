using UnityEngine;
using System.Collections;
using Firebase.Analytics;
using Firebase;
using System;

public class SUAnalytics : BaseSUUnit
{
	//DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	bool firebaseInitialized = false;

	public override void Init ()
	{
		firebaseInitialized = false;
		if (SUGame.haveDependency == true) {
			InitializeFirebase ();
		}


	}


	void InitializeFirebase ()
	{
		try {
			Debug.Log ("Enabling data collection.");
			FirebaseAnalytics.SetAnalyticsCollectionEnabled (true);

			Debug.Log ("Set user properties.");
			// Set the user's sign up method.
			FirebaseAnalytics.SetUserProperty (
				FirebaseAnalytics.UserPropertySignUpMethod,
				"Google");
			// Set the user ID.
			FirebaseAnalytics.SetUserId ("uber_user_510");
			firebaseInitialized = true;
		} catch (System.Exception e) {
			Debug.Log ("Loi init firebase : " + e.ToString ());
		}
	}

	#region LogEventMethods

	public void BuyBooster (string screen, string name, int amount, int price, int level)
	{
		if (firebaseInitialized == false)
			return;
		string buy_booster = "Buy_Booster";
		FirebaseAnalytics.LogEvent (buy_booster, new Parameter[] {
			new Parameter ("screen", screen),
			new Parameter ("item_name", name),
			new Parameter ("number_item", amount),
			new Parameter ("number_coin", price),
			new Parameter ("level", level)
		});
	}

	public void OutofLife (int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string outoflife = "Out_Of_Life";
		FirebaseAnalytics.LogEvent (outoflife, new Parameter ("level", level));
		
	}

	public void LevelComplete (int level, int moveRemain)
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			string levelComplete = "Level_Completed";
			FirebaseAnalytics.LogEvent (levelComplete, new Parameter[] {
				new Parameter ("Level", level),
				new Parameter ("Move_Remain", moveRemain)
			});
		} catch (System.Exception e) {
			Debug.Log ("Loi log event levelCompleted " + e.ToString ());
		}
	}

	public void LevelFail (int level)
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			string levelFail = "Level_Failed";
			FirebaseAnalytics.LogEvent (levelFail, new Parameter ("Level", level));
		} catch (System.Exception e) {
			Debug.Log ("Loi log event LevelFail " + e.ToString ());
		}
	}

	public void LevelRetry (int level)
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			string levelRetry = "Level_Retry";
			FirebaseAnalytics.LogEvent (levelRetry, new Parameter ("Level", level));
		} catch (System.Exception e) {
			Debug.Log ("Loi log event LevelRetry : " + e.ToString ());
		}
	}

	public void LevelStart (int level)
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			string levelStart = "Level_Start";
			FirebaseAnalytics.LogEvent (levelStart, new Parameter ("Level", level));
		} catch (System.Exception e) {
			Debug.Log ("Loi log event LevelStart : " + e.ToString ());
		}
	}

	public void UserViewLeaderBoard ()
	{
		if (firebaseInitialized == false)
			return;
		try {
			FirebaseAnalytics.LogEvent ("ViewLeaderBoard", new Parameter ("HighScore", GameManager.dataSave.bestScore.GetValue ()));
		} catch (System.Exception e) {
			Debug.Log ("Loi log event UserViewLeaderBoard : " + e.ToString ());
		}
	}

	public void UserShare ()
	{
		if (firebaseInitialized == false)
			return;
		FirebaseAnalytics.LogEvent ("Share", new Parameter ("HighScore", GameManager.dataSave.bestScore.GetValue ()));
	}

	public void UseBooster (string scene, string name, int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string useBooster = "Use_Boosters";
		FirebaseAnalytics.LogEvent (useBooster, new Parameter[] {
			new Parameter ("Type", name),
			new Parameter ("Level", level),
			new Parameter ("Screen", scene)
		});
	}

	public void Rating (string from, string action)
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			string rate = "Rating";
			FirebaseAnalytics.LogEvent (rate, new Parameter[] {
				new Parameter ("From", from),
				new Parameter ("Action", action)
			});
		} catch (System.Exception e) {
			Debug.Log ("Loi log event Rating : " + e.ToString ());
		}
	}

	public void GetFreeCoinAtGameplay (string Action, int level, int coin, string scene)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string getFreeCoin = "GetFreeCoin_in_GamePlay";
		FirebaseAnalytics.LogEvent (getFreeCoin, new Parameter[] {
			new Parameter ("Action", Action),
			new Parameter ("Level", level),
			new Parameter ("Coin", coin),
			new Parameter ("Scene", scene)
		});
	}

	public void LastLogin ()
	{
		if (firebaseInitialized == false)
			return;
		try {
			if (GameManager.isNetworkConnected == false) {
				return;
			}
			//int date = 0;
			DateTime now = DateTime.Now;
			int year = now.Year;
			int month = now.Month;
			int day = now.Day;
			//date = year * 10000 + month * 100 + day;
			string date = year.ToString ("0000") + month.ToString ("00") + day.ToString ("00"); 
			Debug.Log ("date la : " + date);
			string lastLoginStr = "Last_Login";
			FirebaseAnalytics.LogEvent (lastLoginStr, new Parameter[]{ new Parameter ("date", date) });
		} catch (System.Exception e) {
			Debug.Log ("Loi log event LastLogin : " + e.ToString ());
		}
	}

	public void GetFreeCoinAtWorld (string Action, int coin)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string getFreecoin = "GetFreeCoin_in_WorldScene";
		FirebaseAnalytics.LogEvent (getFreecoin, new Parameter[] {
			new Parameter ("Action", Action),
			new Parameter ("Coin", coin)
		});
	}

	public void BuyIAP (string scene, string price, int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string inapp = "Purchase_IAP";
		FirebaseAnalytics.LogEvent (inapp, new Parameter[] {
			new Parameter ("Scene", scene),
			new Parameter ("Money", price),
			new Parameter ("Level", level)
		});
	}

	public void ClickOnAdExchange (string scene)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string clickAd = "Click_on_Ad_Exchange";
		FirebaseAnalytics.LogEvent (clickAd, new Parameter ("Scene", scene));
	}

	public void RetryWithFreeBooster (int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string retry = "Retry_With_Free_Booster";
		FirebaseAnalytics.LogEvent (retry, new Parameter ("Level", level));
	}

	public void DailyReward ()
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string Daily_Rewarded = "Daily_Rewarded";
		FirebaseAnalytics.LogEvent (Daily_Rewarded);
	}

	public void GiveUP (int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string giveup = "Give_Up";
		FirebaseAnalytics.LogEvent (giveup, new Parameter ("Level", level));
	}

	public void ExitGame (int level)
	{
		if (firebaseInitialized == false)
			return;
		if (GameManager.isNetworkConnected == false) {
			return;
		}
		string Exit = "Exit";
		FirebaseAnalytics.LogEvent (Exit, new Parameter ("Level", level));
	}

	#endregion

}
