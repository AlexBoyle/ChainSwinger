using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	private int startLevel = 0;
	private int numPlayer = 0;
	bool boop = true;
	// Use this for initialization
	void Start (){
		GameObject.DontDestroyOnLoad (gameObject);
		startLevel = SceneManager.GetActiveScene ().buildIndex;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (startLevel != SceneManager.GetActiveScene ().buildIndex) {
			if (boop) {
				GameObject.Find ("Main Camera").GetComponent<Maps> ().addPlayers (numPlayer);
				boop = false;
			}
		}
	}
	public void numPlayers(int a){
		numPlayer = a;
	}
}
