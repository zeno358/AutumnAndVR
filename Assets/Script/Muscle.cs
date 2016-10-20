using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 上昇する筋肉
/// </summary>
public class Muscle : MonoBehaviour 
{

	float height; //高度

	const float ascend_value = 1f; // 一回の上昇で 何M 上昇するか
	const int ascend_cost = 5; // 何ポイントで１上昇するか？
	int energy = 0; // 集まったエネルギー

	public static Muscle rift;

	void Start()
	{
		height = transform.position.y;
		Debug.Log("スタート時点での高度は " + height.ToString() );
	}

	/// <summary>
	/// 上昇する
	/// </summary>
	void Ascend()
	{
		height += ascend_value;
		transform.DOMoveY( height, 0.5f);

		Debug.Log(height.ToString() + "まで上昇");
	}

	/// <summary>
	/// エネルギーを加算
	/// </summary>
	public void AddEnergy(int val)
	{
		energy += val ;
		if( energy >= ascend_cost ){
			Ascend();
			energy -= ascend_cost;
		}
	}

	/// <summary>
	/// うめく
	/// </summary>
	private void Roar()
	{
		// うめき声を再生
	}
}
