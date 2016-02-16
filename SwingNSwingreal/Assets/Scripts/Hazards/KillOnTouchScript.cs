using UnityEngine;
using System.Collections;

public class KillOnTouchScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			
			other.GetComponent<HealthScript> ().DealDamage (100, -1, true);
		}
	}
}
