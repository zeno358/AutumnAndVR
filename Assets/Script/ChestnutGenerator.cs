using UnityEngine;
using System.Collections;

/// <summary>
/// イガグリ生成装置
/// </summary>
public class ChestnutGenerator : Photon.MonoBehaviour {

	static ChestnutGenerator instance;

	/// <summary>
	/// 生成頻度
	/// </summary>
	public float interval = 1f;
	float timer = 0;

	/// <summary>
	/// 生成位置の誤差
	/// </summary>
	public Vector2 diffRange;

	/// <summary>
	/// イガグリ
	/// </summary>
	public GameObject Chestnut;

	AudioSource myAudio;

	void Start()
	{
		instance = this;

		myAudio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () 
	{
		if( !AutumnVRGameManager.running)
		{
			return;
		}

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

		//　生成位置を決定
		Vector3 pos = new Vector3(transform.position.x + Random.Range( -diffRange.x, diffRange.x ), transform.position.y, transform.position.z + Random.Range( -diffRange.y, diffRange.y ));

		// photn上に生成
		PhotonNetwork.Instantiate("Chestnut", pos, Quaternion.identity, 0);

		// SEを再生
		myAudio.Play();
	}
}
