using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using Assets.USecurity;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class DataProtectionExtensions
{
	#if UNITY_EDITOR
	[MenuItem ("DataProtect/Encrypt Block Data")]
	public static void EncryptBlockData ()
	{
		TextAsset ta = Resources.Load ("BlockData") as TextAsset;
		try {
			BlockData data = JsonUtility.FromJson<BlockData> (ta.text);
			string encryptData = AES.Encrypt (ta.text, StringValuables.password);
			string path = "Assets/Resources/";
			using (FileStream fs = new FileStream (path + "BlockData" + ".json", FileMode.Create)) {
				using (StreamWriter writer = new StreamWriter (fs)) {
					writer.Write (encryptData);
				}
			}
			AssetDatabase.Refresh ();

		} catch (Exception e) {
			Debug.Log ("Da ta da duoc ma hoa roi " + e.ToString ());
		}
	}
	#endif
}
