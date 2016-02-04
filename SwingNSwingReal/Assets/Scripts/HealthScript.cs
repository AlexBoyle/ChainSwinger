using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int health;
	RespawnScript RS;
	PlayerControlScript PCS;
	public ParticleEmitterScript BS;
	ScoreScript SS;
	// Use this for initialization
	void Start () {
		RS = GameObject.Find ("RespawnObject").GetComponent<RespawnScript>();
		PCS = GetComponent<PlayerControlScript> ();
		SS = GameObject.Find ("ScoreObject").GetComponent<ScoreScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void CheckIfKilled(){
		if (health <= 0) {
			transform.gameObject.SetActive (false);
			BS.transform.position = transform.position;
			BS.EmitParticles (50);
			SS.IncrementKill (PCS.GetPlayerNumber ());
			RS.RespawnPlayer (3f,PCS.GetPlayerNumber ());
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
