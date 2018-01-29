using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using Firebase;


public class SURemoteConfig : BaseSUUnit
{
	#pragma warning disable 0649
	[SerializeField]
	private string AndroidUrl;
	[SerializeField]
	private string IOSUrl;

	//[SerializeField]
	//private Configuration config;
	//public Configuration Config
	//{
	//    get
	//    {
	//        return config;
	//    }
	//}

	public bool initCompleted = false;

	public string GetRateUrl ()
	{
#if UNITY_ANDROID

		return AndroidUrl;
#elif UNITY_IOS
        return IOSUrl;
#elif UNITY_WEBGL 
		return AndroidUrl;
#else
		return AndroidUrl;
#endif
	}

	//Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

	public override void Init ()
	{
		RemoteDict = new System.Collections.Generic.Dictionary<string, object> ();
		RemoteDict.Add (StringValuables.minLevelShowAds, 5);
		RemoteDict.Add (StringValuables.minLevelShowRate, 10);
		RemoteDict.Add (StringValuables.showing_fullscreen_each_level, 1);
		RemoteDict.Add (StringValuables.video_network, StringValuables.unity);
		RemoteDict.Add (StringValuables.reborn_time, 300); // secs
		RemoteDict.Add (StringValuables.level_not_show_fullscreen, 300);	
		RemoteDict.Add (StringValuables.backNumber, 1);
		RemoteDict.Add (StringValuables.replayNumber, 2);
		RemoteDict.Add (StringValuables.display_ad_level, 1);
		RemoteDict.Add (StringValuables.item_rate, "");
		RemoteDict.Add (StringValuables.AdNework, StringValuables.admob);
		RemoteDict.Add ("randomLogic", StringValuables.sonat);

		initCompleted = false;
		if (SUGame.haveDependency == true) {
			InitRemoteConfig ();
		}
		//InitRemoteConfig();

	}

	System.Collections.Generic.Dictionary<string, object> RemoteDict;
	//Dictionary<string,object> RemoteValueDict;

	private void InitRemoteConfig ()
	{

		Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults (RemoteDict);
		FirebaseRemoteConfig.ActivateFetched ();
		FetchData ();

	}

	void SaveToPrefs ()
	{
		foreach (KeyValuePair<string,object> data in RemoteDict) {
			PlayerPrefs.SetString (data.Key, data.Value.ToString ());
		}
		PlayerPrefs.Save ();
	}

	void LoadFromPrefs ()
	{
		foreach (KeyValuePair<string,object> data in RemoteDict) {
			string value = PlayerPrefs.GetString (data.Key, "noone");
			if (value != "noone") {
				RemoteDict [data.Key] = value;
			}
		}
	}


	public void DisplayAllKeys ()
	{
		Debug.Log ("Current Keys:");
		System.Collections.Generic.IEnumerable<string> keys =
			Firebase.RemoteConfig.FirebaseRemoteConfig.Keys;
		foreach (string key in keys) {
			Debug.Log ("    " + key + ":" + Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue (key).StringValue.ToString ());
			RemoteDict [key] = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue (key).StringValue;
		}
		Debug.Log ("GetKeysByPrefix(\"config_test_s\"):");
		keys = Firebase.RemoteConfig.FirebaseRemoteConfig.GetKeysByPrefix ("config_test_s");
		foreach (string key in keys) {
			Debug.Log ("    " + key);
		}
	}

	public void DisplayData ()
	{
		Debug.Log (FirebaseRemoteConfig.GetValue (StringValuables.minLevelShowRate).LongValue);
	}

	public void FetchData ()
	{
		Debug.Log ("Fetching data...");
		// FetchAsync only fetches new data if the current data is older than the provided
		// timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
		// By default the timespan is 12 hours, and for production apps, this is a good
		// number.  For this example though, it's set to a timespan of zero, so that
		// changes in the console will always show up immediately.
		//FirebaseRemoteConfig.ActivateFetched();
		System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync ();
		fetchTask.ContinueWith (FetchComplete);
	}

	void FetchComplete (Task fetchTask)
	{
		initCompleted = true;
		if (fetchTask.IsCanceled) {
			Debug.Log ("Fetch canceled.");
		} else if (fetchTask.IsFaulted) {
			Debug.Log ("Fetch encountered an error.");
		} else if (fetchTask.IsCompleted) {
			Debug.Log ("Fetch completed successfully!");
			//FirebaseRemoteConfig.ActivateFetched();
		}

		var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
		switch (info.LastFetchStatus) {
		case Firebase.RemoteConfig.LastFetchStatus.Success:
			Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched ();
                //isplayData();
		    
			Debug.Log (String.Format ("Remote data loaded and ready (last fetch time {0}).",
				info.FetchTime));
			string blockRate = FirebaseRemoteConfig.GetValue (StringValuables.item_rate).StringValue;
			if (blockRate != "") {
				blockRate = blockRate.Replace ("\\", "");
				LoadingControl.InitBlockData (blockRate);
			}

			SaveToPrefs ();
			DisplayAllKeys ();
			break;
		case Firebase.RemoteConfig.LastFetchStatus.Failure:
			LoadFromPrefs ();
			switch (info.LastFetchFailureReason) {
			case Firebase.RemoteConfig.FetchFailureReason.Error:
				Debug.Log ("Fetch failed for unknown reason");
				break;
			case Firebase.RemoteConfig.FetchFailureReason.Throttled:
				Debug.Log ("Fetch throttled until " + info.ThrottledEndTime);
				break;
			}
			break;
		case Firebase.RemoteConfig.LastFetchStatus.Pending:
			Debug.Log ("Latest Fetch call still pending.");
			break;
		}
	}


	public int  score_level_2_value {
		get {
			return 900;
		}
	}

	public int  score_level_1_value {
		get {
			return 500;
		}
	}

	public int  score_level_3_value {
		get {
			return 1500;
		}
	}

	public string randomLogic {
		get {
			
			try {
				if (RemoteDict != null && RemoteDict.ContainsKey ("randomLogic")) {
					return RemoteDict ["randomLogic"].ToString ();
				} else {
					return StringValuables.sonat;
				}
			} catch (Exception e) {
				Debug.Log ("Loi GetRandomLogic : " + e.ToString ());
				//Debug.Log ("Loi get randomLogic  : " + RemoteDict ["randomLogic"].ToString () + " : " + e);
				return StringValuables.sonat;
			}

			//return StringValuables.sonat;
		}
	}

	

	public int GetBackNumber ()
	{
		if (initCompleted == true) {
			//return (int)FirebaseRemoteConfig.GetValue (StringValuables.backNumber).LongValue;
			try {
				return int.Parse (RemoteDict [StringValuables.backNumber].ToString ());
			} catch (Exception e) {
				Debug.Log ("Loi getbacknumber  : " + e);
				return 1;
			}
		}
		return 1;
	}

	public int GetDisplayAdLevel ()
	{


		try {
			return int.Parse (RemoteDict [StringValuables.display_ad_level].ToString ());
		} catch (Exception e) {
			Debug.Log ("Loi : " + e.ToString ());
			return 1;
		}
	}

	public int GetReplayNumber ()
	{
		//return (int)FirebaseRemoteConfig.GetValue (StringValuables.replayNumber).LongValue;
		try {
			return int.Parse (RemoteDict [StringValuables.replayNumber].ToString ());
		} catch (Exception e) {
			Debug.Log ("Loi GetReplayNumbe " + e.ToString ());
			return 1;
		}
	}

	public int GetMinLevelShowAds ()
	{
		//return (int)FirebaseRemoteConfig.GetValue (StringValuables.minLevelShowAds).LongValue;
		try {
			return int.Parse (RemoteDict [StringValuables.minLevelShowAds].ToString ());
		} catch (Exception e) {
			Debug.Log ("Loi GetMinLevelShowAds : " + e.ToString ());
			return 1;
		}
	}

	public int GetMinLevelShowRate ()
	{
		//return (int)FirebaseRemoteConfig.GetValue (StringValuables.minLevelShowRate).LongValue;
		try {
			return int.Parse (RemoteDict [StringValuables.minLevelShowRate].ToString ());
		} catch (Exception e) {
			Debug.Log ("Loi get minlevelshowrate " + e.ToString ());
			return 1;
		}
	}

	public string GetVideoRewardType ()
	{
		//return FirebaseRemoteConfig.GetValue (StringValuables.video_network).StringValue;
		try {
			return RemoteDict [StringValuables.video_network].ToString ();
		} catch (Exception e) {
			Debug.Log ("Loi GetVideoRewardType : " + e.ToString ());
			return StringValuables.admob;
		}
	}

	public string GetAdNetwork ()
	{
		//return FirebaseRemoteConfig.GetValue (StringValuables.AdNework).StringValue;
		try {
			if (RemoteDict != null && RemoteDict.ContainsKey (StringValuables.AdNework)) {
				return RemoteDict [StringValuables.AdNework].ToString ();
			} else
				return StringValuables.admob;
		} catch (Exception e) {
			Debug.Log ("Loi get Adnetwork : " + e.ToString ());
			return StringValuables.admob;
		}
	}

	

	#region CrossPromotion

	//[SerializeField]
	//private CrossPromotionItem bigAd;
	//[SerializeField]
	//private CrossPromotionItem[] smallAd;
	//private static string crossPromotionURL = "http://222.255.46.60/product/index.php/Global_advertise/get_android_ad";
	//private string key_userOpened = "__OPENED_ONCE__";
	//private bool returnUser;
	//private static bool isCrossPromotionLoaded = false;
	//public static bool IsCrossPromotionLoaded { get { return isCrossPromotionLoaded; } }
	//    public int android_id;
	//    public int iOS_id;

	//    public int id
	//    {
	//        get
	//        {
	//#if UNITY_ANDROID
	//            return android_id;
	//#elif UNITY_IPHONE
	//            return iOS_id;
	//#endif
	//        }
	//    }

	//    [SerializeField]
	//    private GameObject adObj;

	//    [SerializeField]
	//    private GameObject moreGameButton;

	//    [SerializeField]
	//    private GameObject crossObj;
	//    [SerializeField]
	//    private GameObject loadingObj;
	//    [SerializeField]
	//    private float waitingTime;

	//    public void ShowPopup()
	//    {
	//        if (!isCrossPromotionLoaded)
	//        {
	//            return;
	//        }
	//        if (!adObj.activeInHierarchy)
	//        {
	//            adObj.SetActive(true);
	//        }
	//    }

	//public void ShowOnUI(bool show)
	//{
	//    crossObj.SetActive(show);
	//}

	//public void Close()
	//{
	//    adObj.SetActive(false);
	//}
	//    public void DisableMoreGameButton()
	//    {
	//        moreGameButton.SetActive(false);
	//    }
	//    public void ActiveMoreGameButton()
	//    {
	//        moreGameButton.SetActive(true);
	//    }
	//    public void HidePopup()
	//    {
	//        if (!isCrossPromotionLoaded)
	//        {
	//            return;
	//        }
	//        if (adObj.activeInHierarchy)
	//        {
	//            adObj.SetActive(false);
	//        }
	//    }
	//public override void Init()
	//{
	//if (!returnUser)
	//{
	//    PlayerPrefs.SetInt(key_userOpened, 1);
	//    PlayerPrefs.Save();
	//}
	//bigAd.Init();
	//foreach(CrossPromotionItem cpi in smallAd)
	//{
	//    cpi.Init();
	//}

	//StartCoroutine(LoadCrossPromotion());
	//StartCoroutine(Loading());
	//if (moreGameButton != null)
	//{
	//    moreGameButton.SetActive(false);
	//}
	//if(adObj != null)
	//{
	//    adObj.SetActive(false);
	//}
	//}
	//IEnumerator Loading()
	//{
	//    loadingObj.SetActive(true);
	//    yield return new WaitForSeconds(waitingTime);
	//    loadingObj.SetActive(false);
	//}
	//private int LoadCount = 0;
	//IEnumerator LoadCrossPromotion()
	//{
	//    LoadCount++;
	//    bool reload = false;
	//    isCrossPromotionLoaded = false;
	//    WWWForm form = new WWWForm();
	//    form.AddField("appId", id);
	//    using (WWW w = new WWW(crossPromotionURL, form.data))
	//    {
	//        yield return w;
	//        if (string.IsNullOrEmpty(w.error))
	//        {
	//            int count = 0;
	//            CrossPromoJSON json = JsonUtility.FromJson<CrossPromoJSON>(w.text);
	//            //Debug.Log(w.text);
	//            if (json.smart_more_app.big_ad.isLoaded())
	//            {
	//                count++;
	//            }

	//            foreach (CrossPromoJSON.Product p in json.smart_more_app.small_ad)
	//            {
	//                if (p.isLoaded())
	//                {
	//                    count++;
	//                }
	//            }

	//            if (count >= 3)
	//            {
	//                count = 0;
	//                bigAd.LoadItem(json.smart_more_app.big_ad);
	//                foreach (CrossPromotionItem cri in smallAd)
	//                {
	//                    cri.LoadItem(json.smart_more_app.small_ad[count++]);
	//                }
	//                if (moreGameButton != null)
	//                {
	//                    moreGameButton.SetActive(true);
	//                }
	//                isCrossPromotionLoaded = true;
	//                if (returnUser)
	//                {
	//                    //ShowPopup();
	//                }
	//            }
	//            //GlobalConfiguration.Instance.SetMinLevelShowRate(json.number_level_passed_to_rate.rate_condition);
	//            config.SetMinLevelShowRate(json.number_level_passed_to_rate.rate_condition);
	//            reload = false;
	//        }
	//        else
	//        {
	//            reload = true;
	//        }
	//    }
	//    if (reload && LoadCount < 5)
	//    {
	//        yield return LoadCrossPromotion();
	//    }
	//}

	//IEnumerator LoadLinkApp()
	//{
	//    bool success = false;
	//    WWWForm form = new WWWForm();
	//    form.AddField("appId", id);
	//    using (WWW w = new WWW(linkAppURL, form.data))
	//    {
	//        yield return w;
	//        if (string.IsNullOrEmpty(w.error))
	//        {
	//            Debug.Log(w.text);
	//            LinkAppJSON json = JsonUtility.FromJson<LinkAppJSON>(w.text);
	//            success = true;
	//            //if (moreGameButton != null)
	//            //{
	//            //    moreGameButton.SetActive(true);
	//            //}
	//            UISystem.GetPopup<PopupFreeCoins>().LoadJSON(json);
	//        }
	//        else
	//        {
	//            Debug.Log(w.error);
	//            success = false;
	//        }
	//    }
	//    isLinkAppLoaded = success;
	//    if (!isLinkAppLoaded)
	//    {
	//        yield return LoadLinkApp();
	//    }
	//}
	//[System.Serializable]
	//public class CrossPromoJSON
	//{
	//    public ProductGroup smart_more_app;
	//    public NumberLevelPassRate number_level_passed_to_rate;

	//    [System.Serializable]
	//    public class Product
	//    {
	//        public string url;
	//        public string image;
	//        public bool isLoaded()
	//        {
	//            return !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(image);
	//        }
	//    }

	//    [System.Serializable]
	//    public class ProductGroup
	//    {
	//        public Product big_ad;
	//        public Product[] small_ad;
	//    }

	//    [System.Serializable]
	//    public class NumberLevelPassRate
	//    {
	//        public int rate_condition;
	//    }
	//}

	#endregion
    

}


