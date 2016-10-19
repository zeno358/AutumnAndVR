using UnityEngine;
using System.Collections;

/// <summary>
/// イガグリ
/// </summary>
public class Chasenut : MonoBehaviour {

	/// <summary>
	/// 落下スピード
	/// </summary>
	float fallingSpeed = 1f;

	/// <summary>
	/// 回転スピード
	/// </summary>
	float rotationSpeed = 0.1f;

	/// <summary>
	/// 命の長さ秒
	/// </summary>
	float lifeTime = 5f;
	float timer = 0;

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
			Destroy(gameObject);
			//PhotonView.Destroy( GameObject );
		}
	}
}
