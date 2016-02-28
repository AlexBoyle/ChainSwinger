using UnityEngine;
using System.Collections;

public class knifechain : MonoBehaviour {
	public GameObject chain;
	public GameObject knife;
	GameObject knifet;
	public int len = 10;
	public bool leftOrRight = true;
	bool reset = true;
	public float time = 0f;
	public int timeTillReset = 600;
	Vector3 start;
	Vector3 end;
	// Use this for initialization
	void Start () {
		if(leftOrRight)
			end = new Vector3 (len*.32f,-.32f,0) + gameObject.transform.position;
		else
			end = new Vector3 (-len*.32f,-.32f,0) + gameObject.transform.position;
		JointAngleLimits2D lim = new JointAngleLimits2D();
		lim.min = 125;
		lim.max = -125;
		GameObject temp = (Instantiate (chain) as GameObject);
		Rigidbody2D rigtemp = temp.GetComponent<Rigidbody2D> ();
		rigtemp.constraints =RigidbodyConstraints2D.FreezePosition;
		temp.transform.parent = gameObject.transform;
		temp.GetComponent<Transform> ().position = new Vector3 (gameObject.transform.position.x,gameObject.transform.position.y,0);
		knifet = (Instantiate (knife) as GameObject);
		knifet.transform.parent = gameObject.transform;
		knifet.AddComponent<DistanceJoint2D> ();
		knifet.GetComponent<DistanceJoint2D> ().connectedBody = rigtemp;
		knifet.GetComponent<DistanceJoint2D> ().distance = (len * .32f) + .32f;
		knifet.GetComponent<DistanceJoint2D> ().maxDistanceOnly = true;
		knifet.transform.position = new Vector3 (0, -len*.32f, 0) + gameObject.transform.position;
		for (int i = 0; i < len; i++) {
			//lim.min = lim.min -(i*2);
			//lim.max = lim.max + (i*2);
			temp = (Instantiate (chain) as GameObject);
			temp.transform.parent = gameObject.transform;
			temp.transform.position = new Vector3 (0, gameObject.transform.position.y -((i+1) *.32f),0);
			temp.AddComponent<HingeJoint2D> ();
			temp.GetComponent<HingeJoint2D> ().connectedBody = rigtemp;
			temp.GetComponent<HingeJoint2D> ().anchor = new Vector2 (0, .16f);
			temp.GetComponent<HingeJoint2D> ().connectedAnchor = new Vector2 (0, -.16f);
			temp.GetComponent<HingeJoint2D> ().useLimits = true;
			temp.GetComponent<HingeJoint2D> ().limits = lim;
			rigtemp = temp.GetComponent<Rigidbody2D>();
		}
		knifet.AddComponent<HingeJoint2D> ();
		knifet.GetComponent<HingeJoint2D> ().connectedBody = rigtemp;
		knifet.GetComponent<HingeJoint2D> ().anchor = new Vector2 (0, .16f);
		knifet.GetComponent<HingeJoint2D> ().connectedAnchor = new Vector2 (0, -.16f);
		knifet.GetComponent<HingeJoint2D> ().limits = lim;
		resetObject ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (time < 1) {
			time += .01f;
			knifet.transform.position = Vector3.Lerp (start, end, time);
		}
		if (timeTillReset < 500)
			timeTillReset++;
		else if (timeTillReset == 500)
			resetObject ();
	}
	public void go(){
		if (time >= 1f && timeTillReset >= 500) {
			timeTillReset = 0;
			knifet.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
		}
	}
	void resetObject(){
		time = 0;
		timeTillReset = 600;
		start = knifet.transform.position ;
		knifet.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePosition;
	}

}
