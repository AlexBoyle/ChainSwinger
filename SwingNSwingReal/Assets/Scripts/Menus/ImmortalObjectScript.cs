using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	
	private int startLevel = 0;
	public int numPlayer = 0;
	private bool isPaused = false;
	public GameObject pauseMenu; 
	private GameObject visPauseMenu;
	bool boop = true;
	public GameObject[] Buttons;
	private CameraFollowScript CFS;
	public int BuildIndex = 1;
	private int pausePlayer = -1;
	// Use this for initialization
	void Start (){
		GameObject.DontDestroyOnLoad (gameObject);
		startLevel = SceneManager.GetActiveScene ().buildIndex;

	}
	// Update is called once per frame
	void OnLevelWasLoaded(int level) {
		visPauseMenu = Instantiate (pauseMenu,Vector3.zero,Quaternion.identity) as GameObject;
		visPauseMenu.SetActive (false);
		if (level == BuildIndex)
			GameObject.Find ("Main Camera").GetComponent<Maps> ().addPlayers (numPlayer);
		else if (level > BuildIndex) {
			RespawnScript a = GameObject.Find ("RespawnObject").GetComponent<RespawnScript> ();
			CFS = GameObject.Find ("Main Camera").GetComponent<CameraFollowScript>();
			for (int i = 0; i < numPlayer ; i++) {
				a.InitialSpawn (i);
			}
		}

	}
	public void numPlayers(int a){
		numPlayer = a;
	}

	public void pauseGame(int player){
		if (!isPaused) {
			Time.timeScale = 0f;
			visPauseMenu.SetActive (true);
			//visPauseMenu player con == player;
			isPaused = true;
			pausePlayer = player;

		} else if(pausePlayer == player) {
			Time.timeScale = 1f;
			visPauseMenu.SetActive (false);
			isPaused = false;
		}
	}

}
