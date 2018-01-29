using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.USecurity;
using System;

public class LoadingControl : MonoBehaviour
{

	// Use this for initialization
	public Block moveBlock;
	public Image loadingBar;
	public GameObject suLib;
	bool haveSulib = false;


	void Start ()
	{
		haveSulib = false;
		StartCoroutine (LoadingCoroutine ());

	}

	IEnumerator LoadingCoroutine ()
	{	
		/*
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime;
			loadingBar.fillAmount = t;
			yield return null;
		}
		*/

		LoadData ();
		GameManager.instance.LoadDataSaved ();
		LoadTutorial ();
		/*
		if (GameManager.dataSave.isLoginedGoogle == true) {
			GPG_LeaderboardManager.instance.LogIn (false);
		}
		*/
		AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync ("scn_play", LoadSceneMode.Single);
		while (!loadSceneAsync.isDone) {
			loadingBar.fillAmount = loadSceneAsync.progress;
			if (loadSceneAsync.progress >= 0.5F && haveSulib == false) {
				haveSulib = true;
				Instantiate (suLib, transform.position, Quaternion.Euler (0, 0, 0));

			}
			if (loadSceneAsync.progress >= 0.8F) {
				loadingBar.fillAmount = 1;
			}
			yield return null;
		}

		//SceneManager.LoadScene ("scn_play");
	}

	void LoadData ()
	{
		TextAsset ta = Resources.Load ("BlockData") as TextAsset;
		InitBlockData (ta.text);
	}

	public static void InitBlockData (string dataStr)
	{
		
		//Debug.Log (ta.text);

		BlockData data;
		try {
			data = JsonUtility.FromJson<BlockData> (dataStr);
		} catch (System.Exception e) {
			//Debug.Log (e + "Khong the pars json data : " + dataStr); 
			//return;
			dataStr = AES.Decrypt (dataStr, StringValuables.password);
			try {
				data = JsonUtility.FromJson<BlockData> (dataStr);
			} catch (System.Exception ee) {
				Debug.Log (e + "Khong the parse json data : " + dataStr + ":" + ee.ToString ()); 
				return;
			}
		}
		if (data != null) {
			if (GameManager.listBlockData != null) {
				GameManager.listBlockData.Clear ();
			} else {
				GameManager.listBlockData = new List<int[,]> ();
			}
			if (GameManager.listBlockDataStruct == null) {
				GameManager.listBlockDataStruct = new List<BlockDataStruct> ();
			} else {
				GameManager.listBlockDataStruct.Clear ();
			}
			if (GameManager.listBlockDataPercent == null) {
				GameManager.listBlockDataPercent = new List<int> ();
			} else {
				GameManager.listBlockDataPercent.Clear ();
			}

			int percentNum = 0;
			for (int i = 0; i < data.blockData.Count; i++) {
				BlockDataStruct dataStruct = new BlockDataStruct ();
				int[,] blockGrid = new int[5, 5];
				string mnDataStr = data.blockData [i];
				string[] dataSplit = mnDataStr.Split (new char[]{ '_' }, 2);
				string percent = dataSplit [1];

				//GameManager.listBlockDataPercent.Add (int.Parse (percent));
				string[] mnDataRow = dataSplit [0].Split (new char[]{ ',' }, 5);
				string str = "";
				for (int j = 0; j < mnDataRow.Length; j++) {
					for (int k = 0; k < mnDataRow [j].Length; k++) {
						blockGrid [j, k] = int.Parse (mnDataRow [j] [k].ToString ());
						str = str + blockGrid [j, k];
					}
					str = str + "/n";
				}
				//Debug.Log ("str " + i + " : " + str);
				GameManager.listBlockData.Add (blockGrid);
				// add to dataStruct
				dataStruct.grid = blockGrid;
				dataStruct.pos = i;
				dataStruct.percent = float.Parse (percent);
				dataStruct.percentNumMin = percentNum + 1;
				percentNum = percentNum + (int)(dataStruct.percent * 100);
				dataStruct.percentNumMax = percentNum;
				GameManager.listBlockDataStruct.Add (dataStruct);
			}
			Debug.Log ("da thay data moi");
		}



	}

	public  void LoadTutorial ()
	{
		TextAsset ta = Resources.Load ("Tutorial") as TextAsset;
		Tutorial _tutorialData = JsonUtility.FromJson<Tutorial> (ta.text);

		GameManager.listTutorial = _tutorialData.tutorial;
		Debug.Log ("tutorial : " + _tutorialData.tutorial [0].grid [0]);
	}
}

[System.Serializable]
public class BlockData
{
	public List<string> blockData;

}

[System.Serializable]
public class BlockDataStruct
{
	public float percent;
	public int pos;
	public int percentNumMax, percentNumMin;
	public int[,] grid;
}