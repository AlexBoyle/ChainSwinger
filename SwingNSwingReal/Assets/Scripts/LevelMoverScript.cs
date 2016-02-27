using UnityEngine;
using System.Collections;

public class LevelMoverScript : MonoBehaviour {
	public bool UpDown;
	public bool LeftRight;
	bool StageLR, StageUD;
	public float UpDownMax, UpDownMin, LeftRightMax, LeftRightMin, Speed;
	bool CanMove = false;
	// Use this for initialization
	void Start () {
		Invoke ("EnableMovement", 2.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (CanMove){
			if (UpDown){
				if (StageUD){
					transform.position -=  new Vector3(0, Speed, 0);
					if (transform.position.y <= UpDownMin){StageUD = false;}
				}else if (!StageUD){
					transform.position += new Vector3(0, Speed, 0);
					if (transform.position.y >= UpDownMax){StageUD = true;}
				}
			}
			if (LeftRight){
				if (StageLR){
					transform.position -= new Vector3(Speed, 0, 0);
					if (transform.position.x <= LeftRightMin){StageLR = false;}
				}else if (!StageLR){
					transform.position += new Vector3(Speed, 0, 0);
					if (transform.position.x >= LeftRightMax){StageLR = true;}
				}
			}
		}
	}
	void EnableMovement(){
		CanMove = true;
	}
}
