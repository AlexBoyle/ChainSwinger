using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour {
	public float rotateSpeed;
	public bool RotateBySpeed;
	public Rigidbody2D RB;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!RotateBySpeed) {
			transform.Rotate (new Vector3 (0, 0, rotateSpeed));
		} else {
			transform.Rotate (new Vector3 (0, 0, rotateSpeed * RB.velocity.magnitude));
		}
	}
}
