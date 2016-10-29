using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMove : Photon.MonoBehaviour {

	public float speed = 10.0f;

	public enum MatColor{
		R,
		B,
	}

	/// <summary>
	/// 初期化されたか？
	/// </summary>
	private bool _initialized;
	public bool initialized{
		get{
			return _initialized;
		}
		set{
			_initialized = value;
		}
	}

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

	static List<CrewMove> players;


	// Use this for initialization
	void Start () {
		if( players == null )
		{
			players = new List<CrewMove>();
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

		if( !photonView.isMine || !initialized )
		{
			return;
		}

		if( bag != null )
		{
			UpdateBagPosition();	
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