using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
	public static TutorialControl instance;
	public bool isShowingTutorial = false;
	public TutorialHand tutorialHand;

	void Awake ()
	{
		instance = this;
	}

	public void ShowTutorial ()
	{
		StartCoroutine (TutorialCoroutine ());
	}

	public int count = 0;
	public Grid tutorialGridPos;

	IEnumerator TutorialCoroutine ()
	{
		isShowingTutorial = true;
		int lastCount = -1;
		while (count < 3) {
			
			yield return null;
			if (lastCount != count) {
				lastCount = count;
				tutorialHand.gameObject.SetActive (false);
				yield return new WaitForSeconds (1F);
				if (count == 3) {
					isShowingTutorial = false;
					tutorialHand.gameObject.SetActive (false);
					yield break;
				}

				tutorialGridPos = GameplayControl.instance.grids [GameManager.listTutorial [count].endBlockMovePosX, GameManager.listTutorial [count].andBlockMovePosY];
				tutorialHand.gameObject.SetActive (true);
				Vector3 handPos1 = GameplayControl.instance.listStartBlock [GameManager.listTutorial [count].startBlockMovePos - 1].transform.position;
				Vector3 handPos2 = tutorialGridPos.transform.position;
				tutorialHand.Move (handPos1, handPos2);
				// clear 
				for (int x = 0; x < 8; x++) {
					for (int y = 0; y < 8; y++) {
						Grid _grid = GameplayControl.instance.grids [x, y];
						_grid.subGrid.SetActive (false);
						_grid.haveBlockCell = false;
						_grid.render.sprite = BlockColorDefine.instance.sprGrid;
						_grid.subRender.color = new Color (1, 1, 1, 1);
						_grid.hintGrid.SetActive (false);
					}
				}

				for (int x = 0; x < 8; x++) {
					for (int y = 0; y < 8; y++) {
						Grid _grid = GameplayControl.instance.grids [x, y];
						if (GameManager.listTutorial [count].grid [x * 8 + y] == 1) {
							
							_grid.subGrid.SetActive (true);
							_grid.haveBlockCell = true;
							_grid.subRender.color = new Color (1, 1, 1, 1);
							_grid.subRender.sprite = BlockColorDefine.instance.GetBlockSpriteByColor (BlockColor.BC_0);
							_grid.gridCol = (BlockColor)GameManager.dataSave.boardColor [y * 8 + x];
						} else if (GameManager.listTutorial [count].grid [x * 8 + y] == 2) {
							_grid.hintGrid.SetActive (true);
						}
					}
				}
				for (int i = 0; i < 3; i++) {
					int blockId = GameManager.listTutorial [count].startBlock [i];
					if (blockId >= 0) {
						GameplayControl.instance.listStartBlock [i].gameObject.SetActive (true);
						GameplayControl.instance.listStartBlock [i].InitBlock (blockId);
						GameplayControl.instance.listStartBlock [i].haveMatchPos = true;
					} else {
						GameplayControl.instance.listStartBlock [i].gameObject.SetActive (false);
					}

				}
			}
		}
		isShowingTutorial = false;
		tutorialHand.gameObject.SetActive (false);
	}

}

[System.Serializable]
public class TutorialModule
{
	public List<int> grid;

	public List<int> startBlock;

	public string text;

	public int startBlockMovePos;

	public int endBlockMovePosX;

	public int andBlockMovePosY;
}

[System.Serializable]
public class Tutorial
{
	public List<TutorialModule> tutorial;
}