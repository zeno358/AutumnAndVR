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

	void Update()
	{
		CheckChestnut();
	}

	void CheckChestnut()
	{
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
				AddScore();
				c.Harvest();
			}
		}
	}


	void AddScore()
	{
		Debug.Log("栗をキャッチ");
		AutumnVRGameManager.count++;
	}

	/// <summary>
	/// カゴのモデルを更新する
	/// </summary>
	public void SetModel(int visualIdx)
	{
		if( visualIdx >= models.Length )
		{
			Debug.Log("そんなものはない");
			return;
		}

		//モデルを切り替え
		for( int i=0 ; i < models.Length ; i++)
		{
			GameObject g = models[i];
			g.SetActive( i == visualIdx );
		}
	}
}
