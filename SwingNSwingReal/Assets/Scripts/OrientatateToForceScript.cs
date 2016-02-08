using UnityEngine;
using System.Collections;

public class OrientatateToForceScript : MonoBehaviour {
	Rigidbody2D RB;
	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs( RB.velocity.x) > 2 || Mathf.Abs( RB.velocity.y) > 2) {
			Debug.Log ("inuse");
			transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 (RB.velocity.y, RB.velocity.x) * Mathf.Rad2Deg - 90);
		}
	}
}
