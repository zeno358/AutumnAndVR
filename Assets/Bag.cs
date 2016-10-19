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
	/// カゴを持っているプレイヤー
	/// </summary>
	public Crew master;

	void OnTriggerEnter(Collider col)
	{
		if( col.tag.Equals("chasenut") )
		{
			Debug.Log("栗をキャッチ");
			if( master != null )
			{
				master.AddCount();
			}
		}
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
