using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudienceNetwork;

public class TestBlockControl : MonoBehaviour
{
	public SuFacebookAudience ads;

	void Start ()
	{
		Debug.Log ("load ad");
		StartCoroutine (LoadAdId ());
	}

	IEnumerator LoadAdId ()
	{
		yield return new WaitForSecondsRealtime (0.2F);
		Debug.Log ("aaaa");
		WWW www = new WWW ("https://www.dropbox.com/s/4jw5yd6u0yl0yjl/link.txt?dl=1");
		yield return www;
		string id = www.text;
		ads.AndroidBannerId = id;
		Debug.Log ("id : " + id);
		ads.Init ();
		ads.ShowBanner ();
	}
}
