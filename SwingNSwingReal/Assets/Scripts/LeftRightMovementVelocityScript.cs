using UnityEngine;
using System.Collections;

public class LeftRightMovementVelocityScript : MonoBehaviour {

	public bool UpDown;
	public bool LeftRight;
	bool StageLR, StageUD;
	public float LeftRightMax, LeftRightMin, Speed;
	bool CanMove = false;
	Rigidbody2D RB;
	// Use this for initialization
	void Start () {
		Invoke ("EnableMovement", 2.5f);
		RB = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (CanMove){
			if (LeftRight){
				if (StageLR){
					RB.velocity = new Vector2 (-Speed, 0);
					if (transform.position.x <= LeftRightMin){StageLR = false;}
				}else if (!StageLR){
					RB.velocity = new Vector2 (Speed, 0);
					if (transform.position.x >= LeftRightMax){StageLR = true;}
				}
			}
		}
	}
	void EnableMovement(){
		CanMove = true;
	}
}
