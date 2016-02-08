using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	private int startLevel = 0;
	private int numPlayer = 0;
	// Use this for initialization
	void Start (){
		GameObject.DontDestroyOnLoad (gameObject);
		startLevel = SceneManager.GetActiveScene ().buildIndex;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (startLevel != SceneManager.GetActiveScene ().buildIndex) {
			GameObject.Find ("Main Camera").GetComponent<Maps> ().addPlayers (numPlayer);
			GameObject.Destroy (gameObject);
		}
	}
	public void numPlayers(int a){
		numPlayer = a;
	}
}
