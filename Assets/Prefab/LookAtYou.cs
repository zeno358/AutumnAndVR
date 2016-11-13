using UnityEngine;
using System.Collections;

public class LookAtYou : MonoBehaviour {
	
	bool findTarget = false;

	string targetName = "Bag";

	// Use this for initialization
	void Start () {
		StartCoroutine(FindTarget());
	}


	IEnumerator FindTarget()
	{
		yield return null;
	}
	// Update is called once per frame
	void Update () {
		if(!findTarget)
		{
			return;
		}


	}
}
