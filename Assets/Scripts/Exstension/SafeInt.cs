using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SafeInt
{
	[SerializeField]
	private int offset;
	[SerializeField]
	private int value;

	public SafeInt (int value = 0)
	{
		offset = Random.Range (-1000, +1000);
		this.value = value + offset;
	}

	public int GetValue ()
	{
		return value - offset;
	}

	public void Dispose ()
	{
		offset = 0;
		value = 0;
	}

	public override string ToString ()
	{
		return GetValue ().ToString ();
	}

	public static SafeInt operator + (SafeInt f1, SafeInt f2)
	{
		return new SafeInt (f1.GetValue () + f2.GetValue ());
	}

	public static SafeInt operator - (SafeInt f1, SafeInt f2)
	{
		return new SafeInt (f1.GetValue () - f2.GetValue ());
	}

	public static SafeInt operator * (SafeInt f1, SafeInt f2)
	{
		return new SafeInt (f1.GetValue () * f2.GetValue ());
	}

	public static SafeInt operator / (SafeInt f1, SafeInt f2)
	{
		return new SafeInt (f1.GetValue () / f2.GetValue ());
	}

	public static SafeInt operator % (SafeInt f1, SafeInt f2)
	{
		return new SafeInt (f1.GetValue () % f2.GetValue ());
	}

	public static SafeInt operator ++ (SafeInt f1)
	{
		return new SafeInt (f1.GetValue () + 1);
	}

	public static SafeInt operator -- (SafeInt f1)
	{
		return new SafeInt (f1.GetValue () - 1);
	}
	// ...the same for the other operators
}