using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 上昇する筋肉
/// </summary>
public class Muscle : MonoBehaviour 
{
	float height; //高度

	const float ascend_value = 1f; // 一回の上昇で 何M 上昇するか
	const int ascend_cost = 1; // 何ポイントで１回上昇するか？
	int energy = 0; // 集まったエネルギー

	/// <summary>
	/// 栗がぶつかったとみなす範囲
	/// </summary>
	float hitRange = 0.1f;

	[SerializeField]
	AudioSource myAudio;

	[SerializeField]
	AudioClip[] se_roar;

	[SerializeField]
	AudioClip se_clear;

	[SerializeField]
	private TextMesh clearText;

	float stanTimer =0;

	enum VoicePat{
		Delight,	// 歓喜
		Painful,	// 苦痛
	}

	void Start()
	{
		height = transform.position.y;
		Debug.Log("スタート時点での高度は " + height.ToString() );
	}

	void Update()
	{
		// 栗とのあたり判定をチェック
		CheckCollisionChestnut();

		ReduceStanTimer ();
	}

	void ReduceStanTimer()
	{
		if (stanTimer >= 0) {
			stanTimer -= Time.deltaTime;
		}
	}

	/// <summary>
	/// 上昇する
	/// </summary>
	void Ascend()
	{
		if (AutumnVRGameManager.over || stanTimer > 0) {
			return;
		}

		height += ascend_value;
		transform.DOMoveY( height, 0.5f);

		Debug.Log(height.ToString() + "まで上昇");

		if( height >= AutumnVRGameManager.goalHeight )
		{
			Debug.LogError("ゴール！！");
			AutumnVRGameManager.over = true;
			clearText.gameObject.SetActive (true);
			clearText.text = "くりあたいむ\n" + ((int)AutumnVRGameManager.gameTimer).ToString () + "びょう";
			myAudio.PlayOneShot (se_clear);
		}

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
	/// <param name="pat">ボイスタイプ</param>
	private void Roar()
	{
		int key = Random.Range (0, se_roar.Length - 1);

		// うめき声を再生
		myAudio.PlayOneShot( se_roar[key] );
	}

	void CheckCollisionChestnut()
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

			// 栗との高度比較
			dist =  Mathf.Abs( (transform.position.y - c.transform.position.y) );

			if( dist <= hitRange )
			{
				Debug.Log("栗が筋肉にヒット！");

				stanTimer += 3f;

				c.Harvest(false);
				Roar();
			}
		}
	}
}
