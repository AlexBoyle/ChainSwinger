using UnityEngine;
using System.Collections;

public class SwingHitboxScript : MonoBehaviour {
	public GameObject Owner, ChainHitbox;
	public bool isTip;
	bool ableToBePickedUp = false;
	public float timeTillDeactivate;
	int pNum;
	// Use this for initialization
	void Awake () {
		pNum = Owner.GetComponent<PlayerControlScript> ().GetPlayerNumber (); 
	}
	void OnEnable(){
		ableToBePickedUp = false;
		StartCoroutine (pickUpTimer());
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" ) {
			if (other.gameObject != Owner) {
				other.GetComponent<HealthScript> ().DealDamage (100, pNum, isTip);
			} else if (ableToBePickedUp){
				other.GetComponent<PlayerControlScript> ().ReturnSword();
				transform.parent.parent.gameObject.SetActive (false);
			}
		}else if(other.tag == "Chain" && other.gameObject != ChainHitbox){
			other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);

		}
	}

	IEnumerator pickUpTimer(){
		yield return new WaitForSeconds (.1f);
		ableToBePickedUp = true;
	}
}
