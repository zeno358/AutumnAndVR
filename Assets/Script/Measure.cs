using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Measure : MonoBehaviour {

	TextMesh mesh;

	MeshRenderer r;

	bool enablePrev;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		r = GetComponent<MeshRenderer> ();
		enablePrev = false;
	}

	// Update is called once per frame
	void Update () {
		int unitVal = AutumnVRGameManager.measureExpInterval;

		// 近くだけ表示
		r.enabled = Muscle.height > transform.position.y - unitVal && Muscle.height < transform.position.y + unitVal;

		if (!enablePrev && r.enabled && AutumnVRGameManager.running) {
			FrameIn ();
		}
		enablePrev = r.enabled;

		// 到達したら黄色
		mesh.color = Muscle.height >= transform.position.y ? Color.yellow : Color.red;
	}

	void FrameIn()
	{
		Vector3 target = transform.position;
		transform.Translate(Vector3.right * 50f);
		transform.DOMove (target, 1f).SetEase(Ease.OutBack);
	}
}
