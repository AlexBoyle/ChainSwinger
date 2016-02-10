using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] players;
	// Use this for initialization
	void Start () {
		//InitialSpawn (0);
		//InitialSpawn (1);
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
		ghost.SetActive(false);
		player.SetActive (true);
		players [playerNum].GetComponentInChildren<HealthScript> ().FillHealth (100);
	}
	/* offline code
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


	} 
	*/

	public void InitialSpawn(int playerNumber){
		GameObject tmp =  PhotonNetwork.Instantiate ("PlayerWrapper", Vector3.zero, Quaternion.identity, 0) as GameObject;
		if (playerNumber == 0) {
			tmp.transform.position = new Vector3 (-4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.blue;
		} else {
			tmp.transform.position = new Vector3 (4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
		}
		tmp.GetComponentInChildren<PlayerControlScript> ().playerNumber = playerNumber;
		players [playerNumber] = tmp;
	}
}
