﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure; 
public class MapSelector : MonoBehaviour {
	public int playerNumber;
	private int playerX = 0;
	private int playerY = 0;
	private float moveX;
	private float moveY;
	private int mapX;
	private int mapY;
	public int moveCoolDown = 30;
	private int moveCoolDownVar = 0;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private Maps mapDim;

	// Use this for initialization
	void Start () {
		playerIndex = (PlayerIndex)playerNumber;
		mapDim = GameObject.Find ("Main Camera").GetComponent<Maps>();
		mapX = mapDim.getWidth ();
		mapY = mapDim.getHeight ();
		moveX = mapDim.getMoveX();
		moveY = mapDim.getMoveY();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);
		if (moveCoolDownVar < 0) {
			moveCoolDownVar = moveCoolDown;
			//
			if (state.ThumbSticks.Left.X > .4f) {
				if (mapX -1 > playerX)
					playerX ++;
			} else if (state.ThumbSticks.Left.X < -.4f) {
				if(0 < playerX)
					playerX --;
			}
			if (state.ThumbSticks.Left.Y < -.4f) {
				if (mapY -1 > playerY)//idk this wokrs now
					playerY ++;
			} else if (state.ThumbSticks.Left.Y > .4f) {
				if(0 < playerY)
					playerY --;
			}
		}
		gameObject.GetComponent<Transform>().position = Vector3.Lerp(gameObject.GetComponent<Transform>().position, new Vector3((playerX * moveX)-10.8f +(playerNumber * .9f),(playerY * -moveY) + 1,0), .2f);
		gameObject.GetComponent<Transform>().Rotate(new Vector3(0,0,playerNumber + 2));
		moveCoolDownVar--;
	}
	public void setPlayerNum(int a)
	{
		playerNumber = a;
		playerIndex = (PlayerIndex)playerNumber;
	}
}
