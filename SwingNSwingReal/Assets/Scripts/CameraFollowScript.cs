using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {
	Camera mainCamera;
	public Transform[] players;
	float cameraToUnits = 1.778114f;
	float lerpFactor = .2f;
	public float LeftBound;
	public float RightBound;
	public float BottomBound;
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
		float newSize = (5f / 12f) * currentDistance;
		float yDistance = Mathf.Abs( players [0].position.y - players [1].position.y);
		if (yDistance > newSize) {
			newSize = yDistance +1;
		}

		if (newSize < 5f) {
			newSize = 5f;
		} else if (newSize > 8.5) {
			newSize = 8.5f;
		}
		mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize ,newSize, lerpFactor);

		// adjust camera to be in center of players
		// stop camera from leaving the ground
		if (midpoint.y < newSize) {
			midpoint.y=  newSize - 5;
		}

		// control the x so that camera does show out of bounds
		float cameraBoundRight = midpoint.x +  (cameraToUnits * newSize);
		float cameraBoundLeft = midpoint.x -  (cameraToUnits * newSize);
		if (newSize == 8.5f){
			midpoint.x = 0;
		}
		else if ( cameraBoundRight >  15){
			midpoint.x = 15f - (cameraToUnits*newSize); 
		}else if (cameraBoundLeft < -15){
			midpoint.x = -15 + (cameraToUnits*newSize);
		}  

		mainCamera.transform.position =  Vector3.Lerp(mainCamera.transform.position, midpoint, lerpFactor );
	}
	public void addPlayer(Transform newPlayer, int pNum){
		players[pNum] = newPlayer;
	}
}
