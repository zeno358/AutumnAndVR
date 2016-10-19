using UnityEngine;

public class CrewMatchmaker : Photon.PunBehaviour
{
    private PhotonView myPhotonView;

	string roomName = "autumnvr";

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
    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {

		Debug.LogError(gameObject.name + "OnJoinedLobby");

		PhotonNetwork.JoinRoom( roomName );
    }

    public override void OnConnectedToMaster()
    {
		Debug.LogError(gameObject.name + "OnConnectedToMaster");

		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 2;
		TypedLobby lobby = new TypedLobby();


		PhotonNetwork.JoinOrCreateRoom(roomName,  options, lobby);

		/*
		if( !PhotonNetwork.JoinRoom( roomName ) ){
			Debug.Log("To join room failed");
			PhotonNetwork.CreateRoom( roomName );
		}
	*/
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
    //    PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
		Debug.LogError(gameObject.name + "OnPhotonRandomJoinFailed");

		PhotonNetwork.CreateRoom(roomName);

    }

    public override void OnJoinedRoom()
    {
		Debug.LogError(gameObject.name + "OnJoinedRoom");

	//	PhotonNetwork.room.name = roomName;

    //    GameObject monster = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
    //    monster.GetComponent<myThirdPersonController>().isControllable = true;
    //    myPhotonView = monster.GetComponent<PhotonView>();

		// 先に部屋に入った（または部屋を作った）プレイヤーはポジション１に、後から部屋二入ったプレイヤーはポジション２に配置したい
		if( PhotonNetwork.room.playerCount < PhotonNetwork.room.maxPlayers ){
			Debug.Log("ポジション１にセット あなたはプレイヤー１");
			myPhotonView = playerPos1.GetComponent<PhotonView>();
		}else{
			Debug.Log("ポジション２にセット あなたはプレイヤー２");
			myPhotonView = playerPos2.GetComponent<PhotonView>();
		}

		// myPhotonView = playerPos1.GetComponent<PhotonView>();
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.inRoom)
        {
            bool shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;

            if (shoutMarco && GUILayout.Button("Marco!"))
            {
                myPhotonView.RPC("Marco", PhotonTargets.All);
            }
            if (!shoutMarco && GUILayout.Button("Polo!"))
            {
                myPhotonView.RPC("Polo", PhotonTargets.All);
            }
        }
    }
}
