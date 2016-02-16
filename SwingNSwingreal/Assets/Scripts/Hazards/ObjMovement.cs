using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjMovement : MonoBehaviour {
	public float speed = .001f;
	float posNum = 0;
	Vector3 start;
	int arrPos = 1;
	int arrMax;
	Vector3 end;
	public Vector3[] positions;
	// Use this for initialization
	void Start () {
		arrMax = positions.Length;
		start = positions [0];
		end = positions [1];
	}
	

	void FixedUpdate () {
		if (posNum < 1f)
			posNum += speed;
		else {
			posNum = 0;
			start = end;
			arrPos++;
			if (arrPos >= arrMax)
				arrPos = 0;
			end = positions [arrPos];
		}
		gameObject.GetComponent<Transform> ().position = Vector3.Lerp (start, end, posNum);
	}
}
