
using UnityEngine;
using System.Collections;

using GooglePlayGames;
using UnityEngine.SocialPlatforms;

using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class GPG_LeaderboardManager : MonoBehaviour
{
	#pragma warning disable
	#region PUBLIC_VAR

	
	public static GPG_LeaderboardManager instance;
	public string leaderboard;
	public const string leaderboard_test_score = "CgkIycbR8_0BEAIQAQ";
	public bool isLoginSuccess;
	public string userName, userId;
	public Texture2D avata;
	bool isClickOnLeaderboardButton = false;


	#endregion

	


	#region DEFAULT_UNITY_CALLBACKS

	
	void Awake ()
	{
		leaderboard = GPGSIds.leaderboard_highscore;
		instance = this;
		#if UNITY_ANDROID || UNITY_IPHONE
		// recommended for debugging:
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
			
			.Build ();

		PlayGamesPlatform.InitializeInstance (config);
		PlayGamesPlatform.DebugLogEnabled = true;

		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate ();
		#endif


	}





	#endregion

	


	#region BUTTON_CALLBACKS

	
	/// <summary>
	/// Login In Into Your Google+ Account
	/// </summary>
	public void LogIn (bool _isShowLeaderBoard)
	{
		bool isShowLeaderBoard = _isShowLeaderBoard;
		#if UNITY_ANDROID || UNITY_IPHONE
		PlayGamesPlatform.Instance.Authenticate ((bool success, string log) => {
			if (success) {
				isLoginSuccess = true;
				Debug.Log ("Login Sucess /////////////////////////////////////////////////////////////////");
				userName = Social.localUser.userName; // UserName
				userId = Social.localUser.id; // UserID
				avata = Social.localUser.image; // ProfilePic

				GameManager.dataSave.isLoginedGoogle = true;
				GameManager.instance.SaveData ();
				LoadScore ();
				// update top and show leaderboard  
				if (isShowLeaderBoard == true) {
					//update top 
					OnAddScoreToLeaderBoard (GameManager.dataSave.bestScore.GetValue (), true);
				}

			} else {
				isLoginSuccess = false;
				LoginClicked = false;
				Debug.Log ("Login failed //// " + log);
			}
		});
		#endif
	}

	/// <summary>
	/// Shows All Available Leaderborad
	/// </summary>
	bool LoginClicked = false;

	public void LoadScore ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		((PlayGamesPlatform)Social.Active).LoadScores (leaderboard, LeaderboardStart.PlayerCentered, 50, LeaderboardCollection.Social, LeaderboardTimeSpan.AllTime, (callback) => {
			if (callback.Valid) {
				if (callback.Status == ResponseStatus.Success) {
					try {
						int _score = (int)callback.PlayerScore.value;
						if (_score > GameManager.dataSave.bestScore.GetValue ()) {
							LosePopupControl.instance.UpdateNewBestFromLeaderboard (_score);
						} else {
							OnAddScoreToLeaderBoard (GameManager.dataSave.bestScore.GetValue (), false);
						}
					} catch (System.Exception e) {
						// score khong chinh xac , khong thuoc khoang 
						Debug.Log ("Loi loadscore " + e.ToString ());
					}
				}
			}
		});
		#endif
	}


	public void OnShowLeaderBoard ()
	{
		
		#if UNITY_ANDROID || UNITY_IPHONE
		//		Social.ShowLeaderboardUI (); // Show all leaderboard
		if (isLoginSuccess == false && LoginClicked == false) {
			LoginClicked = true;
			LogIn (true);
		}
		SUGame.Get<SUAnalytics> ().UserViewLeaderBoard ();
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (leaderboard); // Show current (Active) leaderboard
		#endif

	}

	/// <summary>
	/// Adds Score To leader board
	/// </summary>
	public void OnAddScoreToLeaderBoard (long score, bool isShowLeaderboardAfterSuccess)
	{
		bool _isShowLeaderBoardAfterSucces = isShowLeaderboardAfterSuccess;
		#if UNITY_ANDROID || UNITY_IPHONE
		if (Social.localUser.authenticated) {
			Social.ReportScore (score, leaderboard, (bool success) => {
				if (success) {
					Debug.Log ("Update Score Success");
					if (_isShowLeaderBoardAfterSucces == true) {
						OnShowLeaderBoard ();
					}

				} else {
					Debug.Log ("Update Score Fail");
				}
			});
		}
		#endif

	}

	/// <summary>
	/// On Logout of your Google+ Account
	/// </summary>
	public void OnLogOut ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		GameManager.dataSave.isLoginedGoogle = false;
		((PlayGamesPlatform)Social.Active).SignOut ();
		#endif
	}



	#endregion


}

