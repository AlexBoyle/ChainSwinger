using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class InputScript : MonoBehaviour {
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	int playerNumber;
	PlayerControlScript PCS;
	bool gameOver = false;
	// Use this for initialization
	void Start () {
		PCS = GetComponent<PlayerControlScript> ();
		// for use in local mode, gets palyer number from the pcs
		playerIndex = (PlayerIndex)PCS.GetPlayerNumber();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameOver) {
			prevState = state;
			state = GamePad.GetState (playerIndex, GamePadDeadZone.None);

			PCS.MovementControl (state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);

			// Detect if a button was pressed this frame
			if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) {
				PCS.Jump ();
			}

			// Detect if a button was pressed this frame
			if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed) {
				PCS.SwordThrowPress ();	
			}
			// Detect if a button was released this frame
			if (prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released) {
				PCS.SwordThrowRelease ();
			}
			// x button press
			if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released) {
				PCS.StartSwingAttack ();
			}
			// y held
			if (state.Buttons.LeftShoulder == ButtonState.Pressed) {
				PCS.SwordThrowHold ();
			}
			// trigger press
			if (prevState.Triggers.Right <= .5f && state.Triggers.Right > .5f) {
				PCS.ChainSwingPress ();
			}
			// trigger release
			if (prevState.Triggers.Right > .5f && state.Triggers.Right <= .5f) {
				PCS.BreakLine (true);
			}
			// trigger press
			if (state.Triggers.Left > .5f) {
				//PCS.ReelInChain ();
			}
		}

	}
	public void GameOver(){
		gameOver = true;
	}
}
