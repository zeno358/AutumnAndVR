using UnityEngine;
using System.Collections;

/// <summary>
/// 位置の変化を取得して値を計測するクラス
/// </summary>
public class Pedometer : MonoBehaviour {

	int value;

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

	// Use this for initialization
	void Start () {
		value = 0;
		curveOriginPos = PrevPosY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		CheckPosAndAddValue ();
	}

	void CheckPosAndAddValue()
	{
		float posY = transform.position.y;
		float offset = PrevPosY - posY;

		// 折り返しを検出
		if( offset > 0 != PrevPosY > 0 ){
			if (Mathf.Abs (curveOriginPos - posY) > 0.02f) {
				AddValue ();
			}

			// 新しい折り返し基準値を設定
			curveOriginPos = posY;
		}

		PrevPosY = posY;
		prevOffset = offset;
	}

	void AddValue()
	{
		value++;
		Debug.Log ( gameObject.name + " : AddValue( " + value.ToString () + " ) " );
	}
}
