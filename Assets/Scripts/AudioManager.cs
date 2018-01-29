using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	public Sprite sprSoundOn, sprSoundOff, sprMusicOn, sprMusicOff;
	Dictionary<AudioClipType,AudioClip> audioDic;
	public List<AudioStruct> listAudioStruct;

	void Awake ()
	{
		instance = this;
		audioDic = new Dictionary<AudioClipType, AudioClip> ();
		for (int i = 0; i < listAudioStruct.Count; i++) {
			if (!audioDic.ContainsKey (listAudioStruct [i].type)) {
				audioDic.Add (listAudioStruct [i].type, listAudioStruct [i].clip);
			}
		}
	}

	void Start ()
	{
		AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1 : 0;
		AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
		PlayMusic (AudioClipType.AC_MAIN_MUSIC);
	}

	public AudioSource AS_SOUND, AS_MUSIC;

	[System.Serializable]
	public struct AudioStruct
	{
		public AudioClipType type;
		public AudioClip clip;
	}



	public Sprite GetSoundStatus ()
	{
		return GameManager.dataSave.isSoundOn == true ? sprSoundOn : sprSoundOff;
	}

	public Sprite GetMusicStatus ()
	{
		return GameManager.dataSave.isMusicOn == true ? sprMusicOn : sprMusicOff;
	}


	public  Sprite ChangeSoundStatus ()
	{
		GameManager.dataSave.isSoundOn = !GameManager.dataSave.isSoundOn;
		AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1 : 0;
		GameManager.dataSave.isMusicOn = !GameManager.dataSave.isMusicOn;
		AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
		GameManager.instance.SaveData ();
		return GetSoundStatus ();
	}

	public  Sprite ChangeMusicStatus ()
	{
		GameManager.dataSave.isMusicOn = !GameManager.dataSave.isMusicOn;
		AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
		GameManager.dataSave.isSoundOn = !GameManager.dataSave.isSoundOn;
		AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1 : 0;
		GameManager.instance.SaveData ();
		return GetSoundStatus ();
	}

	public void PlaySound (AudioClipType type)
	{
		AS_SOUND.PlayOneShot (audioDic [type]);
	}

	public void PlayMusic (AudioClipType type)
	{
		AS_MUSIC.Stop ();
		AS_MUSIC.clip = audioDic [type];
		AS_MUSIC.Play ();
	}
}


[System.Serializable]
public enum AudioClipType
{
	AC_MAIN_MUSIC,
	AC_MATCH_1,
	AC_MATCH_2,
	AC_MATCH_3,
	AC_MATCH_4,
	AC_MATCH_5,
	AC_TAKE_BLOCK,
	AC_PUT_BLOCK,
	AC_BUTTON,
	AC_LOSE_EFFECT,
	AC_LOSE,
	AC_COOL,
	AC_GOOD,
	AC_EXCELENT
}