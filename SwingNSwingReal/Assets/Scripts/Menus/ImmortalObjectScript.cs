using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ImmortalObjectScript : MonoBehaviour {
	private int startLevel = 0;
	private int numPlayer = 0;
	bool boop = true;
	public int BuildIndex = 1;
	// Use this for initialization
	void Start (){
		GameObject.DontDestroyOnLoad (gameObject);
		startLevel = SceneManager.GetActiveScene ().buildIndex;
	}
	// Update is called once per frame
	void OnLevelWasLoaded(int level) {
		Debug.Log (level);
		if (level == BuildIndex)
			GameObject.Find ("Main Camera").GetComponent<Maps> ().addPlayers (numPlayer);

	}
	public void numPlayers(int a){
		numPlayer = a;
	}
}
