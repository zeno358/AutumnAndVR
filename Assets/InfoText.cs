/*
開始前テキスト

string str = “”;

str += 

	"くりあたいむ” + Time + “¥n” ;
	str += "ひろった栗の数 = ” + Chestnut.Count + “¥n”;
	str += “おとした栗の数 = ” + Chestnut.Count + “¥n”;

foreach(Photon.

	str +=
	"あなたのふみつけ数” +  00
	ともだちのふみつけ数 00
	ごうけいふみつけ数 00
	*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲーム情報を表示する
/// </summary>
public class InfoText : Photon.MonoBehaviour {

	public TextMesh mesh;

	public MeshRenderer renderer;

	bool initialized = false;
	// Use this for initialization
	void Start () {
		// プレイヤーのほうを向く
		StartCoroutine( WaitForPlayer() );
	}

	IEnumerator WaitForPlayer()
	{
		while(true)
		{
			if(PhotonNetwork.room == null)
			{
				yield return null;	
			}
			else
			{
				break;
			}
		}

		int playerId = PhotonNetwork.player.ID;

		while(true)
		{
			if(TwoPlayerTest.crews == null || TwoPlayerTest.crews.Count <= 0)
			{
				yield return null;	
			}
			else
			{
				break;
			}
		}

		Transform target = null;

		foreach( CrewMoveTest c in TwoPlayerTest.crews)
		{
			if( c.photonView.isMine )
			{
				target = c.transform;
				break;
			}	
		}

		// テキストなのでプレイヤーに背中を向ける
		Vector3 dir = transform.position - target.transform.position;
		Quaternion q = Quaternion.LookRotation( dir );
		transform.rotation = q;

		initialized = true;
	}
	
	// Update is called once per frame
	void Update () 
	{	if(!initialized || GameManagerTest.running ){
			return;
		}
	
		UpdateText();

	}

	void UpdateText()
	{
		string str;

		int playerNum = PhotonNetwork.room.playerCount;
		if(  playerNum < TwoPlayerTest.playerNumNeeded )
		{
			str = "プレイヤーの参加を待機中 ( " + playerNum.ToString() + " / " + TwoPlayerTest.playerNumNeeded + " )";
		}
		else
		{
			bool ready1p = false;
			bool ready2p = false;
			for( int i=0 ; i< TwoPlayerTest.crews.Count ; i++ )
			{
				CrewMoveTest c = TwoPlayerTest.crews[i];
				if ( c.photonView.ownerId == 1 )
				{
					ready1p = c.ready;
				}
				else
				{
					ready2p = c.ready;
				}
			}

			str = "プレイヤーの準備を待機中 ( 1P " + (ready1p ? "◯" : "×") + " / 2P " + (ready2p ? "◯" : "×") + " )"; 
		}

		mesh.text = str;
	}
}
