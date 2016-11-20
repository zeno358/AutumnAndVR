using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMoveTest : Photon.MonoBehaviour {

	float speed = 2f;

	public SteamVR_TrackedObject rightHand;
	public Transform eye;

	public AudioListener listener;

	public bool ready { get; private set; }

	/// <summary>
	/// 自分自身のときに不要
	/// </summary>
	public List<GameObject> notNeededObjForMe;

	/// <summary>
	/// 相手のときに不要
	/// </summary>
	public List<GameObject> notNeededObjForOther;
	public SteamVR_PlayArea pa;
	public SteamVR_ControllerManager cm;
	public SteamVR_Camera sc;
	public SteamVR_Ears se;
	public List<SteamVR_TrackedObject> to;
	public List<Camera> c;

	MuscleTest myMuscle;

	private Vector3 offsetHeightFromMuscle;

	int count;

	// Use this for initialization
	void Start () {
		if( !photonView.isMine )
		{
			notNeededObjForOther.ForEach( g => g.SetActive(false) );
			to.ForEach( g => g.enabled = false );
			c.ForEach( g => g.enabled = false );
			pa.enabled = false;
			cm.enabled = false;
			sc.enabled = false;
			se.enabled = false;

			listener.enabled = false;
		}
		else
		{
			notNeededObjForMe.ForEach( g => g.SetActive(false) );
		}

		TwoPlayerTest.crews.Add(this);

		myMuscle = GameObject.Find("Muscle").GetComponent<MuscleTest>();

		int photonViewId = photonView.ownerId;
		Debug.LogError("playerID = " + photonViewId.ToString());

		Transform t = photonViewId == 1 ? myMuscle.pos1 : myMuscle.pos2;

		transform.SetParent(t);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

	//	photonView.RPC ("SetReady", PhotonTargets.All, false);
	}

	// Update is called once per frame
	void Update () {

		if( !photonView.isMine )
		{
			return;
		}

		GetInput();
	}
		
	void GetInput()
	{
		var device = SteamVR_Controller.Input((int) rightHand.index);

		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
	//		photonView.RPC ("SetReady", PhotonTargets.All, true);
		}
		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
	//		photonView.RPC ("SetReady", PhotonTargets.All, true);
		}

		////////////////
		/// ↓はデバッグ用
		//////////////// 

		bool handControl = Input.GetKey(KeyCode.Space);

		var moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		var moveZ = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

		rightHand.transform.Translate(moveX, 0, moveZ);

		if( Input.GetKeyDown(KeyCode.U) )
		{
			Debug.Log("Uが押された プレイヤーID = " + PhotonNetwork.player.ID.ToString());

			PhotonNetwork.RPC(photonView, "AddCount", PhotonTargets.All, false);
		}
	}

	[PunRPC]
	public void AddCount()
	{
		if( !GameManagerTest.running )
		{
			Debug.Log("ゲームは終了済み。カウント追加は無効");
		}

		count++;
		Debug.Log( "クライアント" + PhotonNetwork.player.ID.ToString() + "上の オーナーID" + photonView.ownerId.ToString() + "のカウント=" + count.ToString());


		if( myMuscle != null ){
			myMuscle.AddEnergy(1, this);
		}else{
			Debug.Log( "myMuscleがnull" );
		}

		int sum = 0;
		TwoPlayerTest.crews.ForEach( c => sum += c.count );

		Debug.Log( "全プレイヤーの合計カウント = " + sum.ToString());

	}

//	[PunRPC]
//	void SetReady(bool value)
//	{
//		Debug.Log ("PhotonViewID[ " + photonView.viewID.ToString () + " ]のreadyを" + value.ToString ());
//		ready = value;
//	}

	public Vector3 handPos
	{
		get{
			return rightHand.transform.position;
		}
	}
}
