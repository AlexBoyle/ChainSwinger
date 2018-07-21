using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class RespawnScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject[] players;
	CameraFollowScript CFS;
	SoundPlayerScript SPS;

	bool gameStarted = false;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	// Use this for initialization
	void Start () {
		CFS = GameObject.Find ("Main Camera").GetComponent<CameraFollowScript> ();
		SPS = GameObject.Find ("SoundObject").GetComponent<SoundPlayerScript> ();
		Invoke ("StartGame", 2f);
	}

	void StartGame(){
		gameStarted = true;
	}
	// Update is called once per frame
	void Update () {
		if (gameStarted) {
			for (int x = 0; x < 4; x++) {
				state = GamePad.GetState ((PlayerIndex)x, GamePadDeadZone.None);

				if (state.Buttons.Start == ButtonState.Pressed && players [x] == null) {
					InitialSpawn (x);
				}
			}
		}
	}

	public void RespawnPlayer(float delay,int playerNum){
		StartCoroutine (DelayRespawn(delay, playerNum));
	}

	IEnumerator DelayRespawn(float delay, int playerNum){
		GameObject ghost = players [playerNum].transform.Find ("Ghost").gameObject;
		GameObject player = players [playerNum].transform.Find ("Player").gameObject;
		SPS.PlayDeath ();
		ghost.transform.position = player.transform.position;
		ghost.SetActive(true);
		yield return new WaitForSeconds (delay);
		player.transform.position = ghost.transform.position;
		ghost.GetComponent<ParticleEmitterScript> ().EmitBurst (75);
		ghost.SetActive(false);
		player.SetActive (true);
		SPS.PlayGhostRespawn ();
		players [playerNum].GetComponentInChildren<HealthScript> ().FillHealth (100);
	}

	public void InitialSpawn(int playerNumber){
		GameObject tmp =  Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		Color tcolor;
		tmp.GetComponentInChildren<PlayerControlScript> ().playerNumber = playerNumber;
		players [playerNumber] = tmp;
		switch (playerNumber) {
		case 0:
			tmp.transform.position = new Vector3 (-4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.red);
			tcolor = Color.red;
			tcolor.a = .5f;
			players [playerNumber].transform.Find ("Ghost").gameObject.GetComponent<SpriteRenderer> ().color = tcolor;
			break;
		case 1:
			tmp.transform.position = new Vector3 (4, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.blue;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.blue);
			tcolor = Color.blue;
			tcolor.a = .5f;
			players [playerNumber].transform.Find ("Ghost").gameObject.GetComponent<SpriteRenderer> ().color = tcolor;
			break;
		case 2:
			tmp.transform.position = new Vector3 (-8, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.yellow;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.yellow);
			tcolor = Color.yellow;
			tcolor.a = .5f;
			players [playerNumber].transform.Find ("Ghost").gameObject.GetComponent<SpriteRenderer> ().color = tcolor;
			break;
		case 3:
			tmp.transform.position = new Vector3 (8, 0, 0);
			tmp.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
			tmp.GetComponentInChildren<PlayerControlScript> ().SetColor (Color.green);
			tcolor = Color.green;
			tcolor.a = .5f;
			players [playerNumber].transform.Find ("Ghost").GetComponent<SpriteRenderer> ().color = tcolor;
			break;
		}


		CFS.addPlayer (tmp.transform.GetChild(0).transform, players [playerNumber].gameObject.transform.Find ("Ghost").gameObject.transform, playerNumber);
		tmp.GetComponentInChildren<HealthScript> ().DealDamage (100, -2, false);

	}
}
