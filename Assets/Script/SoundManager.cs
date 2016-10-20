using UnityEngine;
using System.Collections;

/// <summary>
/// 効果音と音楽を管理
/// </summary>
public class SoundManager : MonoBehaviour {

	[SerializeField]
	private AudioSource myAudio;

	[SerializeField]
	private AudioClip[] clips;

	/// <summary>
	/// 効果音を再生
	/// </summary>
	public void PlaySe()
	{
		// Play
	}
}
