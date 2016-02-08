using UnityEngine;
using System.Collections;

public class RotationAndScale : MonoBehaviour {
	Rigidbody RB;
	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// rotate to face direction that we are moving
		float Zrotation = 90 + Mathf.Atan2(RB.velocity.normalized.x, -RB.velocity.normalized.y) * 180.0f / Mathf.PI;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Zrotation));

		// stretch more the faster object is moving
		if (RB.velocity.magnitude/3 > 1) {
			transform.localScale = new Vector3 (RB.velocity.magnitude/3, 1, 1);
		} else {
			transform.localScale = new Vector3 (1, 1, 1);
		}
	}
}
