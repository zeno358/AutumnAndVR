using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// イガグリ
/// </summary>
public class Chestnut : MonoBehaviour {

	/// <summary>
	/// イガグリリスト
	/// </summary>
	public static List<Chestnut> cList;

	/// <summary>
	/// 落下スピード
	/// </summary>
	float fallingSpeed = 0.5f;

	/// <summary>
	/// 回転スピード
	/// </summary>
	float rotationSpeed = 0.1f;

	/// <summary>
	/// 命の長さ秒
	/// </summary>
	float lifeTime = 5f;
	float timer = 0;

	/// <summary>
	/// ゲットされた状態か？
	/// </summary>
	public bool caught = false;

	void Awake()
	{
		if( cList == null)
		{
			cList = new List<Chestnut>();
		}

		cList.Add(this);
	}

	// Update is called once per frame
	void Update () 
	{
		UpdatePosition();
		UpdateRotation();
		UpdateLifeTimer();
	}

	/// <summary>
	/// 位置更新
	/// </summary>
	void UpdatePosition()
	{
		transform.Translate( Vector3.down * fallingSpeed );
	}

	/// <summary>
	/// 回転更新
	/// </summary>
	void UpdateRotation()
	{
	}

	void UpdateLifeTimer()
	{
		timer += Time.deltaTime;
		if( timer >= lifeTime )
		{
			caught = true;
			Destroy(gameObject);
			//PhotonView.Destroy( GameObject );
		}
	}
}
