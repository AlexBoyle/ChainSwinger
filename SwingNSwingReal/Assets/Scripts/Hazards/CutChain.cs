using UnityEngine;
using System.Collections;

public class CutChain : MonoBehaviour {
	GameObject ChainHitbox;
	public bool On = true;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other){
		if (On) {
			if (other.tag == "Chain") {
				other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);
			}
		}
	}

}
