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
	public static int timeLimitSec = 120;

	/// <summary>
	/// 現在の参加プレイヤーの合計カウント
	/// </summary>
	public static int totalCount;

	/// <summary>
	/// 現在のカウント
	/// 筋肉が上昇すると減少する
	/// </summary>
	static int count;

	/// <summary>
	/// ゴールとみなす高度
	/// </summary>
	public static int goalHeight = 300;

	/// <summary>
	/// 筋肉
	/// </summary>
	[SerializeField]
	Muscle muscle;

	/// <summary>
	/// 開始時の筋肉ボイス
	/// </summary>
	[SerializeField]
	AudioClip vo_start;

	/// <summary>
	/// クリア時の効果音
	/// </summary>
	[SerializeField]
	AudioClip se_clear;

	/// <summary>
	/// クリア時の筋肉ボイス
	/// </summary>
	[SerializeField]
	AudioClip vo_clear;

	/// <summary>
	/// 失敗時の効果音
	/// </summary>
	[SerializeField]
	AudioClip se_failed;

	/// <summary>
	/// 失敗時のボイス
	/// </summary>
	[SerializeField]
	AudioClip vo_failed;

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

	/// <summary>
	/// 高度到達演出が発生する頻度
	/// </summary>
	public static int measureExpInterval = 25;

	void Start()
	{
		ResetParametersAndLoadTitleScene();
	}

	/// <summary>
	/// パラメータを初期化してタイトルシーンを読み込む
	/// </summary>
	public static void ResetParametersAndLoadTitleScene()
	{
		totalCount = 0;
		count = 0;
		gameTimer = 0;

		running = false;

		GameObject b = GameObject.Find ("Bag");
		if( b != null )
		{
			b.GetComponent<Bag>().InitModel();	
		}

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
	/// /ゲーム開始演出
	/// </summary>
	public void ShowGameStartExpression()
	{
		StartCoroutine (_ShowGameStartExpression ());
	}
	private IEnumerator _ShowGameStartExpression()
	{	
		// 筋肉ボイス
		muscle.PlaySe(vo_start);

		yield return new WaitForSeconds(1f);

		// 文字演出

		yield return new WaitForSeconds(1f);


		// BGM再生
		muscle.SetBGM(true);

		running = true;
	}

	/// <summary>
	/// タイムオーバー演出
	/// </summary>
	private IEnumerator ShowTimeOverExpression()
	{
		for (int i = Chestnut.cList.Count-1 ; i >= 0  ; i--) {
			var c = Chestnut.cList [i];
			if (c != null) {
				
				Destroy (c.gameObject);
			}
		}
		Chestnut.cList.Clear ();

		int height = (int)Mathf.Floor( Muscle.height );
		Debug.LogErrorFormat("時間切れ！あなたが到達した高度は{0}", height);

		// BGM停止
		muscle.SetBGM(false);

		// 効果音
		muscle.PlaySe(se_failed);

		// 筋肉ボイス
		muscle.PlaySe(vo_failed);

		yield return new WaitForSeconds(1f);

		// 文字演出
		muscle.DisplayReachedHeight();

		yield return new WaitForSeconds(5f);

		yield return WaitInput();

		// タイトルに戻る
		ResetParametersAndLoadTitleScene();
	}
	/// <summary>
	/// ゲームクリア演出
	/// </summary>
	public IEnumerator ShowGameClearExpression()
	{
		for (int i = Chestnut.cList.Count-1 ; i >= 0  ; i--) {
			var c = Chestnut.cList [i];
			if (c != null) {
				Destroy (c.gameObject);
			}
		}
		Chestnut.cList.Clear ();

		Debug.LogError("ゴール！！");
		AutumnVRGameManager.running = false;

		// BGM停止
		muscle.SetBGM(false);

		// 効果音
		muscle.PlaySe(se_clear);

		yield return new WaitForSeconds(1.5f);

		// 文字演出
		yield return new WaitForSeconds(1.5f);

		// 筋肉ボイス
		muscle.PlaySe(vo_clear);

		yield return WaitInput();

		// タイトルに戻る
		ResetParametersAndLoadTitleScene();
	}

	IEnumerator WaitInput()
	{
		do {
			if(Input.anyKeyDown)
			{
				yield break;
			}
			yield return null;
		} while(true);
	}

}
