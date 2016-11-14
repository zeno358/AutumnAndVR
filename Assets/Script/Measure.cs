using UnityEngine;
using System.Collections;

public class Measure : MonoBehaviour {

	TextMesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
	}

	// Update is called once per frame
	void Update () {
		mesh.color = Muscle.height >= transform.position.y ? Color.yellow : Color.red;
	}
}
