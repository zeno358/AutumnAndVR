using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体の変数を管理
/// </summary>
public class GameManagerTest : MonoBehaviour 
{
	public static GameManagerTest instance;

	/// <summary>
	/// シングルモード
	/// </summary>
	public bool singleMode = false;
	public static bool _singleMode;

	/// <summary>
	/// 制限時間
	/// </summary>
	public int timeLimitSec = 120;
	public static int _timeLimitSec = 120;

	/// <summary>
	/// ゴールとみなす高度
	/// </summary>
	public int goalHeight = 200;
	public static int _goalHeight = 200;

	/// <summary>
	/// 筋肉
	/// </summary>
	public MuscleTest muscle;

	/// <summary>
	/// 開始時の筋肉ボイス
	/// </summary>
	public AudioClip vo_start;

	/// <summary>
	/// クリア時の効果音
	/// </summary>
	public AudioClip se_clear;

	/// <summary>
	/// クリア時の筋肉ボイス
	/// </summary>
	public AudioClip vo_clear;

	/// <summary>
	/// 失敗時の効果音
	/// </summary>
	public AudioClip se_failed;

	/// <summary>
	/// 失敗時のボイス
	/// </summary>
	public AudioClip vo_failed;

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
		instance = this;
		_singleMode = singleMode;

		_goalHeight = goalHeight;

		_timeLimitSec = timeLimitSec;

		ResetParametersAndLoadTitleScene();
	}

	/// <summary>
	/// パラメータを初期化してタイトルシーンを読み込む
	/// </summary>
	public static void ResetParametersAndLoadTitleScene()
	{
		gameTimer = 0;

		running = true;

		if (MuscleTest.instance != null) {
			MuscleTest.instance.SetToOrigin ();
		}

		GameObject b = GameObject.Find ("Bag");
		if( b != null )
		{
			b.GetComponent<BagTest>().InitModel();	
		}

		//生成済みプレイヤーを削除
		/*
		if( players != null )
		{
			for(int i = players.Count-1 ; i >= 0 ; i --)
			{

				PhotonNetwork.Destroy( players[i].gameObject );
			}
		}
		*/

		//SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
	}

	void Update () 
	{
		if(!running)
		{
			return;
		}

		UpdateGameTimer();

		CheckInput();
	}

	/// <summary>
	/// 経過時間を更新
	/// 制限時間を超えたらタイムオーバー
	/// </summary>
	private void UpdateGameTimer()
	{
		gameTimer += Time.deltaTime;

		if( gameTimer >= _timeLimitSec )
		{
			if(!running)
			{
				return;
			}
			running = false;

			StartCoroutine( ShowTimeOverExpression() );
		}
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

		// 全プレイヤーが準備できるまで待つ

		// BGM再生
		muscle.SetBGM(true);

		running = true;
	}

	/// <summary>
	/// タイムオーバー演出
	/// </summary>
	private IEnumerator ShowTimeOverExpression()
	{
		if (Chestnut.cList != null) {
			for (int i = Chestnut.cList.Count - 1; i >= 0; i--) {
				var c = Chestnut.cList [i];
				if (c != null) {

					Destroy (c.gameObject);
				}
			}
			Chestnut.cList.Clear ();
		}

		int height = (int)Mathf.Floor( MuscleTest.height );
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
		if (Chestnut.cList != null) {
			for (int i = Chestnut.cList.Count - 1; i >= 0; i--) {
				var c = Chestnut.cList [i];
				if (c != null) {
					Destroy (c.gameObject);
				}
			}
			Chestnut.cList.Clear ();
		}

		Debug.LogError("ゴール！！");
		running = false;

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

	/// <summary>
	/// 入力を待機
	/// </summary>
	IEnumerator WaitInput()
	{
		do {
			if(Input.anyKeyDown)
			{
				yield break;
			}

			Debug.Log("入力待機中");
			yield return null;
		} while(true);
	}

	/// <summary>
	/// 入力チェックを行う
	/// 主にデバッグ用
	/// </summary>
	void CheckInput()
	{
		if( ( Input.GetKey( KeyCode.LeftControl ) 
			|| Input.GetKey( KeyCode.RightControl ) 
			|| Input.GetKey( KeyCode.LeftAlt ) 
			|| Input.GetKey( KeyCode.RightAlt ) ) 
			&& Input.GetKeyDown( KeyCode.Q ) )
		{
			// ctrl + Q で強制タイトル遷移
			ResetParametersAndLoadTitleScene();
		}
	}
}
