using UnityEngine;
using System.Collections;

public class BounceSword : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other){
		
		Rigidbody2D temp = other.attachedRigidbody;
		if (temp != null)
		temp.velocity = new Vector2(temp.velocity.x,10 );

	}

}