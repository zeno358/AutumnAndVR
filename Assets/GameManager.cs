using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体の変数を管理
/// </summary>
public class GameManager : MonoBehaviour {

	/// <summary>
	/// 現在の参加プレイヤーの合計カウント
	/// </summary>
	int curCount;

	[SerializeField]
	Rift rift;
	/// <summary>
	/// 参加プレイヤー
	/// </summary>
	public static List<Crew> crews = new List<Crew>();

	void Start()
	{
		curCount = 0;
	}

	void Update () 
	{
		PassPlayersCount();
	}

	/// <summary>
	/// プレイヤーのカウントをリフトに渡す
	/// </summary>
	void PassPlayersCount()
	{
		int num = 0;
		foreach(Crew c in crews){
			num += c.PassCount();
		}

		rift.AddEnergy(num);
	}
}
