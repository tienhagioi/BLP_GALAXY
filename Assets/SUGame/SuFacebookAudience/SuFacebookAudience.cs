using UnityEngine;
using System.Collections;
using AudienceNetwork;

public class SuFacebookAudience : BaseSUUnit
{
	#pragma warning disable
	public string AndroidBannerId = "514392348930219_531083070594480";
	public string AndroidIadsId = "514392348930219_531082720594515";
	public string IosBannerId = "514392348930219_531083070594480";
	public string IosIadsId = "514392348930219_531082720594515";
	AdView fbAdView;
	InterstitialAd interstitialAd;
	[HideInInspector]
	public bool isIadsLoaded, isBannerLoaded;

	[SerializeField] private float GA_IAd_Reload = 60;
	private float timer = 0;
	bool iad_need_reload = true;
	public bool isTest = false;


	void Update ()
	{
		if (iad_need_reload) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				#if !UNITY_EDITOR
				LoadInterstitial ();
				#endif
			}
		}
	}

	public override void Init ()
	{
		
		
		#if !UNITY_EDITOR
		if (fbAdView != null) {
			fbAdView.Dispose ();	
		}
		#if UNITY_ANDROID

		fbAdView = new AdView (AndroidBannerId, AdSize.BANNER_HEIGHT_50);
		#elif UNITY_IPHONE
		fbAdView = new AdView (IosBannerId, AdSize.BANNER_HEIGHT_50);
		#endif

		fbAdView.Register (gameObject);
		// Set delegates to get notified on changes or when the user interacts with the ad.
		fbAdView.AdViewDidLoad = (delegate() {
			Debug.Log ("Ad view loaded.");
			isBannerLoaded = true;
			//ShowBanner ();

		});
		fbAdView.AdViewDidFailWithError = (delegate(string error) {
			//Init ();
			Debug.Log ("Ad view failed to load with error: " + error);
		});
		fbAdView.AdViewWillLogImpression = (delegate() {
			Debug.Log ("Ad view logged impression.");
		});
		fbAdView.AdViewDidClick = (delegate() {
			Debug.Log ("Ad view clicked.");
		});
		fbAdView.LoadAd ();
		LoadInterstitial ();
		#endif

	}

	public void ShowBanner ()
	{
		#if !UNITY_EDITOR
		if (isBannerLoaded == true) {
			//fbAdView.Show (0);
			double height = AudienceNetwork.Utility.AdUtility.convert (Screen.height);
			fbAdView.Show (height - 50);
		} else {
			Init ();
		}
		#endif
	}

	public void LoadInterstitial ()
	{
		#if !UNITY_EDITOR
		
		if (interstitialAd != null) {
			isIadsLoaded = false;
			interstitialAd.Dispose ();
		}
		// Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
		// Use different ID for each ad placement in your app.
		#if UNITY_ANDROID

		interstitialAd = new InterstitialAd (AndroidIadsId);
		#elif UNITY_IPHONE
		interstitialAd = new InterstitialAd (IosIadsId);
		#endif
		interstitialAd.Register (this.gameObject);

		// Set delegates to get notified on changes or when the user interacts with the ad.
		interstitialAd.InterstitialAdDidLoad = (delegate() {	
			isIadsLoaded = true;
			iad_need_reload = false;
			Debug.Log ("Interstitial ad loaded.");
		});
		interstitialAd.InterstitialAdWillClose = (delegate() {
			iad_need_reload = true;

		});
		interstitialAd.InterstitialAdDidFailWithError = (delegate(string error) {
			Debug.Log ("Interstitial ad failed to load with error: " + error);
			iad_need_reload = true;
		});
		interstitialAd.InterstitialAdWillLogImpression = (delegate() {
			Debug.Log ("Interstitial ad logged impression.");
		});
		interstitialAd.InterstitialAdDidClick = (delegate() {
			Debug.Log ("Interstitial ad clicked.");
		});
		interstitialAd.InterstitialAdDidClose = (delegate() {
			//iad_need_reload = true;
			LoadInterstitial ();
			SUGame.Get<SUAdmob> ().isShowingFullAds = false;
		});
		// Initiate the request to load the ad.
		interstitialAd.LoadAd ();
		timer = GA_IAd_Reload;
		iad_need_reload = false;
		#endif
	}

	public void HideBanner ()
	{
		fbAdView.Dispose ();
	}

	public void ShowIads ()
	{
		#if !UNITY_EDITOR
		if (isIadsLoaded == true) {
			interstitialAd.Show ();

		} else {
			LoadInterstitial ();
		}
		#endif
	}
}

