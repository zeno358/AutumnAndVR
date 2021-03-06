﻿using UnityEngine;
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

	/// <summary>
	/// シングルモード
	/// </summary>
	public bool singleMode = false;
	public static bool _singleMode;

	void Awake()
	{
		_singleMode = singleMode;
	}

	void Start ()
	{
		// 魔法の呪文
		PhotonNetwork.ConnectUsingSettings ("0.1");

		if( singleMode )
		{
		//	PhotonNetwork.offlineMode = true;
			playerNumNeeded = 1;
		}
	}

	//  ランダムでルームを選び入る
	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	//  JoinRandomRoom()が失敗した(false)時に呼ばれる
	void OnPhotonRandomJoinFailed(){
		//  部屋に入れなかったので自分で作る

		RoomOptions options = new RoomOptions();
		options.MaxPlayers = (byte)playerNumNeeded;
		TypedLobby lobby = new TypedLobby();

		PhotonNetwork.CreateRoom (roomName, options, lobby);
	}

	//  ルームに入れた時に呼ばれる（自分の作ったルームでも）
	void OnJoinedRoom()
	{
		//  ルームに入っている全員の画面にPlayerを生成する
		GameObject player = PhotonNetwork.Instantiate("NetworkCube", this.transform.position, this.transform.rotation, 0);
		//  自分が生成したPlayerを移動可能にする
		myPlayer = player.GetComponent<PlayerMove>();
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
		while( PhotonNetwork.room.playerCount < playerNumNeeded )
		{
			Debug.Log("プレイヤーの参加を待っています... 現在のプレイヤー数 : " + PhotonNetwork.room.playerCount.ToString() + "/" + playerNumNeeded.ToString() );
			yield return new WaitForSeconds(3f);
		}

		Debug.Log("プレイヤーが集まりました");

		// プレイヤーのポジションをセット
		SetPlayerTransform();

		myPlayer.initizlized = true;

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

		myPlayer.transform.parent = null;
	}
}

