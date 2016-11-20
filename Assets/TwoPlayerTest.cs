using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 目標：
/// 二人のプレイヤーをそれぞれ操作 <Clear!
/// お互いの姿が正しく同期される <Clear!
/// 箱をプレイヤーの腕の中点に配置する <Clear
/// イガグリ生成機を１つセット
/// </summary>

public class TwoPlayerTest : Photon.MonoBehaviour 
{
	/// <summary>
	/// ルーム名
	/// </summary>
	string roomName = "autumnvr";

	[SerializeField]
	Transform pos1;

	[SerializeField]
	Transform pos2;

	[SerializeField]
	Transform posGenerator;

	CrewMoveTest myCrew;

	public static List<CrewMoveTest> crews = new List<CrewMoveTest>();

	GameObject bag;

	GameObject chestnutGenerator;

	/// <summary>
	/// ゲームを開始するのに必要なプレイヤー数
	/// </summary>
	public static int playerNumNeeded = 2;

	PhotonObjectSetter crewSetter;

	void Start ()
	{

		PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			if (myCrew == null )
			{
				return;
			}
			Debug.Log("before : " + PhotonNetwork.room.playerCount.ToString());

			PhotonNetwork.Destroy(myCrew.gameObject);

			Debug.Log("after : " + PhotonNetwork.room.playerCount.ToString());

			if( PhotonNetwork.room.playerCount < 2 && bag != null)
			{
				PhotonNetwork.Destroy(bag);
			}
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			SetCrew();
		}

		if(  bag != null )
		{
			UpdateBagPos();
		}

		if( PhotonNetwork.room != null )
		{
		//	Debug.Log("update ルームのプレイヤー数=" +  PhotonNetwork.room.playerCount.ToString() + " プレイヤーリスト長=" + crews.Count.ToString());
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
		SetCrew();
	}

	void SetCrew()
	{
		myCrew = PhotonNetwork.Instantiate("CrewMoveTest", Vector3.zero, Quaternion.identity, 0).GetComponent<CrewMoveTest>();

		int playerId = PhotonNetwork.player.ID;
		Debug.LogError("playerID = " + playerId.ToString());

		Transform t = playerId == 1 ? pos1 : pos2;

		myCrew.transform.SetParent(t);
		myCrew.transform.localPosition = Vector3.zero;
		myCrew.transform.localRotation = Quaternion.identity;
		myCrew.transform.parent = null;

		bag = PhotonNetwork.Instantiate("BagTest", Vector3.zero, Quaternion.identity, 0);

		if(PhotonNetwork.isMasterClient )
		{
			chestnutGenerator = PhotonNetwork.Instantiate("ChestnutGenerator", posGenerator.position, Quaternion.identity, 0);
		}
	}

	void UpdateBagPos()
	{
		Vector3 bagPos = Vector3.zero;

		for( int i=0 ; i< crews.Count ; i++)
		{
			if( crews[i] == null )
			{
				Debug.Log("プレイヤーがnull");
				continue;
			}

			bagPos += crews[i].handPos;
		}
		bagPos /= crews.Count;

		bag.transform.position = bagPos;
	}
}

