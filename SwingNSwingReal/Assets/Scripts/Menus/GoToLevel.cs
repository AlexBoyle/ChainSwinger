using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour {

	public static void goTolvl(int lvl){
		SceneManager.LoadScene(lvl);
	}
}
