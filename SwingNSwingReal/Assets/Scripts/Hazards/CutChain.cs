using UnityEngine;
using System.Collections;

public class CutChain : MonoBehaviour {
	GameObject ChainHitbox;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log (other.tag);
		if(other.tag == "Chain" ){
			other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);
		}
	}

}
