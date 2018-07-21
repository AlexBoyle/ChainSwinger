using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjMovement : MonoBehaviour {
	public float speed = .05f;
	float posNum = 0;
	Vector3 start;
	int arrPos = 1;
	int arrMax;
	Vector3 end;
	public Vector3[] positions = {new Vector3(0,0,0),new Vector3(0,0,0)};
	// Use this for initialization
	void Start () {
		arrMax = positions.Length;
		start = positions [0];
		end = positions [1];
	}
	

	void FixedUpdate () {
		
		if(gameObject.transform.position == end) {
			posNum = 0;
			start = end;
			arrPos++;
			if (arrPos >= arrMax)
				arrPos = 0;
			end = positions [arrPos];
		}
		transform.position = Vector3.MoveTowards (transform.position, end, speed);
	}
}
