using UnityEngine;
using System.Collections;

public class ChainAnimScript : MonoBehaviour {
	public Vector3 endPoint, startPos;
	// Use this for initialization
	void Start () {
		transform.localPosition = startPos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.x > endPoint.x) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, endPoint, .01f);
		}

	}
}
