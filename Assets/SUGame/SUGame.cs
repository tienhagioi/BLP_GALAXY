using UnityEngine;
using System.Collections;

public class SUGame : MonoBehaviour
{
	private static SUGame instance;

	public static SUGame Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<SUGame> ();
				instance.Init ();
			}
			return instance;
		}
	}
	#pragma warning disable 0649
	[SerializeField]
	private BaseSUUnit[] units;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			//instance.Init ();
			CheckDependencies ();
			DontDestroyOnLoad (this.gameObject);
		} else {
			CheckDependencies ();
			DontDestroyOnLoad (this.gameObject);
			//Destroy (gameObject);
		}
#if UNITY_ANDROID

		//Application.targetFrameRate = 60;

		QualitySettings.vSyncCount = 0;

		QualitySettings.antiAliasing = 0;
		int qualityLevel = QualitySettings.GetQualityLevel ();

		if (qualityLevel == 0) {
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 15;
		} else if (qualityLevel == 5) {
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 70;
		}

		// Screen.sleepTimeout = SleepTimeout.NeverSleep;

#elif UNITY_IOS
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

	}

	private void Init ()
	{
		//foreach(BaseSUUnit unit in units)
		//{
		//    unit.Init();
		//}

		for (int i = 0; i < units.Length; i++) {
			units [i].Init ();
		}
	}

	private T _Get<T> () where T : BaseSUUnit
	{
		foreach (BaseSUUnit unit in units) {
			if (typeof(T).Equals (unit.GetType ())) {
				return unit as T;
			}
		}

		return default(T);
	}

	public static T Get<T> () where T : BaseSUUnit
	{
		return Instance._Get<T> ();
	}

	public static bool haveDependency = false;

	void CheckDependencies ()
	{
		Firebase.DependencyStatus dependencyStatus;
		dependencyStatus = Firebase.FirebaseApp.CheckDependencies ();
		if (dependencyStatus != Firebase.DependencyStatus.Available) {
			Firebase.FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
				dependencyStatus = Firebase.FirebaseApp.CheckDependencies ();
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					haveDependency = true;
				} else {
					haveDependency = false;
					Debug.LogError (
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
				Init ();
			});
		} else {
			haveDependency = true;
			Init ();
		}
	}
}
