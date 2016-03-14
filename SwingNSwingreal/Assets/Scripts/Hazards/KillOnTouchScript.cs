using UnityEngine;
using System.Collections;	
public class KillOnTouchScript : MonoBehaviour {
	public bool On = true;
	void Start () {
		gameObject.GetComponent<PolygonCollider2D> ().isTrigger = true;
	}
	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player" && On) {
			
			other.GetComponent<HealthScript> ().DealDamage (100, -1, true);
		}
	}
}
