using UnityEngine;
using UnityEngine.SceneManagement;
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

	/// <summary>
	/// ゲームの最中か？
	/// </summary>
	public static bool running;

	void Start()
	{
		RessetParametersAndLoadTitleScene();
	}

	/// <summary>
	/// パラメータを初期化してタイトルシーンを読み込む
	/// </summary>
	static  void RessetParametersAndLoadTitleScene()
	{
		count = 0;
		gameTimer = 0;

		running = false;
		SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
	}
		
	void Update () 
	{
		if(!running)
		{
			return;
		}

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
			if(!running)
			{
				return;
			}
			running = false;
			StartCoroutine( ShowTimeOverExpression() );
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
		
	/// <summary>
	/// タイムオーバー演出
	/// </summary>
	private IEnumerator ShowTimeOverExpression()
	{
		int height = (int)Mathf.Floor( Muscle.height );
		Debug.LogErrorFormat("時間切れ！あなたが到達した高度は{0}", height);

		// マッチョが悲しそうなセリフ
		muscle.Down();

		// 時間切れの旨の到達高度を表示

		yield return new WaitForSeconds(5f);

		// タイトルに戻る
		RessetParametersAndLoadTitleScene();
	}


	// ゲームクリアしてスコア表示
	public static IEnumerator GameClear()
	{
		Debug.LogError("ゴール！！");
		AutumnVRGameManager.running = false;

		yield return new WaitForSeconds(5f);

		// タイトルに戻る
		RessetParametersAndLoadTitleScene();
	}

}
