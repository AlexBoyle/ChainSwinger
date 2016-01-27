using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour {
	ObjectPoolScript BloodPool;
	// Use this for initialization
	void Start () {
		BloodPool = GetComponent<ObjectPoolScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShootBlood(int amount){
		StartCoroutine (BloodAnim (amount));
	}

	IEnumerator BloodAnim(int amount){
		for (int x = 0; x < amount; x++) {
			GameObject tmp =  BloodPool.FetchObject ();
			tmp.transform.position = transform.position;
			tmp.transform.eulerAngles = new Vector3 (0, 0, Random.Range(0, 365));
			tmp.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (.3f, .7f), 0, 0, 1);
			tmp.transform.localScale = new Vector3 (Random.Range(.05f, .2f),Random.Range(.05f, .2f), 1) ;
			tmp.SetActive (true);
			tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range(-3, 3), Random.Range(0, 5), 0);
			yield return null;
		}
	}
}
