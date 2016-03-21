using UnityEngine;
using System.Collections;

public class ChainDestructionScript : MonoBehaviour {

	public PlayerControlScript PCS;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DestroyChain(Vector3 cutPosition ){
		PCS.BreakLine (false ,true, cutPosition);
	}
}
