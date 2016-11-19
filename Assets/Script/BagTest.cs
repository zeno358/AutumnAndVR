using UnityEngine;
using System.Collections;

public class BagTest : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!photonView.isMine)
		{
			Destroy(gameObject);
		}
	}

}
