using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffectDefine : MonoBehaviour
{
	public static TextEffectDefine instance;

	void Awake ()
	{
		instance = this;
	}

	[System.Serializable]
	public struct TextEffectStruct
	{
		public TextEffectType type;
		public Sprite sprite;
	}

	public List< TextEffectStruct> listTextEffectStruct;

	public Sprite GetTextEffectSpriteByType (TextEffectType type)
	{
		foreach (TextEffectStruct te in listTextEffectStruct) {
			if (te.type == type) {
				return te.sprite;
			}
		}
		return null;
	}
}

[System.Serializable]
public enum TextEffectType
{
	TE_COOL,
	TE_GOOD,
	TE_EXCELENT
}