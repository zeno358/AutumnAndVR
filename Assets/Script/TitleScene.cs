using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			AutumnVRGameManager.running = true;
			SceneManager.UnloadScene("Title");
		}
	}
}
