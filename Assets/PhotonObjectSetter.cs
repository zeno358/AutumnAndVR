using UnityEngine;
using System.Collections;

/// <summary>
/// Photonで管理するものを指定位置にセットする
/// </summary>
public class PhotonObjectSetter : Photon.MonoBehaviour 
{

	public static PhotonObjectSetter instance;
	/// <summary>
	/// プレイヤー１の位置 
	/// </summary>
	[SerializeField]
	Transform playerPos1;

	/// <summary>
	/// プレイヤー２の位置
	/// </summary
	[SerializeField]
	Transform playerPos2;

	/// <summary>
	/// 自分が管理するプレイヤー
	/// </summary>
	CrewMove myPlayer;

	/// <summary>
	/// シングルモード用プレハブ
	/// </summary>
	[SerializeField]
	GameObject NetworkCrewPrefab;

	/// <summary>
	/// 筋肉
	/// </summary>
	[SerializeField]
	Muscle muscle;

	void Start()
	{
		instance = this;
	}

	/// <summary>
	/// 各オブジェクトを配置する
	/// </summary>
	public void Init()
	{
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = PhotonNetwork.Instantiate("NetworkCrew", this.transform.position, this.transform.rotation, 0);
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<CrewMove>();
		//myPlayer.enabled = true;

		muscle.Init();

		// 自分が何番目にルームに入ったプレイヤーかどうかでIDを設定
		print( myPlayer.photonView.ownerId.ToString() );
		myPlayer.order = PhotonNetwork.room.playerCount;
		Debug.Log("あなたは[" + myPlayer.order.ToString() +" ]番目のプレイヤー" );

		StartCoroutine( WaitPlayersAndStartGame() );
	}

	/// <summary>
	/// プレイヤーの参加を待ってゲームを開始する
	/// </summary>
	IEnumerator WaitPlayersAndStartGame()
	{
		// プレイヤーの参加を待つ
		while( PhotonNetwork.room.playerCount < CrewRoomMaking.playerNumNeeded )
		{
			Debug.Log("プレイヤーの参加を待っています... 現在のプレイヤー数 : " + PhotonNetwork.room.playerCount.ToString() + "/" + CrewRoomMaking.playerNumNeeded.ToString() );
			yield return new WaitForSeconds(3f);
		}

		Debug.Log("プレイヤーが集まりました");

		while(true)
		{
			if( AutumnVRGameManager.players == null )
			{
				Debug.Log("プレイヤーリストの生成を待機中..." );

				yield return new WaitForSeconds(5);

				continue;
			}

			if( AutumnVRGameManager.players.Count >= CrewRoomMaking.playerNumNeeded)
			{
				break;
			}

			yield return null;
		}
		// プレイヤーのポジションをセット
		SetPlayerTransform();

		myPlayer.initialized = true;

		// カゴを持たせる
		myPlayer.EnalbeBag();
	}

	/// <summary>
	/// プレイヤーのトランスフォームを設定する
	/// </summary>
	private void SetPlayerTransform()
	{
		for( int i=0 ; i<AutumnVRGameManager.players.Count ; i++)
		{
			CrewMove p = AutumnVRGameManager.players[i];

			if( p == null )
			{
				Debug.Log("Playerがnull");
				continue;
			}

			switch(p.order)
			{
			case 1 :
				p.transform.SetParent(playerPos1);

				break;
			case 2:
				p.transform.SetParent(playerPos2);

			break;
				default:

				p.order = myPlayer.order != 1 ? 1 : 2;
				Debug.Log("プレイヤーの順番が想定外 : [ " + p.order.ToString() + " ]だったので[ " + p.order.ToString() + " ]に変更");	
				p.transform.SetParent( p.order == 1 ? playerPos1 : playerPos2 );
				break;
			}

			p.transform.localPosition = Vector3.zero;
			p.transform.localRotation = Quaternion.identity;
		}
	}

	public void SetupCrewForSingleMode()
	{
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = Instantiate(NetworkCrewPrefab, this.transform.position, this.transform.rotation) as GameObject;
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<CrewMove>();
		//myPlayer.enabled = true;

		print( myPlayer.photonView.ownerId.ToString() );
		myPlayer.order = 1;
		Debug.Log("あなたのIDは [" + myPlayer.order.ToString() +" ]" );

		// プレイヤーのポジションをセット
		SetPlayerTransform();

		myPlayer.initialized = true;

		// カゴを持たせる
		myPlayer.EnalbeBag();
	}
}
