using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectPoolScript : MonoBehaviour {
	public GameObject PoolObject;
	public List<GameObject> Pool;
	// Use this for initialization
	void Start () {
		//initialize list object
		Pool = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () {
	
	}
	public GameObject FetchObject(){

		for (int x = 0; x < Pool.Count; x++) {
			if (Pool[x] != null){
				// check if current element is enabled
				if (Pool[x].activeSelf == false){
					// if not return it
					return Pool[x];
				}
			}
		}
		// else we create a new object and add it to the list and return that
		Pool.Add( Instantiate(PoolObject, Vector3.zero, Quaternion.identity) as GameObject);
		Pool [Pool.Count - 1].SetActive (false);
		return Pool[Pool.Count-1];
	}
}
