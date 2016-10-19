using UnityEngine;
using System.Collections;

/// <summary>
/// リフトの乗組員
/// </summary>
public class Crew : Photon.MonoBehaviour {

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
	int totalCount{ set; get;}

	/// <summary>
	/// 現在残っているカウント
	/// </summary>
	int restCount{set; get;}

	/// <summary>
	/// 栗を拾った数
	/// </summary>
	int catchCount{set; get;}

	/// <summary>
	/// カゴ
	/// </summary>
	public GameObject bag;

	/// <summary>
	/// 手の位置
	/// </summary>
	public Transform hand;

	/// <summary>
	/// もう一人のプレイヤー
	/// </summary>
	private Crew partner = null;

	void Start(){
		GameManager.crews.Add( this );
	}

	// Update is called once per frame
	void Update () {
		CatchInput();
		UpdateBagPosition();
	}

	/// <summary>
	/// カウントを追加
	/// </summary>
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

	void CatchInput()
	{
		if( Input.GetKeyDown( myKeyCode ) ){
			totalCount++;
			restCount++;
			Debug.Log(gameObject.name + "のカウント[ 現在 " + restCount.ToString()  + " 累計 " + totalCount.ToString()  + " ]");
		}
	}

	/// <summary>
	/// 栗をキャッチするカゴの位置を更新する
	/// </summary>
	void UpdateBagPosition()
	{
		if( hand == null )
		{
			Debug.LogError("手がない");
			return;
		}

		bool withPartner = partner != null;

		if(withPartner)
		{
			bag.transform.LookAt( partner.hand.position );
		}
	}

}
