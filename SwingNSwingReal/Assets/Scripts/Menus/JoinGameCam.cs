using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class JoinGameCam : MonoBehaviour {
	public GameObject pressA;
	public int players = 0;
	public int readyPlayers = 0;
	// Use this for initialization
	void Start () {
		int down = 1;
		int across = 1;
		for (int i = 0; i < 4; i++) {
			if (across == 1)
				across = -1;
			else
				across = 1;
			if (i  == 2)
				down = -1;
			
			GameObject temp = (Instantiate (pressA) as GameObject);
			temp.GetComponent<Transform> ().position = new Vector3 (4.43f * across, 2.52f * down, 0);
			temp.GetComponent<PressAScript> ().setPlayerNumber (i);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (players > 1 && players == readyPlayers) {
			//send info to an objec that is not destroyed to move info to the next scene
			//num of players
			GameObject.Find("ImmortalObject").GetComponent<ImmortalObjectScript>().numPlayers(players);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
		}
	}
	public void playerJoin(){
		players++;
	}
	public void playerLeave(){
		players--;
	}
	public void playerReady(){
		readyPlayers++;
	}
	public void playerNotReady(){
		readyPlayers--;
	}
}
