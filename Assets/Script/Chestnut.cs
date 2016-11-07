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
	float fallingSpeed = 0.15f;

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
	/// 収穫済みか？
	/// </summary>
	public bool harvested{
		get;
		private set;
	}

	/// <summary>
	/// モデル
	/// </summary>
	public GameObject model;

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

	/// <summary>
	/// 残り寿命を更新
	/// </summary>
	void UpdateLifeTimer()
	{
		timer += Time.deltaTime;
		if( timer >= lifeTime )
		{
			harvested = true;
			Destroy(gameObject);
			//PhotonView.Destroy( GameObject );
		}
	}

	/// <summary>
	/// 収穫される
	/// </summary>
	public void Harvest()
	{
		if( AutumnVRGameManager.over )
		{
			return;
		}

		harvested = true;
		model.SetActive(false);
	}
}
