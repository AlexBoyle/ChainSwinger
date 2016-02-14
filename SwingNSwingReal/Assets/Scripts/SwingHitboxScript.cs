using UnityEngine;
using System.Collections;

public class SwingHitboxScript : MonoBehaviour {
	public GameObject Owner, ChainHitbox;
	public Rigidbody2D RB;
	public bool isTip;
	bool ableToBePickedUp = false;
	public float timeTillDeactivate;
	public int pNum;
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

		Debug.Log (other.tag);
		if (other.tag == "Player" ) {
			if (other.gameObject != Owner) {
				other.GetComponent<HealthScript> ().DealDamage (100, pNum, isTip);
			} else if (ableToBePickedUp){
				other.GetComponent<PlayerControlScript> ().ReturnSword();
				transform.parent.parent.gameObject.SetActive (false);
			}
		}else if (other.tag == "Ghost") {
			Debug.Log ("swrod");
			if (other.GetComponent<GhostControl> ().GetPlayerNumber () == pNum){
				other.GetComponent<GhostControl>().ReturnSword();
				transform.parent.parent.gameObject.SetActive (false);
			}
		}else if (other.tag == "ThrownSword"){
			other.GetComponent<SwingHitboxScript> ().KnockAway (Owner.transform.position);
		}else if(other.tag == "Chain" && other.gameObject != ChainHitbox){
			other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);

		}
	}

	public void KnockAway(Vector3 pos){
		Vector3 newDir = Vector3.zero;
		if (pos.x > transform.position.x) {
			newDir.x = -10;	
		} else {			
			newDir.x = 10;	
		}

		if (pos.y > transform.position.y){
			newDir.y = -10;
		}else {
			newDir.y = 10;
		}
		RB.velocity = newDir;
	}

	IEnumerator pickUpTimer(){
		yield return new WaitForSeconds (.1f);
		ableToBePickedUp = true;
	}
}
