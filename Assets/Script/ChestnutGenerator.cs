using UnityEngine;
using System.Collections;

/// <summary>
/// イガグリ生成装置
/// </summary>
public class ChestnutGenerator : MonoBehaviour {

	/// <summary>
	/// 生成頻度
	/// </summary>
	public float interval = 1f;
	float timer = 0;

	/// <summary>
	/// 生成位置の誤差
	/// </summary>
	public float diffRange = 3f;

	/// <summary>
	/// イガグリ
	/// </summary>
	public GameObject Chestnut;
	
	// Update is called once per frame
	void Update () {
		if( CheckTimer() )
		{
			Generate();
		}
	}

	/// <summary>
	/// 一定間隔でtrueを返す
	/// </summary>
	bool CheckTimer()
	{
		timer += Time.deltaTime;
		if( timer >= interval )
		{
			timer = 0;
			return true;
		}
		return false;
	}

	/// <summary>
	/// イガグリを生成
	/// </summary>
	void Generate()
	{
		Debug.Log("栗を生成");
		var g = Instantiate(Chestnut);

		//　生成位置を決定
		Vector3 pos = new Vector3(transform.position.x + Random.Range( -diffRange, diffRange ), transform.position.y, transform.position.z + Random.Range( -diffRange, diffRange ));
		g.transform.position = pos;
	}
}
