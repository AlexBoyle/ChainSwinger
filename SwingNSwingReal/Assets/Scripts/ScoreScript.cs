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


		if (noWinners && playerNumber >= 0) {
			playerKills [playerNumber]++;
			StartCoroutine (LerpBar ());
			CheckWin ();
		}
	}
	public void DecrementKills(int playerNumber){
		if (playerKills[playerNumber] > 0){
			playerKills [playerNumber]--;
			StartCoroutine (LerpBar ());
		}
	}
	void CheckWin(){
		for (int x = 0; x < 4; x++) {
			if (playerKills[x] >= killsToWin){
				
				noWinners = false;
				for(int i = 0; i < nextPlayer; i++){
					Players [i].GetComponent<PlayerControlScript> ().DisableControls ();
				}

				string winColor = "";
				switch (x) {
				case 0:
					winColor = "Red";
					break;
				case 1:
					winColor = "Blue";
					break;
				case 2:
					winColor = "Yellow";
					break;
				case 3:
					winColor = "Green";
					break;
				}
				winText.text = winColor  + " Wins";
				Invoke ("LevelSelect", 5f);

			}
		}
	}
	void LevelSelect(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}
	IEnumerator LerpBar(){
		while (Mathf.Abs( scoreBar[0].fillAmount  - (playerKills[0]/killsToWin)) > .002f || Mathf.Abs( scoreBar[1].fillAmount  - (playerKills[1]/killsToWin)) > .002f 
			||Mathf.Abs( scoreBar[2].fillAmount  - (playerKills[2]/killsToWin)) > .002f ||Mathf.Abs( scoreBar[3].fillAmount  - (playerKills[3]/killsToWin)) > .002f ){

			for (int x = 0; x < 4; x++){
				if (Mathf.Abs( scoreBar[x].fillAmount  - (playerKills[x]/killsToWin)) > .002f){
					if (scoreBar [x].fillAmount < playerKills [x] / killsToWin) {
						scoreBar [x].fillAmount += .001f;
					}else if (scoreBar [x].fillAmount > playerKills [x] / killsToWin){
						scoreBar [x].fillAmount -= .001f;
					}
				}
			}
			yield return new WaitForSeconds(.015f);
			Debug.Log ("FILLING");
		}
	}
}
