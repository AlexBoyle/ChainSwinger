using UnityEngine;
using System.Collections;

public class SelfTurnOffScript : MonoBehaviour {
	public float delay;
	// Use this for initialization
	void Start () {
	}
	void OnEnable(){

		Invoke("TurnOff", delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void TurnOff(){
		gameObject.SetActive (false);
	}
}
