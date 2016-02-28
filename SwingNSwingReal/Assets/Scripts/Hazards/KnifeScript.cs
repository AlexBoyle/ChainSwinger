using UnityEngine;
using System.Collections;

public class KnifeScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (other.name);
		if(!other.name.Contains("Wall"))
			gameObject.GetComponentInParent<knifechain>(). go();
	}
}
