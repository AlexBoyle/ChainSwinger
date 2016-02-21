using UnityEngine;
using System.Collections;	
public class KillOnTouchScript : MonoBehaviour {
	void Start () {
		gameObject.GetComponent<PolygonCollider2D> ().isTrigger = true;
	}
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log (other.tag);
		if (other.tag == "Player") {
			
			other.GetComponent<HealthScript> ().DealDamage (100, -1, true);
		}
	}
}
