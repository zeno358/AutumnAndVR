using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MeasureGenerator : MonoBehaviour {

	/// <summary>
	/// 原点
	/// </summary>
	private Vector3 rootPos;

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
		int count = GameManager.instance._goalHeight / GameManager.instance.measureExpInterval;

		count += GameManager.instance._goalHeight / GameManager.instance.measureExpInterval == 0 ?  1 : 2;

		for(int i=0 ; i < count ; i++)
		{
			bool top = i == count - 1;

			var g = Instantiate(measure);
			g.transform.SetParent(transform);
			float h = (top) ? GameManager.instance._goalHeight : GameManager.instance.measureExpInterval * i;
			g.transform.localPosition = Vector3.up * h;
			g.transform.localRotation = Quaternion.Euler(0,-90f,0);

			var m = g.GetComponent<Measure>();
			string str = ((int)h).ToString() + "M";
			g.name = str;
			m.SetText (str); 
			m.EnableFlag (top);
		}
	}
}
