using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Measure : MonoBehaviour {

	TextMesh mesh;

	MeshRenderer r;

	bool enablePrev;

	public void SetText(string value){
		if (mesh == null) {
			mesh = GetComponent<TextMesh>();
		}
		mesh.text = value;
	}

	SpriteRenderer f;
	// Use this for initialization
	void Start () {
		if (mesh == null) {
			mesh = GetComponent<TextMesh>();
		}	

		r = GetComponent<MeshRenderer> ();

		if (f == null) {
			f = transform.GetChild (0).GetComponent<SpriteRenderer> ();
		}	

		enablePrev = false;
	}

	// Update is called once per frame
	void Update () {
		int unitVal = AutumnVRGameManager.measureExpInterval;

		// 近くだけ表示
		r.enabled = MuscleTest.height > transform.position.y - unitVal && MuscleTest.height < transform.position.y + unitVal;

		if (f.gameObject.activeInHierarchy) {
			f.enabled = r.enabled;
		}

		if (!enablePrev && r.enabled && AutumnVRGameManager.running) {
			FrameIn ();
		}
		enablePrev = r.enabled;

		// 到達したら黄色
		mesh.color = MuscleTest.height >= transform.position.y ? Color.yellow : Color.red;
	}

	void FrameIn()
	{
		Vector3 target = transform.position;
		transform.Translate(Vector3.right * 50f);
		transform.DOMove (target, 1f).SetEase(Ease.OutBack);
	}

	public void EnableFlag(bool key)
	{
		if (f == null) {
			f = transform.GetChild (0).GetComponent<SpriteRenderer> ();
		}

		f.gameObject.SetActive (key);
	}
}
