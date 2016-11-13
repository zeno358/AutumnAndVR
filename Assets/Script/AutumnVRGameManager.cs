using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体の変数を管理
/// </summary>
public class AutumnVRGameManager : MonoBehaviour 
{
	/// <summary>
	/// 制限時間
	/// </summary>
	public static float timeLimitSec = 120f;

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
	public static bool EndOfGame = false;

	/// <summary>
	/// 時間切れか？
	/// </summary>
	public static bool TimeOver = false;

	/// <summary>
	/// 筋肉
	/// </summary>
	[SerializeField]
	Muscle muscle;

	/// <summary>
	/// 参加プレイヤー
	/// </summary>
	public static List<CrewMove> players;

	/// <summary>
	/// 経過時間
	/// </summary>
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

		UpdateGameTimer();
	}

	/// <summary>
	/// 経過時間を更新
	/// 制限時間を超えたらタイムオーバー
	/// </summary>
	private void UpdateGameTimer()
	{
		gameTimer += Time.deltaTime;

		if( gameTimer >= timeLimitSec )
		{
			TimeOver = EndOfGame = true;
			ShowTimeOverExpression();
		}
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

	/// <summary>
	/// タイムオーバー演出
	/// </summary>
	private void ShowTimeOverExpression()
	{
		Debug.LogError("時間切れ！");

		// マッチョが悲しそうなセリフ

		// 時間切れの旨の到達高度を表示
	}


	// ゲームクリアしてスコア表示

}
