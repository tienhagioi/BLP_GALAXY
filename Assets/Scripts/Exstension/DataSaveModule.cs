using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaveModule
{
	public int openGameCount;
	public SafeInt currentScore;
	public SafeInt bestScore;
	public List<int> board;
	public List<int> boardColor;
	public List<int> startIds;
	public List<int> startBlockColor;
	public int[] startBlockVisible;
	public bool isSoundOn = true, isMusicOn = true;
	public int comboMatchCount;
	public bool isLoginedGoogle = false;
	public bool isRate = false;
	public long date, todayDate;
	public long numBlockInDayCount;
	public bool isRate5Star = false;
}
