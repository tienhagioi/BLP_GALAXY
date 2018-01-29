using UnityEngine;
using System.Collections;

public class WWWRequest : SingletonTemplate<WWWRequest>
{
    private Helper helper;
    public WWWRequest()
    {
        GameObject go = new GameObject("___WWWRequest___");
        helper = go.AddComponent<Helper>();
        Object.DontDestroyOnLoad(go);
    }

    public static void GET(string url,callback<WWW> callback)
    {
        Instance._GET(url,callback);
    }   
        
    private void _GET(string url,callback<WWW> callback)
    {
        helper.StartCoroutine(Coroutine_GET(url, callback));
    }     

    private IEnumerator Coroutine_GET(string url, callback<WWW> callback)
    {
        WWW w = new WWW(url);
        yield return w;
        if (callback != null)
        {
            callback(w);
        }
        w.Dispose();
    }

    private class Helper : MonoBehaviour { }
}
