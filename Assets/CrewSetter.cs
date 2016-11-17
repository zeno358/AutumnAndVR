using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーを指定位置にセットする
/// </summary>
public class CrewSetter : Photon.MonoBehaviour 
{

	public static CrewSetter instance;
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
	/// シングルモード用おプレハブ
	/// </summary>
	[SerializeField]
	GameObject NetworkCrewPrefab;

	void Start()
	{
		instance = this;
	}

	/// <summary>
	/// プレイヤーを配置する
	/// </summary>
	public void SetCrew()
	{
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = PhotonNetwork.Instantiate("NetworkCrew", this.transform.position, this.transform.rotation, 0);
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<CrewMove>();
		//myPlayer.enabled = true;

		// 自分が何番目にルームに入ったプレイヤーかどうかでIDを設定
		print( myPlayer.photonView.ownerId.ToString() );
		myPlayer.id = PhotonNetwork.room.playerCount;
		Debug.Log("あなたのIDは [" + myPlayer.id.ToString() +" ]" );

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
		if( myPlayer.id == 1 )
		{
			myPlayer.transform.SetParent(playerPos1);
		}else
		{
			myPlayer.transform.SetParent(playerPos2);
		}

		myPlayer.transform.localPosition = Vector3.zero;
		myPlayer.transform.localRotation = Quaternion.identity;

		//myPlayer.transform.parent = null;
	}

	public void SetupCrewForSingleMode()
	{
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = Instantiate(NetworkCrewPrefab, this.transform.position, this.transform.rotation) as GameObject;
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<CrewMove>();
		//myPlayer.enabled = true;

		print( myPlayer.photonView.ownerId.ToString() );
		myPlayer.id = 1;
		Debug.Log("あなたのIDは [" + myPlayer.id.ToString() +" ]" );

		// プレイヤーのポジションをセット
		SetPlayerTransform();

		myPlayer.initialized = true;

		// カゴを持たせる
		myPlayer.EnalbeBag();
	}
}
