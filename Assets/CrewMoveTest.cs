using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMoveTest : Photon.MonoBehaviour {

	float speed = 2f;

	[SerializeField]
	Transform hand;

	[SerializeField]
	AudioListener listener;

	[SerializeField]
	List<GameObject> notNeededForOther;

	[SerializeField]
	List<GameObject> notNeededForMe;

	int count;

	// Use this for initialization
	void Start () {
		if( !photonView.isMine )
		{
			notNeededForOther.ForEach( g => g.SetActive(false) );

			listener.enabled = false;
		}
		else
		{
			notNeededForMe.ForEach( g => g.SetActive(false) );
		}

		TwoPlayerTest.crews.Add(this);
	}

	void OnPhotonSerializeView(PhotonStream s, PhotonMessageInfo i)
	{
		if(s.isWriting)
		{
		//	s.SendNext(hand.transform.position);
		}
		else
		{
		//	hand.transform.position = (Vector3)s.ReceiveNext();
		}
	}

	// Update is called once per frame
	void Update () {
		if( !photonView.isMine )
		{
			return;
		}

		GetKeyBoardInput();

	}

	void GetKeyBoardInput()
	{

		bool handControl = Input.GetKey(KeyCode.Space);

		var moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		var moveZ = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

		Transform target = handControl ? hand : transform;

		target.Translate(moveX, 0, moveZ);

		if( Input.GetKeyDown(KeyCode.U) )
		{
			PhotonNetwork.RPC(photonView, "AddCount", PhotonTargets.All, false);
		}
	}

	[PunRPC]
	void AddCount()
	{
		count++;
		Debug.Log( "クライアント" + PhotonNetwork.player.ID.ToString() + "上の オーナーID" + photonView.ownerId.ToString() + "のカウント=" + count.ToString());

		int sum = 0;
		TwoPlayerTest.crews.ForEach( c => sum += c.count );

		Debug.Log( "全プレイヤーの合計カウント = " + sum.ToString());

	}


	public Vector3 handPos
	{
		get{
			return hand.position;
		}
	}
}
