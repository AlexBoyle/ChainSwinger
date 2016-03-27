using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	//temp
	private float time;
	private float temp;
	private GamePadState state;
	private GamePadState prestate;
	/// ////
	/// 
	/// 
	private bool[] players = {false,false,false,false};
	private bool spawn = false;
	private  int curlvl = 0;
	private int startLevel = 0;
	private int count = 0;

	private bool isPaused = false;
	public GameObject pauseMenu; 
	private GameObject visPauseMenu;
	bool boop = true;
	public GameObject[] Buttons;

	public int BuildIndex = 1;
	private int pausePlayer = -1;
	// Use this for initialization
	void Start (){
		GameObject.DontDestroyOnLoad (gameObject);
		startLevel = SceneManager.GetActiveScene ().buildIndex;

	}
	// Update is called once per frame
	void OnLevelWasLoaded(int level) {
		curlvl = level;
		visPauseMenu = Instantiate (pauseMenu,Vector3.zero,Quaternion.identity) as GameObject;
		visPauseMenu.transform.position = Vector3.zero;
		visPauseMenu.SetActive (false);

		//visPauseMenu.SetActive (false);
		if (level == BuildIndex)
			GameObject.Find ("Main Camera").GetComponent<Maps> ().addPlayers (players);
		else if (level > BuildIndex) {
			spawn = true;
			count = 0;
		}

	}
	public void numPlayers(bool[] a){
		players = a;
	}

	public void pauseGame(int player){
		if(curlvl > BuildIndex)	
		if (!isPaused) {
			//Debug.Log ("here");
			visPauseMenu.SetActive (true);
			visPauseMenu.GetComponent<PauseMenu> ().setPlayer (player);
			isPaused = true;
			pausePlayer = player;
			Time.timeScale = 0f;
		} else if(pausePlayer == player) {
			
			visPauseMenu.SetActive (false);
			isPaused = false;
			Time.timeScale = 1f;
		}
	}


	void FixedUpdate(){
		//put this in an update loop to prevent forver pausing
		for(int i = 0; i < 4; i ++){
			state = GamePad.GetState ((PlayerIndex)i, GamePadDeadZone.None);
			if (state.Buttons.Start == ButtonState.Pressed && prestate.Buttons.Start == ButtonState.Released && !isPaused) {
				prestate = state;
				pauseGame (i);
			}
		}
		if (spawn) {
			count++;
			if (count > 200) {
				RespawnScript a = GameObject.Find ("RespawnObject").GetComponent<RespawnScript> ();

				for (int i = 0; i < 4 ; i++) {
					if (players [i]) {
						Debug.Log ("adding player: " + i);
						a.InitialSpawn (i);
					}
				}
				spawn = false;
			}
		}
		prestate = state;
	}


}

