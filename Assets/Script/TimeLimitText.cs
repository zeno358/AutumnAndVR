using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 残り制限直を表示する
/// </summary>
public class TimeLimitText : MonoBehaviour 
{
	TextMesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!AutumnVRGameManager.running) {
			mesh.text = "";
			return;
		}

		//残り時間計算
		float diff = AutumnVRGameManager.timeLimitSec - AutumnVRGameManager.gameTimer;
		int val = (int)Mathf.Floor(diff);

		// 文字色更新
		mesh.color = val < 30 ? Color.red : Color.white;

		if(val <= 0)
		{
			val = 0;
		}

		int min = val / 60;
		int sec = val % 60;
		string str = string.Format("{0}:{1}", min.ToString("00"), sec.ToString("00"));
		mesh.text = str;
	}
}
