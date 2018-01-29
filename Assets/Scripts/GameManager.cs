using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static int replayCount;
	public static List<int[,]> listBlockData;
	public static List<int> listBlockDataPercent;
	public static DataSaveModule dataSave;
	public static List<TutorialModule> listTutorial;
	public static List<int> listPercent = new List<int> ();
	public static GameManager instance;
	public static List<BlockDataStruct> listBlockDataStruct;
	public static bool isPopupRateShowed = false;

	public GameManager ()
	{
		instance = this;
	}

	public static bool isNetworkConnected {
		get {
			return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
		}
	}



	public IEnumerator LoadDataSaveCoroutine ()
	{
		Debug.Log ("load data coroutine");
		isPopupRateShowed = false;
		replayCount = 0;
		string str = PlayerPrefs.GetString ("fuckyou");

		//str = "";
		bool isReplaceData = false;
		if (str != null && str != "") {
			//Debug.Log (str);
			try {
				dataSave = JsonUtility.FromJson<DataSaveModule> (str);
			} catch (System.Exception e) {
				
				isReplaceData = true;
				Debug.Log ("Thay data : " + e);
				if (dataSave == null) {
					
					try {
						byte[] decodedStringToByte = System.Convert.FromBase64String (str);
						string decodedString = System.Text.Encoding.UTF8.GetString (decodedStringToByte);
						dataSave = JsonUtility.FromJson<DataSaveModule> (decodedString);
						Debug.Log ("current Score = " + dataSave.currentScore.GetValue ());
						string hash = PlayerPrefs.GetString ("Data_Hash");
						string hash_check = Md5Sum (decodedString + dataSave.bestScore.ToString ());
						// check hash to detect hack
						if (!hash.Equals (hash_check)) {
							Debug.Log ("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa Phat hien nghi van hack");
							dataSave.bestScore = new SafeInt ((int)dataSave.numBlockInDayCount - (int)dataSave.todayDate);
							GameManager.instance.SaveData ();
						}	
						SaveData ();
					} catch (System.Exception ee) {
						Debug.Log ("aaaaaaaaaaa tao moi data " + ee.ToString ());
						dataSave.isMusicOn = true;
						dataSave.isSoundOn = true;
						dataSave = new DataSaveModule ();
						dataSave.date = System.DateTime.Now.Ticks;
						dataSave.bestScore = new SafeInt (0);
						dataSave.currentScore = new SafeInt (0);
						dataSave.board = new List<int> ();
						dataSave.boardColor = new List<int> ();
						dataSave.startIds = new List<int> ();
						dataSave.startBlockColor = new List<int> ();
						for (int i = 0; i < 64; i++) {
							dataSave.board.Add (0);
							dataSave.boardColor.Add (0);
							if (i < 3) {
								dataSave.startIds.Add (-1);
								dataSave.startBlockColor.Add (-1);
							}
						}
						dataSave.startBlockVisible = new int[]{ 1, 1, 1 };
						dataSave.openGameCount = 0;
						SaveData ();
					}
				}
			}
			dataSave.openGameCount++;
			if (dataSave.date == 0) {
				dataSave.date = System.DateTime.Now.Ticks;
				SaveData ();
				Debug.Log ("data cu chua co date  , tao moi date");
			} else {
				if (isReplaceData == false) {
					string hash = PlayerPrefs.GetString ("Data_Hash");
					string hash_check = Md5Sum (str + dataSave.bestScore.ToString ());
					// check hash to detect hack
					if (!hash.Equals (hash_check)) {
						Debug.Log ("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa Phat hien nghi van hack");
						dataSave.bestScore = new SafeInt ((int)(dataSave.numBlockInDayCount - dataSave.todayDate));
						GameManager.instance.SaveData ();
					}	
				}
			}

		} else {
			dataSave = new DataSaveModule ();
			dataSave.date = System.DateTime.Now.Ticks;
			dataSave.bestScore = new SafeInt (0);
			dataSave.currentScore = new SafeInt (0);
			dataSave.board = new List<int> ();
			dataSave.boardColor = new List<int> ();
			dataSave.startIds = new List<int> ();
			dataSave.startBlockColor = new List<int> ();
			for (int i = 0; i < 64; i++) {
				dataSave.board.Add (0);
				dataSave.boardColor.Add (0);
				if (i < 3) {
					dataSave.startIds.Add (-1);
					dataSave.startBlockColor.Add (-1);
				}
			}
			dataSave.startBlockVisible = new int[]{ 1, 1, 1 };
			dataSave.openGameCount = 0;
			dataSave.isMusicOn = true;
			dataSave.isSoundOn = true;
			SaveData ();
		}
		//listBlockId = new List<int> ();

		/*

		listPercent.Add (1);
		listPercent.Add (3);
		listPercent.Add (3);
		listPercent.Add (3);
		for (int i = 0; i < 5; i++) {
			listPercent.Add (5);
		}
		for (int i = 0; i < 10; i++) {
			listPercent.Add (10);
		}
		*/
		yield return null;
	}




	public void LoadDataSaved ()
	{
		
		Debug.Log ("load data");
		StartCoroutine (LoadDataSaveCoroutine ());
	}

	public void SaveData ()
	{
		StartCoroutine (SaveDataCoroutine ());
	}

	IEnumerator SaveDataCoroutine ()
	{
		dataSave.todayDate = System.DateTime.Now.Ticks;
		dataSave.numBlockInDayCount = dataSave.todayDate + dataSave.bestScore.GetValue ();
		string str = JsonUtility.ToJson (dataSave);
		byte[] toByteEncode = System.Text.Encoding.UTF8.GetBytes (str);
		string encodedText = System.Convert.ToBase64String (toByteEncode);
		Debug.Log ("current Score save : " + dataSave.currentScore.GetValue ());
		//		Debug.Log ("str save" + str);
		PlayerPrefs.SetString ("Data_Hash", Md5Sum (str + dataSave.bestScore.ToString ()));
		PlayerPrefs.SetString ("fuckyou", encodedText);
		PlayerPrefs.Save ();
		yield return null;
	}


	/*
	public static List<int> listBlockId;

	public static int GetRandomBlockId ()
	{
		listBlockId.Clear ();
		listPercent.Shuffle ();
		int percent = listPercent [0];
		//Debug.Log ("percent : " + percent);
		for (int i = 0; i < listBlockDataPercent.Count; i++) {
			if (listBlockDataPercent [i] == percent) {
				listBlockId.Add (i);
			}
		}
		listBlockId.Shuffle ();
		if (listBlockId.Count > 0) {
			return listBlockId [0];
		} else {
			Debug.Log ("list rong " + percent);
			return 0;
		}

	}
	*/
	public static int GetRandomBlockId ()
	{
		if (SUGame.Get<SURemoteConfig> () != null) {
			if (SUGame.Get<SURemoteConfig> ().randomLogic == StringValuables.su) {
				bool haveZ = false;
				if (GameplayControl.instance.isGetNewHighScore == true && GameplayControl.instance.score.GetValue () >= 500) {
					haveZ = true;
				}

				int rnd = haveZ == true ? Random.Range (0, 10000) : Random.Range (0, 9300);
				//Debug.Log ("have z : " + haveZ + " rnd : " + rnd);
				for (int i = 0; i < listBlockDataStruct.Count; i++) {
					BlockDataStruct blockData = listBlockDataStruct [i];
					if (rnd >= blockData.percentNumMin && rnd <= blockData.percentNumMax) {
						return blockData.pos;
					}
				}
				//Debug.Log ("khong tim thay " + rnd + ", return 0");
				return 0;
			} else if (SUGame.Get<SURemoteConfig> ().randomLogic == StringValuables.sonat) {
				return SonatLogic.GetRandomBlockId (GameManager.dataSave.currentScore.GetValue ());
			} else
				return 0;
		} else {
			return SonatLogic.GetRandomBlockId (GameManager.dataSave.currentScore.GetValue ());
		}
	}

	public static string Md5Sum (string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
		byte[] bytes = ue.GetBytes (strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
		byte[] hashBytes = md5.ComputeHash (bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
		}

		return hashString.PadLeft (32, '0');
	}
}

