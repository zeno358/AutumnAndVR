using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScene : MonoBehaviour {

	void Start()
	{
		var m = GameObject.Find("Muscle").GetComponent<Muscle>();
		if(m != null)
		{
			m.SetToOrigin();
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			AutumnVRGameManager.running = true;
			SceneManager.UnloadScene("Title");
		}
	}
}
