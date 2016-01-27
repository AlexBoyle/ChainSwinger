using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
	public Image[] scoreBar;
	public Text winText;
	public float killsToWin;
	public float[] playerKills;
	public GameObject[] Players;
	public bool noWinners;
	int nextPlayer;
	// Use this for initialization
	void Start () {
		noWinners = true;
		nextPlayer = 0;
		playerKills = new float[4];
		Players = new GameObject[4];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void AddPlayer(GameObject newPlayer){
		Players[nextPlayer] = newPlayer;
		nextPlayer++;
	}
	public void IncrementKill(int playerNumber){
		if (noWinners) {
			playerKills [playerNumber]++;
			StartCoroutine (LerpBar ());
			CheckWin ();
		}
	}
	void CheckWin(){
		for (int x = 0; x < 4; x++) {
			if (playerKills[x] >= killsToWin){
				
				noWinners = false;
				for(int i = 0; i < nextPlayer; i++){
					Players [i].GetComponent<PlayerControlScript> ().DisableControls ();
				}
				winText.text = "Player " + x + " Wins";
				Invoke ("ReloadLevel", 5f);

			}
		}
	}
	void ReloadLevel(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex);
	}
	IEnumerator LerpBar(){
		while (scoreBar[0].fillAmount  < playerKills[0]/killsToWin || scoreBar[1].fillAmount  < playerKills[1]/killsToWin || scoreBar[2].fillAmount  < playerKills[2]/killsToWin || scoreBar[3].fillAmount  < playerKills[3]/killsToWin){

			for (int x = 0; x < 4; x++){
				if (scoreBar [x].fillAmount < playerKills [x] / killsToWin) {
					scoreBar [x].fillAmount += .002f;
				}
			}
			yield return new WaitForSeconds(.015f);
		}
	}
}
