using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ImageCaching : SingletonTemplate<ImageCaching>
{
    private Dictionary<string, Sprite> dicts;

    private class Helper : MonoBehaviour { }

    public ImageCaching()
    {
        dicts = new Dictionary<string, Sprite>();
    }

    public static void LoadImage(string url, callback<Sprite> callback)
    {
        Instance._LoadImage(url, callback);
    }

    private void _LoadImage(string url, callback<Sprite> callback)
    {
        //helper.StartCoroutine(url, callback);
        WWWRequest.GET(url, (w) =>
        {
            Sprite s = null;
            if (string.IsNullOrEmpty(w.error))
            {
                Texture2D txt = w.texture;
                s = Sprite.Create(txt, new Rect(0, 0, txt.width, txt.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.Log(w.error + " - " + url);
            }
            if (callback != null)
            {
                callback(s);
            }
        });
    }
}
