using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Measure : MonoBehaviour {

	TextMesh mesh;

	MeshRenderer r;

	bool enablePrev;

	// Use this for initialization
	void Start () {
		enablePrev = false;
		mesh = GetComponent<TextMesh>();
		r = GetComponent<MeshRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		int unitVal = AutumnVRGameManager.measureExpInterval;

		// 近くだけ表示
		r.enabled = Muscle.height > transform.position.y - unitVal && Muscle.height < transform.position.y + unitVal;

		if (!enablePrev && r.enabled && AutumnVRGameManager.running) {
			FrameIn ();
		}
		enablePrev = enabled;

		// 到達したら黄色
		mesh.color = Muscle.height >= transform.position.y ? Color.yellow : Color.red;
	}

	void FrameIn()
	{
		float target = transform.localPosition.x;
		transform.localPosition = Vector3.right * 50f;
		transform.DOLocalMoveX (target, 3f);
	}
}
