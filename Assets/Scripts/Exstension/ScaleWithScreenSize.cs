using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithScreenSize : MonoBehaviour
{
	void Start ()
	{
		ScaleFitScreenSize ();
	}

	public void ScaleFitScreenSize ()
	{
		float StandardSizex = 720f / 1280f;
		float targetSizex = Screen.width * 1f / Screen.height;
		//float StandardSizey = 1280f / 720f;
		//float targetSizey = Screen.height * 1f / Screen.width;
		float ratex = targetSizex / StandardSizex;
		//float ratey = StandardSizex / targetSizex;
		transform.localScale = new Vector3 (transform.localScale.x * ratex, transform.localScale.y, 0f);

	}
}
