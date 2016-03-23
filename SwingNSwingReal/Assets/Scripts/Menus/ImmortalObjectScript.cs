using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	//temp
	private float time;
	private float temp;
	private GamePadState state;
	/// ////

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
		visPauseMenu.transform.position = Vector3.zero;
		visPauseMenu.SetActive (false);
		pauseGame (0);
		//visPauseMenu.SetActive (false);
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
			Debug.Log ("here");
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

	//this update function is temporaty for the pause menu
	void Update () {
		time = Time.realtimeSinceStartup;
		if (temp + .1f < time ) {
			//Debug.Log (time);
			for(int i = 0; i < 4; i ++){
				state = GamePad.GetState ((PlayerIndex)i, GamePadDeadZone.None);
				if (state.Buttons.Start == ButtonState.Pressed)
					pauseGame (0);
			}

			temp = time;
		}

}
}
