using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
	public Image[] scoreBar;
	public float killsToWin;
	public float[] playerKills;
	// Use this for initialization
	void Start () {
		playerKills = new float[4];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void IncrementKill(int playerNumber){
		playerKills[playerNumber]++;
		StartCoroutine (LerpBar());
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
