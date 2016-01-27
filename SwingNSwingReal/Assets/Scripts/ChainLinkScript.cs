using UnityEngine;
using System.Collections;

public class ChainLinkScript : MonoBehaviour {
	public HingeJoint2D HJ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DelinkChain(float delay = 0){
		StartCoroutine (DelinkDelay (delay));

	}
	IEnumerator DelinkDelay(float delay){
		yield return new WaitForSeconds (delay);
		if (HJ.connectedBody != null){
			//HJ.connectedBody.GetComponent<ChainLinkScript> ().DelinkChain ();
		}
		HJ.enabled = false;
	}
}
