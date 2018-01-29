using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
	public Sprite sprClick, sprRelease;
	public SpriteRenderer render;


	public void Move (Vector3 startPos, Vector3 endPos)
	{
		StartCoroutine (MoveCoroutine (startPos, endPos));
	}

	public IEnumerator MoveCoroutine (Vector3 startPos, Vector3 endPos)
	{
		while (gameObject.activeInHierarchy == true) {
			float t = 0;
			render.sprite = sprClick;
			while (t < 1) {
				transform.position = Vector3.Lerp (startPos, endPos, t);
				t += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds (0.5F);
			render.sprite = sprRelease;
			t = 0;
			while (t < 1) {
				transform.position = Vector3.Lerp (endPos, startPos, t);
				t += Time.deltaTime * 2;
				yield return null;
			}
		}
	}

}
