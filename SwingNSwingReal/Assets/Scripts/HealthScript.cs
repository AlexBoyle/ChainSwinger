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
	void CheckIfKilled(int killerPnum, bool tipKill){
		if (health <= 0) {
			transform.gameObject.SetActive (false);
			BS.transform.position = transform.position;
			// more elaborate effects for the tip kill
			Debug.Log(tipKill);
			if (tipKill) {
				BS.EmitParticles (125, 1, 2f, 1.5f);
			} else {
				BS.EmitParticles (50);
			}
			SS.IncrementKill (killerPnum);
			
			RS.RespawnPlayer (3f,PCS.GetPlayerNumber ());
		}
	}
	public void DealDamage (int amount, int killerPnum ,bool tipKill){
		health -= amount;
		CheckIfKilled (killerPnum ,tipKill);
	}
	public void FillHealth(int amount){
		health = amount;
	}
	
}
