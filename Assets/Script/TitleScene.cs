using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScene : Photon.MonoBehaviour {

	SteamVR_TrackedObject trackedObject;

	bool starting;

	public TextMesh statusText;

	void Start()
	{
		starting = false;

		var m = GameObject.Find("Muscle").GetComponent<MuscleTest>();
		if(m != null)
		{
			m.SetToOrigin();
		}
		#if UNITY_EDITOR 
		if( UnityEditor.PlayerSettings.virtualRealitySupported )
		#endif
			StartCoroutine (TryGetVrController ());
		
	}

	IEnumerator TryGetVrController(){
		GameObject g;

		float tryInterval = 5f;

		do {
			g = GameObject.Find("Controller (right)");

			if(g == null)
			{
				Debug.LogWarning("VRコントローラの取得に失敗。" + tryInterval.ToString() + "秒後に再トライします");
			}
			else
			{
				Debug.LogWarning("VRコントローラの取得に成功");
			}

			yield return new WaitForSeconds(tryInterval);
		} while(true);

		trackedObject = g.GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {

		UpdateStatusText();

		if(Input.anyKeyDown)
		{
			StartCoroutine( GoToMainGame () );
		}

		if (trackedObject == null) {
			return;
		}

        var device = SteamVR_Controller.Input((int) trackedObject.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			StartCoroutine( GoToMainGame () );
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			StartCoroutine( GoToMainGame () );
        }
	}

	void UpdateStatusText()
	{
		if( PhotonNetwork.room == null )
		{
			statusText.text = "ルームが作成されていません";
		}
		else{
			statusText.text = "プレイヤーの参加を待機しています...。 プレイヤー数" + PhotonNetwork.room.playerCount.ToString() + "/" + TwoPlayerTest.playerNumNeeded.ToString();
		}
	}

	private IEnumerator GoToMainGame()
	{
		if(starting || PhotonNetwork.room == null)
		{
			yield break;
		}

		starting = true;

		while( PhotonNetwork.room.playerCount < TwoPlayerTest.playerNumNeeded )
		{
			Debug.LogError("プレイヤー参加待機中");
			yield return null;
		}

		//PhotonObjectSetter.instance.Init();

		var g = GameObject.Find("GameManager").GetComponent<GameManagerTest>();
		if(g != null)
		{
			g.ShowGameStartExpression();
		}

		SceneManager.UnloadScene("Title");
	}
}
