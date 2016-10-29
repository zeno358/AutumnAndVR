using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : Photon.MonoBehaviour {

	public float speed = 10.0f;

	public enum MatColor{
		R,
		B,
	}

	/// <summary>
	/// 初期化されたか？
	/// </summary>
	public bool initizlized = false;

	/// <summary>
	/// プレイヤー１の位置 
	/// </summary>
	[SerializeField]
	Vector3 playerPos1;

	/// <summary>
	/// プレイヤー１の回転
	/// </summary>
	[SerializeField]
	Vector3 playerRot1;

	/// <summary>
	/// プレイヤー２の位置
	/// </summary
	[SerializeField]
	Vector3 playerPos2;

	/// <summary>
	/// プレイヤー２の回転
	/// </summary>
	[SerializeField]
	Vector3 playerRot2;

	/// <summary>
	/// 手
	/// </summary>
	[SerializeField]
	Transform hand;

	/// <summary>
	/// カゴ
	/// </summary>
	[SerializeField]
	GameObject bag;

	/// <summary>
	/// プレイヤーID
	/// </summary>
	public int id = -1;

	static List<PlayerMove> players;

		
	// Use this for initialization
	void Start () {
		if( players == null )
		{
			players = new List<PlayerMove>();
		}
		players.Add(this);

		if( RoomMaking._singleMode )
		{
			SetBagPositionForSinglePlayer(transform);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Debug.Log("自分か？ = " + photonView.isMine );

		if( !photonView.isMine || !initizlized )
		{
			return;
		}

		var moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		var moveZ = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

		bool handControl = Input.GetKey(KeyCode.Space);

		if( !handControl ){ hand.transform.localPosition = Vector3.zero; }

		Transform target = handControl ? hand : transform;
		target.Translate(moveX, 0, moveZ);

		if( bag != null )
		{
			UpdateBagPosition();	
		}

	}


	public void SetMaterial( MatColor color )
	{
		var mat = GetComponent<MeshRenderer>().material;

		switch( color )
		{
		case MatColor.R:
			mat.color = Color.red;
			break;
		case MatColor.B:
			mat.color = Color.blue;
			break;
		}
	}

	/// <summary>
	/// カゴを有効にする
	/// </summary>
	public void EnalbeBag(bool key=true)
	{
		bag.SetActive(key);
	}
		
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonSerializeView");
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			// Network player, receive data
			this.transform.position = (Vector3) stream.ReceiveNext();
			this.transform.rotation = (Quaternion) stream.ReceiveNext();
		}
	}

	/// <summary>
	/// カゴの位置を更新する
	/// ２人プレイの時は２人の間のポジション
	/// １人プレイの時は何もしない
	/// </summary>
	private void UpdateBagPosition()
	{
		if( RoomMaking._singleMode )
		{
			return;
		}

		Vector3 bagPos = Vector3.zero;

		for( int i=0 ; i<players.Count ; i++)
		{
			bagPos += players[i].hand.position;
		}
		bagPos /= players.Count;

		bag.transform.position = bagPos;
	}

	/// <summary>
	/// シングルプレイヤー用にカゴの位置をセット
	/// </summary>
	private void SetBagPositionForSinglePlayer(Transform transform )
	{
		bag.transform.SetParent( transform );
		bag.transform.localPosition = Vector3.forward * 2.5f;
	}
}