using UnityEngine;
using System.Collections;

public class SUAppInfo : BaseSUUnit
{

	private static bool isAppInfoLoaded = false;

	public static bool IsAppInfoLoaded { get { return isAppInfoLoaded; } }

	//private static string linkAppURL = "http://222.255.46.60/product/index.php/Global_advertise/get_app_info";
	#pragma warning disable 0649
	[SerializeField]

	private Configuration config;

	public Configuration Config {
		get {
			return config;
		}
	}

	public int android_id;
	public int iOS_id;

	public int id {
		get {
#if UNITY_ANDROID
			return android_id;
#elif UNITY_IPHONE
            return iOS_id;
#else
			return 0;
#endif
		}
	}

	public override void Init ()
	{
		//StartCoroutine(LoadAppInfo());
	}

	//private int LoadCount = 0;
	//IEnumerator LoadAppInfo()
	//{
	//    LoadCount++;
	//    bool success = false;
	//    WWWForm form = new WWWForm();
	//    form.AddField("appId", id);
	//    using (WWW w = new WWW(linkAppURL, form.data))
	//    {
	//        yield return w;
	//        if (string.IsNullOrEmpty(w.error))
	//        {
	//            //Debug.Log(w.text);
	//            AppInfoJSON json = JsonUtility.FromJson<AppInfoJSON>(w.text);
	//            success = true;
	//            //if (moreGameButton != null)
	//            //{
	//            //    moreGameButton.SetActive(true);
	//            //}
	//            //UISystem.GetPopup<PopupFreeCoins>().LoadJSON(json);
	//            config.SetVideoCoin(json.videoCoin.coins);
	//            config.SetRatingCoin(json.ratingCoin.coins);
	//            config.SetInstallApps(json.installApps);
	//        }
	//        else
	//        {
	//            Debug.Log(w.error);
	//            success = false;
	//        }
	//    }
	//    isAppInfoLoaded = success;
	//    if (!isAppInfoLoaded && LoadCount < 5)
	//    {
	//        yield return LoadAppInfo();
	//    }
	//}

	[System.Serializable]
	public class Configuration
	{
		public int videoCoin;

		public void SetVideoCoin (int coin)
		{
			videoCoin = coin;
		}

		public int ratingCoin;

		public void SetRatingCoin (int coin)
		{
			ratingCoin = coin;
		}

		public AppInfoJSON.InstallApp[] installApps;

		public void SetInstallApps (AppInfoJSON.InstallApp[] apps)
		{
			installApps = apps;
			foreach (AppInfoJSON.InstallApp app in apps) {
				ImageCaching.LoadImage (app.icon, app.SetImageLoaded);
			}
		}
	}
}

[System.Serializable]
public class AppInfoJSON
{
	public VideoCoin videoCoin;
	public RatingCoin ratingCoin;
	public InstallApp[] installApps;

	[System.Serializable]
	public class VideoCoin
	{
		public int coins;
	}

	[System.Serializable]
	public class RatingCoin
	{
		public int coins;
	}

	[System.Serializable]
	public class InstallApp
	{
		public string name;
		public string description;
		public int coin;
		public string url;
		public string icon;
		public string package;
		[System.NonSerialized]
		public Sprite icon_spr;

		public void SetImageLoaded (Sprite spr)
		{
			icon_spr = spr;
			loaded = true;
		}

		[System.NonSerialized]
		public bool loaded;
	}
}