using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体の変数を管理
/// </summary>
public class AutumnVRGameManager : MonoBehaviour {

	/// <summary>
	/// 現在の参加プレイヤーの合計カウント
	/// </summary>
	public int totalCount;
	public static int count;

	/// <summary>
	/// ゴールとみなす高度
	/// </summary>
	public static float goalHeight = 15f;

	[SerializeField]
	Muscle muscle;
	/// <summary>
	/// 参加プレイヤー
	/// </summary>
	public static List<Crew> crews = new List<Crew>();

	void Start()
	{
		count = 0;
	}

	void Update () 
	{
	//	PassPlayersCount();
		PassCount();
	}

	void PassCount()
	{
		muscle.AddEnergy(count);
		count = 0;
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

		muscle.AddEnergy(num);
	}

	// ゲームオーバーしてタイトル画面

	// ゲームクリアしてスコア表示
}
