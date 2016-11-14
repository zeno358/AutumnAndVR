using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScene : MonoBehaviour {

	SteamVR_TrackedObject trackedObject;

	void Start()
	{
		var m = GameObject.Find("Muscle").GetComponent<Muscle>();
		if(m != null)
		{
			m.SetToOrigin();
		}

		if( UnityEditor.PlayerSettings.virtualRealitySupported )
		{
			StartCoroutine (TryGetVrController ());
		}
	}

	IEnumerator TryGetVrController(){
		GameObject g;
		do {
			g = GameObject.Find("Controller (right)");
			yield return new WaitForSeconds(5);
		} while(g == null);

		trackedObject = g.GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			GoToMainGame ();
		}

		if (trackedObject == null) {
			return;
		}

        var device = SteamVR_Controller.Input((int) trackedObject.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			GoToMainGame ();
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			GoToMainGame ();
        }
	}

	private void GoToMainGame(){
		var g = GameObject.Find("MatchMaker").GetComponent<AutumnVRGameManager>();
		if(g != null)
		{
			g.ShowGameStartExpression();
		}

		SceneManager.UnloadScene("Title");
	}
}
