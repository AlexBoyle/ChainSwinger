using UnityEngine;
using System.Collections;

public class SwingHitboxScript : MonoBehaviour {
	public GameObject Owner, ChainHitbox;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && other.gameObject != Owner) {
			other.GetComponent<HealthScript> ().DealDamage (100);
		}else if(other.tag == "Chain" && other.gameObject != ChainHitbox){
			other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);

		}
	}
}
