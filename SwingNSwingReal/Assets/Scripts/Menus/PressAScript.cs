using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
public class PressAScript : MonoBehaviour {
	public int playerNumber;
	private PlayerIndex playerIndex;

	private GamePadState state;
	private GamePadState pre;
	private JoinGameCam cam;
	private bool playerjoin = false;
	private bool playerReady = false;
	private bool pressedB = false;
	// Use this for initialization
	void Start () {
		playerIndex = (PlayerIndex)playerNumber;
		cam = GameObject.Find ("Main Camera").GetComponent<JoinGameCam> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
		if (state.Buttons.A == ButtonState.Pressed && !playerjoin) {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			cam.playerJoin ();
			playerjoin = true;
		}
		if (playerjoin && state.Buttons.Start == ButtonState.Pressed && !playerReady) {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
			cam.playerReady ();
			playerReady = true;
		}
		if (state.Buttons.B == ButtonState.Released) {
			pressedB = false;
		}
		if (!pressedB) {
			if (state.Buttons.B == ButtonState.Pressed) {
				pressedB = true;
				if (playerReady) {
					playerReady = false;
					cam.playerNotReady ();
					gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
				} else if (playerjoin) {
					playerjoin = false;
					cam.playerLeave ();
					gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				}
			}
		}
		pre = state;
	}
	public void setPlayerNumber(int a){
		playerNumber = a;
		playerIndex = (PlayerIndex)playerNumber;
	}
}