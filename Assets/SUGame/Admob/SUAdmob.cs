using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

//using AudienceNetwork;

public class SUAdmob : BaseSUUnit
{

	[SerializeField]
	private string AndroidBannerId = "";
	[SerializeField]
	private string AndroidIadsId = "";
	[SerializeField]
	private string IOSBannerId;
	[SerializeField]
	private string IOSIadsId;
	private BannerView GA_Banner;
	private InterstitialAd GA_Iad;
	public bool isShowingFullAds;
	[SerializeField] private float GA_IAd_Reload = 60;
	private float timer = 0;
	bool iad_need_reload = true;
	bool initialized = false;
	public  string UnityAndroidGameId, UnityIOSGameId;
	float timeCantShowUnityAds = 0;

	void Update ()
	{
		if (timeCantShowUnityAds > 0 && initialized == true) {
			timeCantShowUnityAds -= Time.deltaTime;
		}
		if (iad_need_reload && initialized == true) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				ReloadIads ();
			}
		}
	}

	public override void Init ()
	{                


#if UNITY_IPHONE
		Advertisement.Initialize(UnityIOSGameId);
		GA_Banner = new BannerView (IOSBannerId, GoogleMobileAds.Api.AdSize.SmartBanner, AdPosition.Bottom);
#else
		GA_Banner = new BannerView (AndroidBannerId, GoogleMobileAds.Api.AdSize.SmartBanner, AdPosition.BottomRight);
		Advertisement.Initialize (UnityAndroidGameId);
#endif
		AdRequest request = new AdRequest.Builder ().Build ();
		GA_Banner.OnAdLoaded += GA_Banner_OnAdLoaded;
		GA_Banner.OnAdFailedToLoad += GA_Banner_OnAdFailedToLoad;
		GA_Banner.OnAdClosed += GA_Banner_OnAdClosed;
		GA_Banner.OnAdOpening += GA_Banner_OnAdOpening;
		GA_Banner.LoadAd (request);
		GA_Banner.Hide ();
#if UNITY_IPHONE
		GA_Iad = new GoogleMobileAds.Api.InterstitialAd (IOSIadsId);   
#else
		GA_Iad = new GoogleMobileAds.Api.InterstitialAd (AndroidIadsId);
#endif
		AdRequest iadRequest = new AdRequest.Builder ().Build ();
		GA_Iad.OnAdLoaded += GA_Iad_OnAdLoaded;
		GA_Iad.OnAdFailedToLoad += GA_Iad_OnAdFailedToLoad;
		GA_Iad.OnAdClosed += GA_Iad_OnAdClosed;
		GA_Iad.OnAdOpening += GA_Iad_OnAdOpening;
		GA_Iad.LoadAd (iadRequest);
		timer = GA_IAd_Reload;
		initialized = true;
	}

	void GA_Iad_OnAdOpening (object sender, System.EventArgs e)
	{
		isShowingFullAds = true;
		iad_need_reload = true;
		Time.timeScale = 0;
	}

	void GA_Iad_OnAdClosed (object sender, System.EventArgs e)
	{
		isShowingFullAds = false;
		iad_need_reload = true;
		timeCantShowUnityAds = 2F;
		Time.timeScale = 1;
		ReloadIads ();
	}

	void GA_Iad_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
	{
		iad_need_reload = true;
	}

	void GA_Iad_OnAdLoaded (object sender, System.EventArgs e)
	{

	}
	//
	void GA_Banner_OnAdOpening (object sender, System.EventArgs e)
	{

	}

	void GA_Banner_OnAdClosed (object sender, System.EventArgs e)
	{

	}

	void GA_Banner_OnAdFailedToLoad (object sender, AdFailedToLoadEventArgs e)
	{

	}

	void GA_Banner_OnAdLoaded (object sender, System.EventArgs e)
	{

	}

	public void ShowIads ()
	{
		if (initialized == false) {
			return;
		}
		string adNetwork = SUGame.Get<SURemoteConfig> ().GetAdNetwork ();
		if (adNetwork == StringValuables.admob) {
			if (GA_Iad.IsLoaded ()) {
				GA_Iad.Show ();
			} else {
				Debug.Log ("Khong show dc ad admob do chua load dc");
				ReloadIads ();
			}
		} else if (adNetwork == StringValuables.facebook) {
			
			if (GA_Iad.IsLoaded ()) {
				GA_Iad.Show ();
			} else {
				Debug.Log ("Khong show dc ad trong facebook do chua load dc");
				ReloadIads ();
			}
				

		} else if (adNetwork == StringValuables.unity) {

			if (timeCantShowUnityAds <= 0) {
				// neu co the show unity ads 
				if (Advertisement.IsReady ()) {
					ShowUnityRewardedVideo ();
					timeCantShowUnityAds = 31;
				} else {
					Debug.Log ("Khong show dc UnityAds do chua load dc,show admob thay the ");
					if (GA_Iad.IsLoaded ()) {
						GA_Iad.Show ();
					} else {
						Debug.Log ("Admob thay the chua load dc , khong show ad ");
						ReloadIads ();
					}
				}
			} else {
				Debug.Log ("Khong show dc ad do timeCantShowUnityAds > 0");
				/*
				if (GA_Iad.IsLoaded ()) {
					GA_Iad.Show ();
				} else {
					ReloadIads ();
				}
				*/
			}

		} else {
			if (GA_Iad.IsLoaded ()) {
				Debug.Log ("Khong show dc ad admob do chua load dc ");
				GA_Iad.Show ();
			} else {
				ReloadIads ();
			}
		}

	}

	public void ShowBanner ()
	{
		if (initialized == true) {
			#if UNITY_5_6_1
		GA_Banner.Show ();
			#else
			string adNetwork = SUGame.Get<SURemoteConfig> ().GetAdNetwork ();
			if (adNetwork == StringValuables.admob) {
				GA_Banner.Show ();
			}
			#endif
		} else {
			Invoke ("ShowBanner", 1F);
		}
	}

	public void HideBanner ()
	{
		#if UNITY_5_6_1
		GA_Banner.Hide ();
		#else
		string adNetwork = SUGame.Get<SURemoteConfig> ().GetAdNetwork ();
		if (adNetwork == StringValuables.admob) {
			GA_Banner.Hide ();
		}
		#endif
	}

	private void ReloadIads ()
	{
		if (initialized == true) {
			GA_Iad.OnAdLoaded -= GA_Iad_OnAdLoaded;
			GA_Iad.OnAdFailedToLoad -= GA_Iad_OnAdFailedToLoad;
			GA_Iad.OnAdClosed -= GA_Iad_OnAdClosed;
			GA_Iad.OnAdOpening -= GA_Iad_OnAdOpening;
			GA_Iad.Destroy ();
			#if UNITY_ANDROID
			GA_Iad = new InterstitialAd (AndroidIadsId);
			#elif UNITY_IPHONE 
		GA_Iad = new GoogleMobileAds.Api.InterstitialAd (IOSIadsId);  
			#endif
			GA_Iad.LoadAd (new AdRequest.Builder ().Build ());
			GA_Iad.OnAdLoaded += GA_Iad_OnAdLoaded;
			GA_Iad.OnAdFailedToLoad += GA_Iad_OnAdFailedToLoad;
			GA_Iad.OnAdClosed += GA_Iad_OnAdClosed;
			GA_Iad.OnAdOpening += GA_Iad_OnAdOpening;
			timer = GA_IAd_Reload;
			iad_need_reload = false;
		}
	}

	void ShowUnityRewardedVideo ()
	{
		ShowOptions options = new ShowOptions ();
		options.resultCallback = HandleShowUnityVideoResult;
		if (Advertisement.IsReady ()) {
			Advertisement.Show (options);
			isShowingFullAds = true;
		}
	}

	void HandleShowUnityVideoResult (ShowResult result)
	{
		if (result == ShowResult.Finished) {
			isShowingFullAds = false;
			timeCantShowUnityAds = 1F;
			Debug.Log ("Video completed - Offer a reward to the player");

		} else if (result == ShowResult.Skipped) {
			isShowingFullAds = false;
			timeCantShowUnityAds = 1F;
			Debug.LogWarning ("Video was skipped - Do NOT reward the player");

		} else if (result == ShowResult.Failed) {
			isShowingFullAds = false;
			timeCantShowUnityAds = 1F;
			Debug.LogError ("Video failed to show");
		}

	}
}
