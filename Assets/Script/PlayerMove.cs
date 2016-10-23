using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed = 10.0f;

	public enum MatColor{
		R,
		B,
	}

	/// <summary>
	/// プレイヤーID
	/// </summary>
	public int id = -1;

	PhotonView myPhotonView;
		
	// Use this for initialization
	void Awake () {
		myPhotonView = GetComponent<PhotonView>();
	}

	public PhotonView photonView
	{
		get{
			return myPhotonView;
		}
	}

	// Update is called once per frame
	void Update () {
		var moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
		var moveZ = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

		transform.Translate(moveX, 0, moveZ);

	//	this.GetComponent<Rigidbody>().velocity = new Vector3( * speed, 0,  * speed);
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
}