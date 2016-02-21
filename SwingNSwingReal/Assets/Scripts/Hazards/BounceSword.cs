using UnityEngine;
using System.Collections;

public class BounceSword : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
	}
	void OnTriggerEnter2D(Collider2D other){
		
		Rigidbody2D temp = other.attachedRigidbody;
		Debug.Log (temp.velocity);
		temp.velocity = new Vector2(-temp.velocity.x,-temp.velocity.y );

	}

}