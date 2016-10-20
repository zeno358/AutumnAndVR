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

	[SerializeField]
	Crew myCrew;

	[SerializeField]
	Crew Partner;
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

        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
    }

    public void OnPhotonRandomJoinFailed()
    {
		Debug.LogError(gameObject.name + "OnPhotonRandomJoinFailed");

		PhotonNetwork.CreateRoom(roomName);

    }

    public override void OnJoinedRoom()
    {
		Debug.Log(gameObject.name + "OnJoinedRoom");

		// 先に部屋に入った（または部屋を作った）プレイヤーはポジション１に、後から部屋二入ったプレイヤーはポジション２に配置したい
		if( PhotonNetwork.room.playerCount < PhotonNetwork.room.maxPlayers ){
			Debug.Log("ポジション１にセット あなたはプレイヤー１");
			myPhotonView = playerPos1.GetComponent<PhotonView>();
			myCrew.transform.SetParent(playerPos1);
		}else{
			Debug.Log("ポジション２にセット あなたはプレイヤー２");
			myPhotonView = playerPos2.GetComponent<PhotonView>();
			myCrew.transform.SetParent(playerPos2);
		}
		myCrew.transform.localPosition = Vector3.zero;
		myCrew.transform.localRotation = Quaternion.identity;
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
