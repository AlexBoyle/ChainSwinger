using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] players;
	// Use this for initialization
	void Start () {
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
		yield return new WaitForSeconds (delay);
		players [playerNum].transform.FindChild("Player").transform.position = Vector3.zero;
		players [playerNum].GetComponentInChildren<HealthScript> ().FillHealth (100);
		players [playerNum].SetActive (true);
	}

	void InitialSpawn(int playerNumber){
		GameObject tmp =  Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		if (playerNumber == 0) {
			tmp.transform.position = new Vector3 (-4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
		} else {
			tmp.transform.position = new Vector3 (4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.blue;
		}
		tmp.GetComponentInChildren<PlayerControlScript> ().playerNumber = playerNumber;
		players [playerNumber] = tmp;


	}
}
