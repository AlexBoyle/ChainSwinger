using UnityEngine;
using System.Collections;

public class SeflDestroyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("DestroySelf", 3f);
	}
	void DestroySelf(){
		Destroy (gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
