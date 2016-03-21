using UnityEngine;
using System.Collections;

// script use
// bounds the camera to be locked within the range specified by the top left right and bottom bound variables
// also lerpes the camera size to always show the two players that are taken in

public class CameraFollowScript : MonoBehaviour {
	Camera mainCamera;
	public Transform[] players;
	public Transform[] playerGhosts;
	float cameraToUnits = 1.778114f;
	float lerpFactor = .025f;

	int numberOfPlayers = 0, introBuff = 60;

	public float topBound;
	public float leftBound;
	public float rightBound;
	public float bottomBound;
	public float cameraMaxSize;
	public float cameraMinSize;
	public bool begining = true;
	// Use this for initialization
	void Start () {
		players = new Transform[4];
		playerGhosts = new Transform[4];
		mainCamera = GetComponent<Camera> ();
		if (begining){
			transform.position = new Vector3 (-30, 3.5f, -10);
			mainCamera.orthographicSize = 8.5f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (begining) {
			if (introBuff > 0) {
				introBuff--;
			} else {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (0, 3.5f, -10), .03f);
				if (transform.position.x > -.1f) {
					transform.position = new Vector3 (0, 3.5f, -10);
					begining = false;
				}
			}
		} else {
			if (numberOfPlayers == 2) {
				adjustCamera ();
			} else {
				mainCamera.orthographicSize = cameraMaxSize;
				mainCamera.transform.position = new Vector3 (0, 3.5f, -100);
			}
		}
	}
	void adjustCamera(){
		Vector3 p1Pos = players[0].position, p2Pos = players[1].position;
		if (!players [0].gameObject.activeSelf) {
			p1Pos = playerGhosts [0].position;
		}
		if (!players [1].gameObject.activeSelf) {
			p2Pos = playerGhosts [1].position;
		}
		float currentDistance = Vector3.Distance (p1Pos, p2Pos);

		Vector3 midpoint = new Vector3((p1Pos.x + p2Pos.x)/2, (p1Pos.y +  p2Pos.y)/2, -100 );


		// adjust camera to be big enough to show both players
		float newSize = ((cameraToUnits  * currentDistance)/2) + 2;
		float yDistance = Mathf.Abs( p1Pos.y - p2Pos.y);

		if (yDistance > newSize) {
			newSize = yDistance +1;
		}

		if (newSize < cameraMinSize) {
			newSize = cameraMinSize;
		} else if (newSize > cameraMaxSize) {
			newSize = cameraMaxSize;
		}
		mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize ,newSize, lerpFactor);

		// adjust camera to be in center of players

		float cameraBoundUp = midpoint.y + newSize;
		float cameraBoundDown = midpoint.y - newSize;
		// stop camera from leaving the ground
		if (cameraBoundUp > topBound) {
			midpoint.y = topBound - newSize;
		} else if (cameraBoundDown < bottomBound) {
			midpoint.y = bottomBound + newSize ;
		} 


		// control the x so that camera does show out of bounds
		float cameraBoundRight = midpoint.x +  (cameraToUnits * newSize);
		float cameraBoundLeft = midpoint.x -  (cameraToUnits * newSize);
		if (newSize == cameraMaxSize){
			midpoint.x = 0;
			midpoint.y = 3.5f;
		}
		else if ( cameraBoundRight >  rightBound){
			midpoint.x = rightBound - (cameraToUnits*newSize); 
		}else if (cameraBoundLeft < leftBound){
			midpoint.x = leftBound + (cameraToUnits*newSize);
		}  

		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, midpoint, lerpFactor );
	}
	public void addPlayer(Transform newPlayer, Transform newGhost,int pNum){
		players[pNum] = newPlayer;
		playerGhosts [pNum] = newGhost;
		numberOfPlayers++;
	}
}
