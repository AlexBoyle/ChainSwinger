﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
public class PressAScript : MonoBehaviour {
	private int playerNumber;
	private PlayerIndex playerIndex;
	public Sprite sp1;
	public Sprite sp2;
	public Sprite sp3;
	private GamePadState state;
	private GamePadState pre;
	private JoinGameCam cam;
	private bool playerjoin = false;
	private bool playerReady = false;
	private bool pressedB = false;
	private RespawnScript a;
	// Use this for initialization
	void Start () {
		// a = GameObject.Find ("RespawnObject").GetComponent<RespawnScript> ();
		playerIndex = (PlayerIndex)playerNumber;
		cam = GameObject.Find ("Main Camera").GetComponent<JoinGameCam> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
		if (state.Buttons.A == ButtonState.Pressed && !playerjoin) {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
			gameObject.GetComponent<SpriteRenderer> ().sprite = sp2;
			cam.playerJoin ();
			playerjoin = true;
		}
		if (playerjoin && state.Buttons.Start == ButtonState.Pressed && !playerReady) {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
			gameObject.GetComponent<SpriteRenderer> ().sprite = sp3;
			cam.playerReady (playerNumber);
			playerReady = true;
			//a.InitialSpawn (playerNumber);
		}
		if (state.Buttons.B == ButtonState.Released) {
			pressedB = false;
		}
		if (!pressedB) {
			if (state.Buttons.B == ButtonState.Pressed) {
				pressedB = true;
				if (playerReady) {
					playerReady = false;
					cam.playerNotReady (playerNumber);
					gameObject.GetComponent<SpriteRenderer> ().sprite = sp2;
					gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
				} else if (playerjoin) {
					playerjoin = false;
					cam.playerLeave ();
					gameObject.GetComponent<SpriteRenderer> ().sprite = sp1;
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