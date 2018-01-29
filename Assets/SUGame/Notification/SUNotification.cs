using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using Firebase;

//using UnityEngine.iOS;
using System.Collections.Generic;
using System.Text;

#region NotificationItem
//[SerializeField]
//private int repeatitionID;
//[SerializeField]
//private NotificationItem[] notifyItems;
//INotification notification;
//public enum NotificationType { Once, Repeat }
//public enum NotificationTimeType { DelayTime, DelayDate, DelayDateInWeek, SpecificDate }
//[System.Serializable]
//public class NotificationItem
//{
//    public int id;
//    public string title;
//    public string content;
//    public string icon;
//    public bool hasSound;

//    //purpose specific
//    public NotificationType notificationType;
//    public NotificationTimeType notificationTimeType;

//    public int delayTime;

//    public int delayDate;
//    public DayOfWeek dateInWeek;

//    public int exactHour;
//    public int exactMinute;
//    public int exactSecond;

//    public string exactDate;

//    public int repeatTimes;
//}
#endregion

public class SUNotification : BaseSUUnit
{
	

	public List<NotificationStruct> listNotifications;


	[SerializeField]
	private bool _enableNotify = true;


	public bool enableNotify {
		get {
			return _enableNotify;
		}
	}

	//Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

	public override void Init ()
	{		

		//SendMessageToDevice ("day la tittle", "day la body", "dX8soOE3-XM:APA91bFZGY_wnDSPR5i-HWl_Zo4iwL7qk8TUc5gerIlIurIP6TJE3e6Aa9CbKzqLUDHaKrDcsWgNBz1Nh3wSUdDLvPjgsFKD-vbQ0-cI_RRLC4F3mxC1lwVlwDhsXWZ_TEwP-KABOSOV");

		//StartCoroutine (InitTokenCoroutine ("dadad"));
		Debug.Log ("try to create notification crossplatform");
#if !UNITY_EDITOR
#if UNITY_ANDROID
        			//notification = new CNotificationAndroid ();
#elif UNITY_IOS
                RegisterforNotifyIos();
#endif
#endif
		Debug.Log ("Created notification crossplatform");
		//if (notification != null)
		//{
		//    notification.Init();
		//}

		if (enableNotify) {
			Debug.Log ("enable notification");
			CancelAllNotification ();
			for (int i = 0; i < listNotifications.Count; i++) {
				if (listNotifications [i].isEnable == true) {
					listNotifications [i].Init ();
				}
			}
			//SetupNewNotification();
		}


		if (SUGame.haveDependency == true) {
			InitializeFirebase ();
		}



	}

	#region checkPlayServices

	//public bool IsPlayServicesAvailable()
	//{
	//    const string GoogleApiAvailability_Classname =
	//        "com.google.android.gms.common.GoogleApiAvailability";
	//    AndroidJavaClass clazz =
	//        new AndroidJavaClass(GoogleApiAvailability_Classname);
	//    AndroidJavaObject obj =
	//        clazz.CallStatic<AndroidJavaObject>("getInstance");

	//    var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	//    var activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");

	//    int value = obj.Call<int>("isGooglePlayServicesAvailable", activity);

	//    Debug.Log(value);

	//    // result codes from https://developers.google.com/android/reference/com/google/android/gms/common/ConnectionResult

	//    // 0 == success
	//    // 1 == service_missi
	//    // 2 == update service required
	//    // 3 == service disabled
	//    // 18 == service updating
	//    // 9 == service invalid
	//    return value == 0;
	//}
	//public bool isReachCondition = false;
	//IEnumerator InitFireBaseRoutine()
	//{
	//    while(IsPlayServicesAvailable())
	//    {
	//        FirebaseApp.FixDependenciesAsync();
	//        yield return SUtils.Yielder.GetTimeYielder(0.1f);
	//    }
	//    isReachCondition = true;
	//    //FirebaseApp.
	//}

	#endregion

	private void InitializeFirebase ()
	{
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

	}

	public void OnTokenReceived (object sender, Firebase.Messaging.TokenReceivedEventArgs token)
	{
		
		UnityEngine.Debug.Log ("Received notification Registration Token: " + token.Token);

	}

	public void OnMessageReceived (object sender, Firebase.Messaging.MessageReceivedEventArgs e)
	{
		
		UnityEngine.Debug.Log ("Received a new message from: " + e.Message.From);
	}

	#if UNITY_IOS
    void RegisterforNotifyIos()
    {
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Badge);
    }
    void CancelAllNotifyIOS()
    {
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();

        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
    }
    void ScheduleNotificationIOS(string title, string content, DateTime dateTime,
            bool hasAction = false, bool hasSound = false, bool hasVibrate = false)
    {
        UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();

        notif.alertAction = title;
        notif.alertBody = content;
        notif.fireDate = dateTime;
        notif.hasAction = hasAction;
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
    }
#endif

	#region ScheduleLocalNotifyMutipleTimes

	//public void SendNotification(int id, string title, string content, string icon, long delay, bool hasSound = false)
	//    {
	//        if (!enableNotify)
	//        {
	//            return;
	//        }
	//        if (delay <= 0)
	//        {
	//            Debug.Log("can't start a notification with time <=0");
	//            return;
	//        }
	//        if (notification != null)
	//        {
	//            notification.SendNotification(id, title, content, icon, delay, hasSound);
	//            Debug.Log("Send Notification content : " + content);
	//        }
	//        else
	//        {
	//            Debug.Log("Send Notification content : " + content);
	//        }
	//    }

	//    public void CancelNotification(int id)
	//    {
	//        if (notification != null)
	//        {
	//            notification.CancelNotification(id);
	//        }
	//        else
	//        {
	//            Debug.Log("Cancel notification");
	//        }
	//    }

	//    public void SetupNewNotification()
	//    {

	////#if UNITY_ANDROID
	////        if (notification == null)
	////        {
	////            return;
	////        }
	////#endif
	//        foreach (NotificationItem item in notifyItems)
	//        {
	//            SetupNotify(item);
	//        }
	//    }

	//public void SetupNotify(NotificationItem item)
	//{
	//    switch (item.notificationType)
	//    {
	//        case NotificationType.Once:
	//            SetupNotificationPlayOnce(item);
	//            break;
	//        case NotificationType.Repeat:
	//            SetupNotificationPlayMultipleTime(item);
	//            break;
	//    }

	//}

	//    private void SetupNotificationPlayOnce(NotificationItem item)
	//    {
	//        Debug.Log("setupNotify");
	//        long time = 0;
	//        TimeSpan span;
	//        DateTime today = DateTime.Today;
	//        switch (item.notificationTimeType)
	//        {
	//            case NotificationTimeType.DelayDate:
	//                //DateTime currentDate = DateTime.Today;
	//                //span = new TimeSpan(item.delayDate, item.exactHour, item.exactMinute, item.exactSecond);				
	//                today = today.AddDays(item.delayDate).AddHours(item.exactHour).AddMinutes(item.exactMinute).AddSeconds(item.exactSecond);
	//                time = (long)today.Subtract(DateTime.Now).TotalMilliseconds;
	//                Debug.Log(time);
	//                break;
	//            case NotificationTimeType.DelayTime:
	//                time = item.delayTime * 1000L;
	//                break;
	//            case NotificationTimeType.SpecificDate:
	//                DateTime targetDate = DateTime.ParseExact(item.exactDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
	//                span = new TimeSpan(0, item.exactHour, item.exactMinute, item.exactSecond);
	//                targetDate = targetDate.Add(span);
	//                span = targetDate.Subtract(DateTime.Now);
	//                time = (long)span.TotalMilliseconds;
	//                break;
	//            case NotificationTimeType.DelayDateInWeek:
	//                DateTime currentDate = DateTime.Today;
	//                int diff = item.dateInWeek - currentDate.DayOfWeek;
	//                if (diff < 0)
	//                {
	//                    diff += 7;
	//                }
	//                today = today.AddDays(diff).AddHours(item.exactHour).AddMinutes(item.exactMinute).AddSeconds(item.exactSecond);
	//                span = today.Subtract(DateTime.Now);
	//                time = (long)span.TotalMilliseconds;
	//                break;
	//        }
	//#if UNITY_ANDROID
	//        //SendNotification(item.id, item.title, item.content, item.icon, time, item.hasSound);
	//        LocalNotification.SendNotification(1,time, item.title, item.content, Color.red, true, true, true, "app_icon");
	//#elif UNITY_IOS
	//        DateTime dateTime = DateTime.Now.AddMilliseconds(time);
	//        ScheduleNotificationIOS(item.title, item.content, dateTime, true, item.hasSound);
	//#endif
	//    }


	//    private void SetupNotificationPlayMultipleTime(NotificationItem item)
	//    {
	//        long time = 0;
	//        TimeSpan span;
	//        DateTime today = DateTime.Today;
	//        switch (item.notificationTimeType)
	//        {
	//            case NotificationTimeType.DelayDate:
	//                for (int i = 0; i < item.repeatTimes; i++)
	//                {
	//                    today = DateTime.Today;
	//                    today = today.AddDays(item.delayDate * (i + 1)).AddHours(item.exactHour).AddMinutes(item.exactMinute).AddSeconds(item.exactSecond);
	//                    time = (long)today.Subtract(DateTime.Now).TotalMilliseconds;
	//#if UNITY_ANDROID
	//                    //var notificationParams = new NotificationParams
	//                    //{
	//                    //    Id = UnityEngine.Random.Range(0, int.MaxValue),
	//                    //    Delay = TimeSpan.FromSeconds(time / 1000L),
	//                    //    Title = "Full Lives!!!",
	//                    //    Message = "Continue the journey!",
	//                    //    Ticker = "Ticker",
	//                    //    Sound = true,
	//                    //    Vibrate = true,
	//                    //    Light = true,
	//                    //    SmallIcon = NotificationIcon.Heart,
	//                    //    SmallIconColor = Color.red,
	//                    //    LargeIcon = "app_icon"
	//                    //};
	//                    //NotificationManager.SendCustom(notificationParams);
	//#elif UNITY_IOS
	//                    DateTime dateTime = DateTime.Now.AddMilliseconds(time);
	//                    ScheduleNotificationIOS(item.title, item.content, dateTime, true, item.hasSound);
	//#endif
	//                }
	//                break;
	//            case NotificationTimeType.DelayTime:
	//                for (int i = 0; i < item.repeatTimes; i++)
	//                {
	//                    time = item.delayTime * 1000L * (i + 1);
	//#if UNITY_ANDROID
	//                    //var notificationParams = new NotificationParams
	//                    //{
	//                    //    Id = UnityEngine.Random.Range(0, int.MaxValue),
	//                    //    Delay = TimeSpan.FromSeconds(time / 1000L),
	//                    //    Title = "Full Lives!!!",
	//                    //    Message = "Continue the journey!",
	//                    //    Ticker = "Ticker",
	//                    //    Sound = true,
	//                    //    Vibrate = true,
	//                    //    Light = true,
	//                    //    SmallIcon = NotificationIcon.Heart,
	//                    //    SmallIconColor = Color.red,
	//                    //    LargeIcon = "app_icon"
	//                    //};
	//                    //NotificationManager.SendCustom(notificationParams);
	//#elif UNITY_IOS
	//                    DateTime dateTime = DateTime.Now.AddMilliseconds(time);
	//                    ScheduleNotificationIOS(item.title, item.content, dateTime, true, item.hasSound);
	//#endif
	//                }
	//                break;
	//            case NotificationTimeType.DelayDateInWeek:
	//                DateTime currentDate = DateTime.Today;
	//                int diff = item.dateInWeek - currentDate.DayOfWeek;
	//                if (diff < 0)
	//                {
	//                    diff += 7;
	//                }
	//                for (int i = 0; i < item.repeatTimes; i++)
	//                {
	//                    today = DateTime.Today;
	//                    today = today.AddDays(diff + i * 7).AddHours(item.exactHour).AddMinutes(item.exactMinute).AddSeconds(item.exactSecond);
	//                    span = today.Subtract(DateTime.Now);
	//                    time = (long)span.TotalMilliseconds;
	//#if UNITY_ANDROID
	//                    //var notificationParams = new NotificationParams
	//                    //{
	//                    //    Id = UnityEngine.Random.Range(0, int.MaxValue),
	//                    //    Delay = TimeSpan.FromSeconds(time / 1000L),
	//                    //    Title = "Full Lives!!!",
	//                    //    Message = "Continue the journey!",
	//                    //    Ticker = "Ticker",
	//                    //    Sound = true,
	//                    //    Vibrate = true,
	//                    //    Light = true,
	//                    //    SmallIcon = NotificationIcon.Heart,
	//                    //    SmallIconColor = Color.red,
	//                    //    LargeIcon = "app_icon"
	//                    //};
	//                    //NotificationManager.SendCustom(notificationParams);
	//#elif UNITY_IOS
	//                    DateTime dateTime = DateTime.Now.AddMilliseconds(time);
	//                    ScheduleNotificationIOS(item.title, item.content, dateTime, true, item.hasSound);
	//#endif                 
	//                }
	//                break;
	//        }
	//    }
	//public interface INotification
	//{
	//    void Init();
	//    void SendNotification(int id, string title, string content, string icon, long delay, bool hasSound = false, bool hasVibrate = false);
	//    void CancelNotification(int id);
	//    void CancelAllNotification();
	//    void OnApplicationPause(bool paused);
	//}

	//public class CNotificationAndroid //: INotification
	//{
	//private string pluginpackage = "enet.utils.unity.LocalNotification";
	//private string unityPackage = "com.unity3d.player.UnityPlayerActivity";

	//void SetAppActivity(bool active)
	//{
	//    using (AndroidJavaClass cl = new AndroidJavaClass(pluginpackage))
	//    {
	//        cl.CallStatic("setAppActivity", active ? 1 : 0);
	//    }
	//}

	//public void Init()
	//{
	//    try
	//    {
	//        SetAppActivity(true);
	//    }
	//    catch (System.Exception e)
	//    {
	//        Debug.Log("error on Init Android " + e.Message);
	//    }
	//}

	//public void SendNotification(int id, string title, string content, string icon, long delay, bool hasSound = false, bool hasVibrate = false)
	//{
	//    using (AndroidJavaClass cl = new AndroidJavaClass(pluginpackage))
	//    {
	//        cl.CallStatic("startNotify", title, content, icon, hasSound ? 1 : 0, hasVibrate ? 1 : 0, delay, id, unityPackage);
	//    }
	//}

	//public void CancelNotification(int id)
	//{
	//    using (AndroidJavaClass cl = new AndroidJavaClass(pluginpackage))
	//    {
	//        cl.CallStatic("cancelNotify", id);
	//    }
	//}

	//public void CancelAllNotification()
	//{
	//    using (AndroidJavaClass cl = new AndroidJavaClass(pluginpackage))
	//    {
	//        cl.CallStatic("cancelAllNotify");
	//    }
	//}

	//public void OnApplicationPause(bool paused)
	//{
	//    if (paused)
	//    {
	//        SetAppActivity(false);
	//    }
	//    else
	//    {
	//        SetAppActivity(true);
	//    }
	//}
	//}
	#endregion

    public void SendFullLivesNotify (long time)
	{
		//time = 10;
		string title = StringValuables.gamename;
		string content = StringValuables.fulllivesMessage;
#if UNITY_ANDROID
		int id = 2; // UnityEngine.Random.Range(0, int.MaxValue);
		LocalNotification.SendNotification (id, time * 1000L, title, content, new Color32 (0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
#elif UNITY_IOS
        DateTime dateTime = DateTime.Now.AddMilliseconds(time);
        ScheduleNotificationIOS(title, content, dateTime, true, true);
#endif
	}

	public void OnApplicationPause (bool paused)
	{
#if UNITY_ANDROID
		//if (notification != null)
		//{
            
		//    else
		//    {
		//        if (enableNotify)
		//            SetupNewNotification();
		//    }
		//    notification.OnApplicationPause(paused);
		//}
		//if (!paused)
		//{
		//    if (enableNotify)
		//    {
		//        LocalNotification.CancelNotification(2);
		//    }
		//}
#elif UNITY_IOS
        if (paused)
        {
            if (enableNotify)
            {
                CancelAllNotifyIOS();
                SetupNewNotification();
            }
        }
        else
        {
            CancelAllNotifyIOS();
        }
#endif

	}

	public void CancelAllNotification ()
	{
#if UNITY_ANDROID
		LocalNotification.ClearNotifications ();
#elif UNITY_IOS
        CancelAllNotifyIOS();
#endif
	}

	// send messenger

	public void SendMessageToDevice (string title, string body, string deviceTokenId)
	{		
		StartCoroutine (SendMessageToDeviceCoroutine (title, body, deviceTokenId));
	}

	IEnumerator SendMessageToDeviceCoroutine (string title, string body, string deviceTokenId)
	{		
		string serverKey = "AAAALGWuiD0:APA91bGRrSqBXJuK-nyNwunKjVdjgliIfAe_Dou2NnafrgyIdnLEsMqSMNDMfU_Utkm9V_dD4VOidzN5Q4znUqiZVkvCkrIPw2NroLFTpUFpLUhhpvc6i4RHB5j0WAkpmePgj6TUj-tz\ncontent_copy\n \ndelete\n";

		string data = "{\"notification\":{\"title\": \"" + title + "\",\"body\": \"" + body + "\"},\"to\" : \"" + deviceTokenId + "\"}";
		//WWWForm form = new WWWForm();
		//Hashtable _header = new Hashtable ();

		Dictionary<string,string> _header = new Dictionary<string, string> ();
		_header.Add ("Content-Type", "application/json");
		//_header.Add ("Content-Length", data.Length.ToString ());
		_header.Add ("Authorization", "key=" + serverKey);


		WWW www = new WWW ("https://fcm.googleapis.com/fcm/send", Encoding.UTF8.GetBytes (data), _header);
		//	WWW www = new WWW ("https://fcm.googleapis.com/v1/registernotifycationuser/messages:send", Encoding.UTF8.GetBytes (data), _header);
		yield return www;
		//UnityEngine.Debug.Log ("result is : " + www.text);
	}
}


[System.Serializable]
[ExecuteInEditMode]
public class NotificationStruct
{
	public int id;
	public bool isClearAllWhenInit;
	public string title;
	public string messenger;
	public bool isEnable;
	public NotificationType type;
	public int delayLength;
	public NotificationDelayType delayType;
	public int delayTime;
	public bool isShowAtTime;
	public NotificationDelayType showAtTimeType;
	public Date showAtTime;
	public bool enableStartAfterTime;
	public NotificationDelayType startAfterTimeType;
	public Date startAfterTime;

	public void Init ()
	{
		
		DateTime now = DateTime.Now;
		DateTime startTime = now;
		if (enableStartAfterTime == true) {
			switch (startAfterTimeType) {
			case NotificationDelayType.Sec: 
				startTime = now.AddSeconds (startAfterTime.sec);
				break;
			case NotificationDelayType.Minute: 
				startTime = now.AddMinutes (startAfterTime.minute);
				break;
			case NotificationDelayType.Hour: 
				startTime = now.AddHours (startAfterTime.hour);
				break;
			case NotificationDelayType.Day: 
				startTime = now.AddDays (startAfterTime.day);
				break;
			case NotificationDelayType.Month: 
				startTime = now.AddMonths (startAfterTime.month);
				break;
			case NotificationDelayType.Year: 
				startTime = now.AddYears (startAfterTime.year);
				break;
			case NotificationDelayType.SpecificDate: 
				startTime = new DateTime (startAfterTime.year, startAfterTime.month, startAfterTime.day, startAfterTime.hour, startAfterTime.minute, startAfterTime.sec);
				break;
			default :
				startTime = now;
				break;
			}
		}
		if (isShowAtTime == true) {
			switch (showAtTimeType) {
			case NotificationDelayType.Sec: 
				startTime = new DateTime (startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, showAtTime.sec);
				break;
			case NotificationDelayType.Minute: 
				startTime = new DateTime (startTime.Year, startTime.Month, startTime.Day, startTime.Hour, showAtTime.minute, showAtTime.sec);
				break;
			case NotificationDelayType.Hour: 
				startTime = new DateTime (startTime.Year, startTime.Month, startTime.Day, showAtTime.hour, showAtTime.minute, showAtTime.sec);
				break;
			case NotificationDelayType.Day: 
				startTime = new DateTime (startTime.Year, startTime.Month, showAtTime.day, showAtTime.hour, showAtTime.minute, showAtTime.sec);
				break;
			case NotificationDelayType.Month: 
				startTime = new DateTime (startTime.Year, showAtTime.month, showAtTime.day, showAtTime.hour, showAtTime.minute, showAtTime.sec);
				break;
			case NotificationDelayType.Year: 
				startTime = new DateTime (showAtTime.year, showAtTime.month, showAtTime.day, showAtTime.hour, showAtTime.minute, showAtTime.sec);
				break;			
			}
		}


		switch (type) {
		case NotificationType.Once: 
			LocalNotification.SendNotification (id, (long)(startTime - now).TotalSeconds * 1000L, title, messenger, new Color32 (0xff, 0x44, 0x44, 255));
			Debug.Log ("send notification after " + (startTime - now).TotalSeconds + " sec");
			break;
		case NotificationType.Repeat: 
			for (int i = 0; i < delayLength; i++) {
				LocalNotification.SendNotification (id * 100 + i, (startTime - now), title, messenger, new Color32 (0xff, 0x44, 0x44, 255));
				Debug.Log ("send notification repeat after " + (startTime - now).TotalSeconds + " sec");
				switch (delayType) {
				case NotificationDelayType.Sec: 
					startTime = startTime.AddSeconds (delayTime);
					break;
				case NotificationDelayType.Minute: 
					startTime = startTime.AddMinutes (delayTime);
					break;
				case NotificationDelayType.Hour: 
					startTime = startTime.AddHours (delayTime);
					break;
				case NotificationDelayType.Day: 
					startTime = startTime.AddDays (delayTime);
					break;
				case NotificationDelayType.Week: 
					startTime = startTime.AddDays (delayTime * 7);
					break;
				case NotificationDelayType.Month: 
					startTime = startTime.AddMonths (delayTime);
					break;
				case NotificationDelayType.Year: 
					startTime = startTime.AddYears (delayTime);
					break;
				}
			}
			break;
		}


	}

}

[System.Serializable]
public class Date
{
	[HideInInspector]
	public long ticks;
	public int sec;
	public int minute;
	public int hour;
	public int day;
	public int month;
	public int year;
}



public enum NotificationType
{
	Once,
	Repeat
}

public enum NotificationDelayType
{
	Sec,
	Minute,
	Hour,
	Day,
	Week,
	Month,
	Year,
	SpecificDate
}

