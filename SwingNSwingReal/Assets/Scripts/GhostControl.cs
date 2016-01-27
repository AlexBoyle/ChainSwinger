using UnityEngine;
using System.Collections;

using XInputDotNetPure; // Required in C#
public class GhostControl : MonoBehaviour {
	public Rigidbody2D RB;
	public PlayerControlScript PCS;

	public float flySpeed;
	float deadSize = .25f, x, y;
	bool inputEnabled = true;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	// Use this for initialization
	void Start () {
		playerIndex = (PlayerIndex)PCS.GetPlayerNumber ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (inputEnabled){
			Debug.Log ("hello");
			prevState = state;
			state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
			// axis crontrols for horizontal movement
			if (Mathf.Abs (state.ThumbSticks.Left.X) > deadSize) {
				x = state.ThumbSticks.Left.X;
			} else {
				x = 0;
			}
			if (Mathf.Abs (state.ThumbSticks.Left.Y) > deadSize) {
				y = state.ThumbSticks.Left.Y;
			} else {
				y = 0;
			}

			RB.velocity = new Vector3 (x * flySpeed, y * flySpeed, 0);
		}
	}

}
