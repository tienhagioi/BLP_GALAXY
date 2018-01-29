using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonatLogic : MonoBehaviour
{
	public static PattemType type;
	private static Config[] ListEasy = new Config[12] {
		new Config () {
			type = PattemType.O0,
			weight = 8
		},
		new Config () {
			type = PattemType.O1,
			weight = 10
		},
		new Config () {
			type = PattemType.O2,
			weight = 4
		},
		new Config () {
			type = PattemType.I0,
			weight = 10
		},
		new Config () {
			type = PattemType.I1,
			weight = 10
		},
		new Config () {
			type = PattemType.I2,
			weight = 10
		},
		new Config () {
			type = PattemType.I3,
			weight = 5
		},
		new Config () {
			type = PattemType.L0,
			weight = 10
		},
		new Config () {
			type = PattemType.L1,
			weight = 7
		},
		new Config () {
			type = PattemType.T0,
			weight = 10
		},
		new Config () {
			type = PattemType.LB0,
			weight = 10
		},
		new Config () {
			type = PattemType.Z0,
			weight = 0
		}
	};
	private static Config[] ListMedium = new Config[12] {
		new Config () {
			type = PattemType.O0,
			weight = 4
		},
		new Config () {
			type = PattemType.O1,
			weight = 10
		},
		new Config () {
			type = PattemType.O2,
			weight = 6
		},
		new Config () {
			type = PattemType.I0,
			weight = 10
		},
		new Config () {
			type = PattemType.I1,
			weight = 10
		},
		new Config () {
			type = PattemType.I2,
			weight = 10
		},
		new Config () {
			type = PattemType.I3,
			weight = 7
		},
		new Config () {
			type = PattemType.L0,
			weight = 10
		},
		new Config () {
			type = PattemType.L1,
			weight = 10
		},
		new Config () {
			type = PattemType.T0,
			weight = 10
		},
		new Config () {
			type = PattemType.LB0,
			weight = 10
		},
		new Config () {
			type = PattemType.Z0,
			weight = 8
		}
	};
	private static Config[] ListHard = new Config[12] {
		new Config () {
			type = PattemType.O0,
			weight = 2
		},
		new Config () {
			type = PattemType.O1,
			weight = 10
		},
		new Config () {
			type = PattemType.O2,
			weight = 8
		},
		new Config () {
			type = PattemType.I0,
			weight = 10
		},
		new Config () {
			type = PattemType.I1,
			weight = 10
		},
		new Config () {
			type = PattemType.I2,
			weight = 10
		},
		new Config () {
			type = PattemType.I3,
			weight = 10
		},
		new Config () {
			type = PattemType.L0,
			weight = 10
		},
		new Config () {
			type = PattemType.L1,
			weight = 10
		},
		new Config () {
			type = PattemType.T0,
			weight = 10
		},
		new Config () {
			type = PattemType.LB0,
			weight = 10
		},
		new Config () {
			type = PattemType.Z0,
			weight = 10
		},
	};

	public static PattemType GetFixedRandomType (int score)
	{
		switch ((long)score <= SUGame.Get<SURemoteConfig> ().score_level_2_value ? ((long)score <= SUGame.Get<SURemoteConfig> ().score_level_1_value ? Level.Easy : Level.Medium) : Level.Hard) {
		case Level.Easy:
			return RandomWeight (ListEasy);
		case Level.Medium:
			return RandomWeight (ListMedium);
		default:
			return RandomWeight (ListHard);
		}
	}

	private static PattemType RandomWeight (Config[] list)
	{
		int max = 0;
		for (int index = 0; index < list.Length; ++index)
			max += list [index].weight;
		int num1 = Random.Range (0, max);
		int num2 = 0;
		for (int index = 0; index < list.Length; ++index) {
			if (list [index].weight + num2 >= num1)
				return list [index].type;
			num2 += list [index].weight;
		}
		return list [0].type;
	}

	public static int GetRandomBlockId (int score)
	{
		return 	ConvertPattemTypeToSUGameData (GetFixedRandomType (score));
	}


	private enum Level
	{
		Easy,
		Medium,
		Hard,
	}

	private struct Config
	{
		public PattemType type;
		public int weight;
	}

	public static int ChooseRandom (List<int> l)
	{
		l.Shuffle ();
		return l [0];
	}

	public static int ChooseRandom (int x0, int x1, int x2, int x3)
	{
		List<int> l = new List<int> (){ x0, x1, x3, x3 };
		l.Shuffle ();
		return l [0];
	}

	public static int ConvertPattemTypeToSUGameData (PattemType _type)
	{
		switch (_type) {
		case PattemType.O0: 
			return 0;
		case PattemType.O1: 
			return 11;
		case PattemType.O2: 
			return 30;
		case PattemType.I0: 
			// 2 vien doc , ngang 
			return Random.Range (0, 100) > 50 ? 2 : 1;
		case PattemType.I1: 
			// 3 vien doc ngang 
			return Random.Range (0, 100) > 50 ? 3 : 8;
		case PattemType.I2: 
			// 4 vien doc - ngang 
			return Random.Range (0, 100) > 50 ? 9 : 10;
		case PattemType.I3: 
			// 5 vien doc - ngang 
			return Random.Range (0, 100) > 50 ? 29 : 28;
		case PattemType.L0: 
			return ChooseRandom (4, 5, 6, 7);
		case PattemType.L1: 
			return ChooseRandom (24, 25, 26, 27);
		case PattemType.T0: 
			return ChooseRandom (20, 21, 22, 23);		
		case PattemType.Z0: 
			return ChooseRandom (31, 32, 33, 34);
		case PattemType.LB0: 
			return ChooseRandom (new List<int> (){ 12, 13, 14, 15, 16, 17, 18, 19 });
		default :
			return 0;
		}
	}

	public enum PattemType
	{
		O0,
		O1,
		O2,
		I0,
		I1,
		I2,
		I3,
		L0,
		L1,
		T0,
		LB0,
		Z0,
	}
}
