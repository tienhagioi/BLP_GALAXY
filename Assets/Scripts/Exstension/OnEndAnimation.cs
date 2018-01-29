using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEndAnimation : MonoBehaviour
{

	Animator ani;

	void Awake ()
	{
		ani = GetComponent<Animator> ();
	}

	public void StopAni ()
	{
		ani.SetFloat ("Speed", 0F);
	}

	public void DisableAni ()
	{
		ani.enabled = false;
	}

	public void Disable ()
	{
		gameObject.SetActive (false);
	}

	public void Destroy ()
	{
		Destroy (gameObject);
	}
}
