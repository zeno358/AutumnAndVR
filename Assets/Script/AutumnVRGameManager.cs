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
	public static int totalCount;

	/// <summary>
	/// 現在のカウント
	/// 筋肉が上昇すると現象する
	/// </summary>
	static int count;

	/// <summary>
	/// ゴールとみなす高度
	/// </summary>
	public static float goalHeight = 120f;

	/// <summary>
	/// ゲーム終了か？
	/// </summary>
	public static bool over = false;

	[SerializeField]
	Muscle muscle;
	/// <summary>
	/// 参加プレイヤー
	/// </summary>
	public static List<CrewMove> players;

	public static float gameTimer;

	void Start()
	{
		count = 0;
		gameTimer = 0;
	}

	void Update () 
	{
		PassPlayersCount();
	//	PassCount();
		gameTimer += Time.deltaTime;
	}

	public static void AddCount()
	{
		count++;
		totalCount++;
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
		foreach(CrewMove c in players){
			num += c.PassCount();
		}

		muscle.AddEnergy(num);
	}

	// ゲームオーバーしてタイトル画面

	// ゲームクリアしてスコア表示
}
