using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int health;
	RespawnScript RS;
	PlayerControlScript PCS;
	// Use this for initialization
	void Start () {
		RS = GameObject.Find ("RespawnObject").GetComponent<RespawnScript>();
		PCS = GetComponent<PlayerControlScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void CheckIfKilled(){
		if (health <= 0) {
			transform.parent.gameObject.SetActive (false);
			RS.RespawnPlayer (2f,PCS.GetPlayerNumber ());
		}
	}
	public void DealDamage (int amount){
		health -= amount;
		CheckIfKilled ();
	}
	public void FillHealth(int amount){
		health = amount;
	}
}
