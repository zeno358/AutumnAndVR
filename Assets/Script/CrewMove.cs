using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMove : Photon.MonoBehaviour {

	public float speed = 10.0f;

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

	/// <summary>
	/// ペダルを踏んだ回数累計
	/// </summary>
	int totalCount{ set; get;}

	/// <summary>
	/// 現在残っているカウント
	/// </summary>
	int restCount{set; get;}




	// Use this for initialization
	void Start () {
		if( AutumnVRGameManager.players == null )
		{
			AutumnVRGameManager.players = new List<CrewMove>();
		}
		AutumnVRGameManager.players.Add(this);

		if( CrewRoomMaking._singleMode )
		{
			SetBagPositionForSinglePlayer(hand);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Debug.Log("自分か？ = " + photonView.isMine );

		if(!AutumnVRGameManager.running)
		{
			return;
		}

		if( !CrewRoomMaking._singleMode && (!photonView.isMine || !initialized) )
		{
			return;
		}

		TemporalMovementForNotVR();

		if( bag != null )
		{
			UpdateBagPosition();	
		}

	}

	/// <summary>
	/// 非VR環境でテスト用の操作
	/// </summary>
	void TemporalMovementForNotVR()
	{
		var moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		var moveZ = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

		hand.Translate(moveX, 0, moveZ);
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
		if( CrewRoomMaking._singleMode )
		{
			return;
		}

		Vector3 bagPos = Vector3.zero;

		for( int i=0 ; i<AutumnVRGameManager.players.Count ; i++)
		{
			bagPos += AutumnVRGameManager.players[i].hand.position;
		}
		bagPos /= AutumnVRGameManager.players.Count;

		bag.transform.position = bagPos;
	}

	/// <summary>
	/// カウントを追加
	/// </summary>
	public void AddCount() {
		totalCount++;
		restCount++;
		Debug.Log(gameObject.name + "のカウント[ 現在 " + restCount.ToString()  + " 累計 " + totalCount.ToString()  + " ]");
	}


	/// <summary>
	/// 所持カウントを渡し、所持分は0にする
	/// </summary>
	/// <returns>The count.</returns>
	public int PassCount()
	{
		int num = restCount;
		restCount = 0;
		return num;
	}

	/// <summary>
	/// シングルプレイヤー用にカゴの位置をセット
	/// </summary>
	private void SetBagPositionForSinglePlayer(Transform transform )
	{
		bag.transform.SetParent( transform );
		bag.transform.localPosition = Vector3.forward * 1.5f;
	}
}