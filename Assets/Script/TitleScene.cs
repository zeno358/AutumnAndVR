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
			var g = GameObject.Find("MatchMaker").GetComponent<AutumnVRGameManager>();
			if(g != null)
			{
				StartCoroutine( g.ShowGameStartExpression() );
			}

			SceneManager.UnloadScene("Title");
		}
	}
}
