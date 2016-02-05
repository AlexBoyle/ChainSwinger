using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] players;
	CameraFollowScript CFS;
	// Use this for initialization
	void Start () {
		CFS = GameObject.Find ("Main Camera").GetComponent<CameraFollowScript> ();
		InitialSpawn (0);
		InitialSpawn (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespawnPlayer(float delay,int playerNum){
		StartCoroutine (DelayRespawn(delay, playerNum));
	}

	IEnumerator DelayRespawn(float delay, int playerNum){
		GameObject ghost = players [playerNum].transform.FindChild ("Ghost").gameObject;
		GameObject player = players [playerNum].transform.FindChild ("Player").gameObject;
		ghost.transform.position = player.transform.position;
		ghost.SetActive(true);
		yield return new WaitForSeconds (delay);
		player.transform.position = ghost.transform.position;
		ghost.GetComponent<ParticleEmitterScript> ().EmitBurst (75);
		ghost.SetActive(false);
		player.SetActive (true);
		players [playerNum].GetComponentInChildren<HealthScript> ().FillHealth (100);
	}

	void InitialSpawn(int playerNumber){
		GameObject tmp =  Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		if (playerNumber == 0) {
			tmp.transform.position = new Vector3 (-4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.blue;
		} else {
			tmp.transform.position = new Vector3 (4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
		}
		tmp.GetComponentInChildren<PlayerControlScript> ().playerNumber = playerNumber;
		players [playerNumber] = tmp;
		CFS.addPlayer (tmp.transform.GetChild(0).transform, playerNumber);


	}
}
