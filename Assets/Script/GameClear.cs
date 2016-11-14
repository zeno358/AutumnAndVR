using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

	SteamVR_TrackedObject trackedObject;

	bool endExp = false;

	public TextMesh clearText1;
	public TextMesh clearText2;

	void Start()
	{
		if( UnityEditor.PlayerSettings.virtualRealitySupported )
		{
			StartCoroutine (TryGetVrController ());
		}

		StartCoroutine (ShowExp());
	}

	IEnumerator ShowExp()
	{
		clearText1.gameObject.SetActive (true);
		//clearText1.transform.position = 

		yield return new WaitForSeconds (3);

		clearText2.gameObject.SetActive (true);
		clearText2.text = ((int)AutumnVRGameManager.gameTimer).ToString () + "びょう";

		yield return new WaitForSeconds (3);

		endExp = true;
	}

	IEnumerator TryGetVrController(){
		do {
			trackedObject = GameObject.Find("Controller (right)"). GetComponent<SteamVR_TrackedObject>();
			yield return new WaitForSeconds(5);
		} while(trackedObject == null);
	}

	// Update is called once per frame
	void Update () {
		if (!endExp) {
			return;
		}


		if(Input.anyKeyDown)
		{
			GoToTitle ();
		}

		if (trackedObject == null) {
			return;
		}

		var device = SteamVR_Controller.Input((int) trackedObject.index);

		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			GoToTitle ();
		}
		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			GoToTitle ();
		}
	}

	private void GoToTitle()
	{
		AutumnVRGameManager.ResetParametersAndLoadTitleScene ();
	}
}
