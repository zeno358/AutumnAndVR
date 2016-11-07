using UnityEngine;
using System.Collections;

/// <summary>
/// カゴ
/// </summary>
public class Bag : MonoBehaviour {

	/// <summary>
	/// カゴのモデルリスト
	/// </summary>
	public GameObject[] models;

	/// <summary>
	/// 栗を取ったとみなす範囲
	/// </summary>
	float catchRange = 1f;

	/// <summary>
	/// モデル差し替えの閾値
	/// </summary>
	int[] modelChangeThreshold = new int[3]{10, 20, 40};

	int catchCount = 0;

	void Update()
	{
		CheckChestnut();
	}

	void CheckChestnut()
	{
		if(Chestnut.cList == null)
		{
			return;
		}

		float dist; 
		for(int i=0 ; i < Chestnut.cList.Count ; i++)
		{
			Chestnut c = Chestnut.cList[i];

			// すでに取られた栗はスキップ
			if( c.harvested ) continue;

			// 栗との距離
			dist =  Mathf.Abs( (transform.position - c.transform.position).magnitude );

			if( dist <= catchRange )
			{
				Debug.Log("栗をキャッチ");
				c.Harvest();

				catchCount++;

				// モデルの更新
				SetModel();
			}
		}
	}

	/// <summary>
	/// カゴのモデルを更新する
	/// </summary>
	public void SetModel()
	{
		for( int i=0 ; i < modelChangeThreshold.Length ; i++ )
		{
			models[i].SetActive( catchCount >= modelChangeThreshold[i] );
		}
	}
}
