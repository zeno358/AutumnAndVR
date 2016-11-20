using UnityEngine;
using System.Collections;

public class LookAtYou : MonoBehaviour {
	
	bool findTarget = false;

	GameObject bag;

	// Use this for initialization
	void Start () {
		StartCoroutine(FindTarget());
	}

	IEnumerator FindTarget()
	{
		do{
			bag = GameObject.Find ("BagTest(Clone)");

			yield return new WaitForSeconds(1);
		}while(bag == null);

		findTarget = true;	
	}

	// Update is called once per frame
	void Update () {
		if(!findTarget || bag == null)
		{
			return;
		}

		transform.LookAt (bag.transform);
	}
}