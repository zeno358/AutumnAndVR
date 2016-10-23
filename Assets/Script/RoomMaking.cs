using UnityEngine;
using System.Collections;

public class RoomMaking : Photon.MonoBehaviour 
{
	/// <summary>
	/// ルーム名
	/// </summary>
	string roomName = "autumnvr";

	/// <summary>
	/// ゲームを開始するのに必要なプレイヤー数
	/// </summary>
	int playerNumNeeded = 2;

	/// <summary>
	/// 自分が管理するプレイヤー
	/// </summary>
	PlayerMove myPlayer;

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

	// Use this for initialization
	void Start () {
		//	魔法の呪文
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	// Update is called once per frame
	void Update () {

	}

	//  ランダムでルームを選び入る
	void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom();
	}

	//  JoinRandomRoom()が失敗した(false)時に呼ばれる
	void OnPhotonRandomJoinFailed(){
		//  部屋に入れなかったので自分で作る

		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 2;
		TypedLobby lobby = new TypedLobby();

		PhotonNetwork.CreateRoom (roomName, options, lobby);
	}

	//  ルームに入れた時に呼ばれる（自分の作ったルームでも）
	void OnJoinedRoom(){
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = PhotonNetwork.Instantiate("NetworkCube", this.transform.position, this.transform.rotation, 0);
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<PlayerMove>();
		myPlayer.enabled = true;

		Debug.Log("自分か？ = " + myPlayer.photonView.isMine );

		// 自分が何番目にルームに入ったプレイヤーかどうかでIDを設定
		myPlayer.id = PhotonNetwork.room.playerCount;
		Debug.Log("あなたのIDは [" + myPlayer.id.ToString() +" ]" );

		StartCoroutine( WaitPlayersAndStartGame() );
	}

	/// <summary>
	/// プレイヤーの参加を待ってゲームを開始する
	/// </summary>
	/// <returns>The players and start game.</returns>
	IEnumerator WaitPlayersAndStartGame()
	{
		// プレイヤーの参加を待つ
		while( PhotonNetwork.room.playerCount < playerNumNeeded )
		{
			Debug.Log("プレイヤーの参加を待っています... 現在のプレイヤー数 : " + PhotonNetwork.room.playerCount.ToString() + "/" + playerNumNeeded.ToString() );
			yield return new WaitForSeconds(2f);
		}

		Debug.Log("プレイヤーが集まりました");

		SetPlayerPosition();

	}

	/// <summary>
	/// プレイヤーのポジションを設定する
	/// </summary>
	private void SetPlayerPosition()
	{
		if( myPlayer.id == 1 )
		{
			myPlayer.transform.position = playerPos1.position;
		}else
		{
			myPlayer.transform.position = playerPos2.position;
		}
	}
}

