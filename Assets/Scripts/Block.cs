
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

	public GameObject blockCellPrefap;
	public SpriteRenderer[,] listBlockCell;
	Color col1 = new Color (1, 1, 1, 1);
	Color col2 = new Color (1, 1, 1, 0.4F);
	BlockColor _blockColor;
	[HideInInspector]
	public List<Grid> listGridsCanMatch;

	public BlockColor blockColor {
		get { return _blockColor; }
		set {
			_blockColor = value;
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					listBlockCell [x, y].sprite = BlockColorDefine.instance.GetBlockSpriteByColor (value);
				}
			}
		}
	}

	void Awake ()
	{
		listGridsCanMatch = new List<Grid> ();
		listBlockCell = new SpriteRenderer[5, 5];
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				GameObject blockCellObj = (GameObject)Instantiate (blockCellPrefap, transform.position + new Vector3 (-2 + x, 2 - y, 0), Quaternion.Euler (0, 0, 0));
				blockCellObj.transform.parent = transform;
				blockCellObj.name = "Cell_" + x + "_" + y;
				listBlockCell [x, y] = blockCellObj.GetComponent<SpriteRenderer> ();
			}
		}
		transform.localScale = new Vector3 (0.4F, 0.4F, 0.4F);

		//id = Random.Range (0, GameManager.listBlockData.Count);
		//InitBlock (id);
	}

	public int id;
	bool _haveMatchPos = true;

	public bool haveMatchPos {
		get {
			return _haveMatchPos;
		}
		set {
			_haveMatchPos = value;
			Color col = col1;
			if (value == false) {
				col = col2;
			}
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					listBlockCell [x, y].color = col;
				}
			}
		}
	}

	public void InitBlock (int _id)
	{
		
		//Debug.Log ("Init id : " + id);
		id = _id;
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				bool isActive = GameManager.listBlockData [_id] [x, y] == 1 ? true : false;
				listBlockCell [x, y].gameObject.SetActive (isActive);
			}
		}
		blockColor = (BlockColor)Random.Range (0, BlockColorDefine.instance.listBlockColorStruct.Count);
	}



	void OnMouseDown ()
	{
		if (gameObject.activeInHierarchy == true && haveMatchPos == true && GameplayControl.instance.blockMoveOutCoroutine == null) {
			Application.targetFrameRate = 60;
			GameplayControl.instance.startBlock = this;
			gameObject.SetActive (false);
			AudioManager.instance.PlaySound (AudioClipType.AC_TAKE_BLOCK);
			if (UITopControl.instance.isShowingPauseMenu == true) {
				UITopControl.instance.ShowPauseMenu ();
			}
		}
	}

	public void NewBlockEffect ()
	{
		StartCoroutine (NewBlockEffectCoroutine ());
	}

	IEnumerator NewBlockEffectCoroutine ()
	{
		Vector3 startScale = new Vector3 (0, 0, 0);
		Vector3 endScale = new Vector3 (0.4F, 0.4F, 0.4F);
		float t = 0; 
		while (t < 1) {
			t += Time.deltaTime * 5;
			transform.localScale = Vector3.Lerp (startScale, endScale, t);
			yield return null;
		}
		transform.localScale = endScale;
	}

}
