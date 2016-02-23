using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	public RespawnScript RS;
	// Use this for initialization
	void Start () {
		//Connect ();	
	}

	public void Connect() {
		PhotonNetwork.ConnectUsingSettings ("ChainSwinger 0.01");
	}
	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());

		if (GUI.Button (new Rect (220, 250, 100, 90), "Local Game")) {
			Debug.Log ("testgui");Debug.Log ("testgui");Debug.Log ("testgui");
		}

		if (GUI.Button (new Rect (420, 250, 100, 90), "Online Game")) {
			Connect ();
		}
	}
	void OnJoinedLobby(){
		Debug.Log ("onjoinedlobby");
		PhotonNetwork.JoinRandomRoom ();
	}
	void OnPhotonRandomJoinFailed(){
		Debug.Log ("onphotonrandomjoinfailed");
		PhotonNetwork.CreateRoom (null);
	}
	void OnJoinedRoom(){
		Debug.Log ("OnJoinedRoom");

		SpawnMyPlayer ();
	}
	void SpawnMyPlayer(){
		RS.InitialSpawn (0);

	}
}
