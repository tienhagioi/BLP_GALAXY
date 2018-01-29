using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable
public class UITopControl : MonoBehaviour
{
	public Button bSound;
	public static UITopControl instance;
	public Text tScore, tBest;
	[HideInInspector]
	public Animator ani;
	public bool isShowingPauseMenu = false;

	void Awake ()
	{
		instance = this;
		ani = GetComponent<Animator> ();

	}

	void Start ()
	{
		bSound.image.sprite = AudioManager.instance.GetSoundStatus ();
	}

	int showPauseCount = 0;
	float lastTimeClickPause = 0;
	float showPausePeriod = 0;

	public void ShowPauseMenu ()
	{
		showPauseCount++;
		AudioManager.instance.PlaySound (AudioClipType.AC_BUTTON);
		if (isShowingPauseMenu == false) {
			isShowingPauseMenu = true;

			ani.SetInteger ("State", 1);
			ani.SetFloat ("Speed", 1F);
			ani.Play (0);
		} else {
			isShowingPauseMenu = false;


			ani.SetInteger ("State", 1);
			ani.SetFloat ("Speed", -1F);
		}

	}

	public void RestartGame ()
	{
		
		AudioManager.instance.PlaySound (AudioClipType.AC_BUTTON);
		GameplayControl.instance.InitNewGame ();
		ShowPauseMenu ();
	}

	public void bSoundClick ()
	{
		bSound.image.sprite = AudioManager.instance.ChangeSoundStatus ();
	}

	public void StopAni ()
	{
		ani.SetFloat ("Speed", 0F);
	}
}
