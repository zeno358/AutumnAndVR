using UnityEngine;
using System.Collections;

public class CrewRoomMaking : Photon.MonoBehaviour 
{
	/// <summary>
	/// ルーム名
	/// </summary>
	string roomName = "autumnvr";

	/// <summary>
	/// ゲームを開始するのに必要なプレイヤー数
	/// </summary>
	public static int playerNumNeeded = 2;

	CrewSetter crewSetter;

	public static bool completeJoinedRoom = false;
		
	void Start ()
	{
		completeJoinedRoom = false;

		crewSetter = GameObject.Find("GameManager").GetComponent<CrewSetter>();

		if( AutumnVRGameManager.instance.singleMode )
		{
			// PhotonNetwork.offlineMode = true;
			crewSetter.SetupCrewForSingleMode();
		}
		else{
			// 魔法の呪文
			PhotonNetwork.ConnectUsingSettings ("0.1");
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
		completeJoinedRoom = true;

		return;

		if( crewSetter == null )
		{
			Debug.LogError("crewStterがnull");
		}
		else
		{
		//	crewSetter.SetCrew();
		}
	}
}

