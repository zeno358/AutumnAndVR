using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MeasureGenerator : MonoBehaviour {

	/// <summary>
	/// 原点
	/// </summary>
	private Vector3 rootPos;

	/// <summary>
	/// メジャーを設置する間隔
	/// </summary>
	public int interval;

	/// <summary>
	/// 最も高い高度
	/// </summary>
	public int top;

	public GameObject measure; 

	// Use this for initialization
	void Start () 
	{
		rootPos = transform.position;
		GenerateMeasure();
	}

	/// <summary>
	/// 目盛りを生成する
	/// </summary>
	private void GenerateMeasure()
	{
		// 設置数を定義
		int count = top / interval + 2;

		for(int i=0 ; i < count ; i++)
		{
			var g = Instantiate(measure);
			g.transform.SetParent(transform);
			float h = (i == count-1) ? top : interval * i;
			g.transform.localPosition = Vector3.up * h;
			g.transform.localRotation = Quaternion.Euler(0,-90f,0);

			var m = g.GetComponent<TextMesh>();
			m.text = g.name = ((int)h).ToString() + "M";
		}
	}
}
