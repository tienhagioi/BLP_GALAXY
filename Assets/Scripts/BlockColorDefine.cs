using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlockColorDefine : MonoBehaviour
{
	public static BlockColorDefine instance;
	public Sprite sprCellLose, sprGrid;

	void Awake ()
	{
		instance = this;
	}

	[System.Serializable]
	public struct BlockColorStruct
	{
		public BlockColor col;
		public Sprite blockSprite;
		public GameObject breakEffect;
	}

	public List<BlockColorStruct> listBlockColorStruct;

	public Sprite GetBlockSpriteByColor (BlockColor col)
	{
		foreach (BlockColorStruct str in listBlockColorStruct) {
			if (str.col == col) {
				return str.blockSprite;
			}
		}
		return null;
	}

	public GameObject GetBreakEffectByColor (BlockColor col)
	{
		foreach (BlockColorStruct str in listBlockColorStruct) {
			if (str.col == col) {
				return str.breakEffect;
			}
		}
		return null;
	}
}

[System.Serializable]
public enum BlockColor
{
	BC_0,
	BC_1,
	BC_2,
	BC_3,
	BC_4
}