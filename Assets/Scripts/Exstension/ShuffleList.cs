using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ShuffleList
{
	public static void Shuffle<T> (this IList<T> list)
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range (0, n + 1);  
			T value = list [k];  
			list [k] = list [n];  
			list [n] = value;  

		}  
	}

}

[System.Serializable]
public class Location
{
	public string ip;

	public string country_code;

	public string country_name;

	public string region_code;

	public string region_name;

	public string city;

	public string zip_code;

	public string time_zone;

	public double latitude;

	public double longitude;

	public int metro_code;
}
