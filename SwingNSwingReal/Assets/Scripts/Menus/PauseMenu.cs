using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	private int plyNum = 0;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private GamePadState prestate;
	private float time;
	private float temp;
	public GameObject but1;
	public GameObject but2;
	public GameObject but3;
	public int pos = 0;
	private bool first = true;
	// Use this for initialization
	void Start () {
		playerIndex = (PlayerIndex)0;

	}
	public void setPlayer(int a){
		plyNum = a;
		playerIndex = (PlayerIndex)a;
		time = Time.realtimeSinceStartup;
		temp = time;
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
	}

	// Update is called once per frame
	void Update () {
		time = Time.realtimeSinceStartup;
		if (temp + .1f < time ) {
			//Debug.Log (time);
			state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
			//Debug.Log ("Current: " + state.Buttons.Start + " || Past: " + prestate.Buttons.Start);
			if (state.Buttons.Start == ButtonState.Pressed && prestate.Buttons.Start == ButtonState.Released ) {
				GameObject.Find ("ImmortalObject").GetComponent<ImmortalObjectScript> ().pauseGame (plyNum);
			}
			if (state.ThumbSticks.Left.Y < -.2f )
			if (pos < 2)
				pos++;
			if (state.ThumbSticks.Left.Y > .2f)
			if (pos > 0)
				pos--;
			switch (pos) {
			case 0:
				but1.GetComponent<SpriteRenderer> ().color = Color.red;
				but2.GetComponent<SpriteRenderer> ().color = Color.white;
				but3.GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			case 1:
				but1.GetComponent<SpriteRenderer> ().color = Color.white;
				but2.GetComponent<SpriteRenderer> ().color = Color.red;
				but3.GetComponent<SpriteRenderer> ().color = Color.white;
				break;
			case 2:
				but1.GetComponent<SpriteRenderer> ().color = Color.white;
				but2.GetComponent<SpriteRenderer> ().color = Color.white;
				but3.GetComponent<SpriteRenderer> ().color = Color.red;
				break;

			}
			prestate = state;
			temp = time;
		}

	}
}
