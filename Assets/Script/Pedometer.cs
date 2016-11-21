using UnityEngine;
using System.Collections;

/// <summary>
/// 位置の変化を取得して値を計測するクラス
/// </summary>
public class Pedometer : MonoBehaviour
{
	/// <summary>
	/// １フレーム前のポジション
	/// </summary>
	float PrevPosY;

	/// <summary>
	/// １フレーム前の差分
	/// </summary>
	float prevOffset;

	/// <summary>
	/// 折り返しを検出するための基準値
	/// </summary>
	float curveOriginPos;

	public CrewMoveTest master;

	// Use this for initialization
	void Start () {
		curveOriginPos = PrevPosY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!GameManagerTest.running) {
			return;
		}
		CheckPosAndAddValue ();

		GetTemporaryInput();
	}

	void CheckPosAndAddValue()
	{
		float posY = transform.localPosition.y;
		float offset = PrevPosY - posY;

		// 折り返しを検出
		if( offset > 0 != prevOffset > 0 ){
			if (Mathf.Abs (curveOriginPos - posY) > 0.02f) {
				AddValue ();
			}

			// 新しい折り返し基準値を設定
			curveOriginPos = posY;
		}

		PrevPosY = posY;
		prevOffset = offset;
	}

	void GetTemporaryInput()
	{
		if( Input.GetKey(KeyCode.U) )
		{
			AddValue();
		}
	}

	void AddValue()
	{
		if (master != null) {
			//master.AddStompCount ();
			PhotonNetwork.RPC(master.photonView, "AddStompCount", PhotonTargets.All, false);
		}
	}
}
