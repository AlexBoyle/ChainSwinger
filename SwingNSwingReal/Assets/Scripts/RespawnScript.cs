using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class RespawnScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] players;
	CameraFollowScript CFS;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	// Use this for initialization
	void Start () {
		CFS = GameObject.Find ("Main Camera").GetComponent<CameraFollowScript> ();

	}
	
	// Update is called once per frame
	void Update () {
		for (int x = 0; x < 4; x++) {
			state = GamePad.GetState ((PlayerIndex)x, GamePadDeadZone.None);

			if (state.Buttons.Start == ButtonState.Pressed && players[x] == null) {
				InitialSpawn (x);
			}
		}
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
		switch (playerNumber) {
		case 0:
			tmp.transform.position = new Vector3 (-4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.red);
			break;
		case 1:
			tmp.transform.position = new Vector3 (4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.blue;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.blue);
			break;
		case 2:
			tmp.transform.position = new Vector3 (-8, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.yellow;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.yellow);
			break;
		case 3:
			tmp.transform.position = new Vector3 (8, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.green);
			break;
		}

		tmp.GetComponentInChildren<PlayerControlScript> ().playerNumber = playerNumber;
		players [playerNumber] = tmp;
		CFS.addPlayer (tmp.transform.GetChild(0).transform, players [playerNumber].transform.FindChild ("Ghost").gameObject.transform, playerNumber);


	}
	public void OnlineInitalSpawn(){
		GameObject MyPlayerGO =  (GameObject)PhotonNetwork.Instantiate ("OnlinePlayerWrapper", Vector3.zero, Quaternion.identity, 0);
		MyPlayerGO.GetComponentInChildren<InputScript> ().enabled = true;

	}

}
