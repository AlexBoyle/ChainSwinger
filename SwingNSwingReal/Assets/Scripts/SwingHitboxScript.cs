using UnityEngine;
using System.Collections;

public class SwingHitboxScript : MonoBehaviour {
	public GameObject Owner, ChainHitbox;
	public Rigidbody2D RB;
	public SpriteRenderer swordSprite;
	public bool isTip;
	bool ableToBePickedUp = false;
	public float timeTillDeactivate;
	public int pNum, damagePNum;
	// Use this for initialization
	void Awake () {
		pNum = Owner.GetComponent<PlayerControlScript> ().GetPlayerNumber (); 
		damagePNum = pNum;
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
			int otherPnum = other.GetComponent<PlayerControlScript> ().GetPlayerNumber ();
			if (otherPnum != damagePNum) {
				other.GetComponent<HealthScript> ().DealDamage (100, damagePNum, isTip);
			} else if (ableToBePickedUp && otherPnum == pNum){
				other.GetComponent<PlayerControlScript> ().ReturnSword();
				transform.parent.parent.gameObject.SetActive (false);
				returnSword ();
			}
		}else if (other.tag == "Ghost") {
			Debug.Log ("swrod");
			if (other.GetComponent<GhostControl> ().GetPlayerNumber () == pNum && pNum ==  damagePNum){
				other.GetComponent<GhostControl>().ReturnSword();
				returnSword();
				transform.parent.parent.gameObject.SetActive (false);
			}
		}else if (other.tag == "ThrownSword"){
			other.GetComponent<SwingHitboxScript> ().KnockAway (Owner.transform.position, damagePNum);
		}else if(other.tag == "Chain" && other.gameObject != ChainHitbox){
			other.GetComponent<ChainDestructionScript> ().DestroyChain (transform.position);

		}
	}
	void OnDisable(){
		returnSword ();
	}
	void returnSword(){
		StopAllCoroutines ();
		swordSprite.color = Color.white;
		damagePNum = pNum;
	}
	public void KnockAway(Vector3 pos, int newPNum){
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
		StartCoroutine (SwordRepel(newPNum));
	}
	IEnumerator SwordRepel(int newPNum){
		damagePNum = newPNum;
		swordSprite.color = Color.red;
		yield return new WaitForSeconds (1.5f);
		swordSprite.color = Color.white;
		damagePNum = pNum;
	}
	IEnumerator pickUpTimer(){
		yield return new WaitForSeconds (.25f);
		ableToBePickedUp = true;
	}
}
