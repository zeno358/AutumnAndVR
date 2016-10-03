using UnityEngine;
using System.Collections;

/// <summary>
/// リフトの乗組員
/// </summary>
public class Crew : MonoBehaviour {

	[SerializeField]
	KeyCode myKeyCode;
	/*
	/// <summary>
	/// このプレイヤーに割り当てわれたキーコードを返す
	/// </summary>
	public KeyCode GetAssignedKeyCode(){ return myKeyCode;}
	*/

	/// <summary>
	/// ペダルを踏んだ回数累計
	/// </summary>
	public int totalCount{ private set; get;}

	/// <summary>
	/// 現在残っているカウント
	/// </summary>
	/// <value>The rest count.</value>
	public int restCount{private set; get;}

	void Start(){
		GameManager.crews.Add( this );
	}

	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown( myKeyCode ) ){
			totalCount++;
			restCount++;
			Debug.Log(gameObject.name + "のカウント[ 現在 " + restCount.ToString()  + " 累計 " + totalCount.ToString()  + " ]");
		}
	}
	public void AddCount() {
		totalCount++;
		restCount++;
		Debug.Log(gameObject.name + "のカウント[ 現在 " + restCount.ToString()  + " 累計 " + totalCount.ToString()  + " ]");
	}


	/// <summary>
	/// 所持カウントを渡し、所持分は0にする
	/// </summary>
	/// <returns>The count.</returns>
	public int PassCount()
	{
		int num = restCount;
		restCount = 0;
		return num;
	}
}
