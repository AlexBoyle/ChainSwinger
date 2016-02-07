using UnityEngine;
using System.Collections;

// script use
// bounds the camera to be locked within the range specified by the top left right and bottom bound variables
// also lerpes the camera size to always show the two players that are taken in

public class CameraFollowScript : MonoBehaviour {
	Camera mainCamera;
	public Transform[] players;
	float cameraToUnits = 1.778114f;
	float lerpFactor = .025f;

	public float topBound;
	public float leftBound;
	public float rightBound;
	public float bottomBound;
	public float cameraMaxSize;
	public float cameraMinSize;
	// Use this for initialization
	void Start () {
		players = new Transform[4];
		mainCamera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		adjustCamera ();
	}
	void adjustCamera(){
		float currentDistance = Vector3.Distance (players [0].position, players [1].position);

		Vector3 midpoint = new Vector3((players [0].position.x + players[1].position.x)/2, (players [0].position.y +  players[1].position.y)/2, -100 );


		// adjust camera to be big enough to show both players
		float newSize = ((cameraToUnits  * currentDistance)/2) + 2;
		float yDistance = Mathf.Abs( players [0].position.y - players [1].position.y);

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
	public void addPlayer(Transform newPlayer, int pNum){
		players[pNum] = newPlayer;
	}
}
