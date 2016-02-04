using UnityEngine;
using System.Collections;

public class GrapplePointEffectScript : MonoBehaviour {
	public ObjectPoolScript SparksScript;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		ShootBlood (5);
	}
	public void ShootBlood(int amount){
		StartCoroutine (SparksAnim (amount));
	}

	IEnumerator SparksAnim(int amount){
		for (int x = 0; x < amount; x++) {
			for (int i = 0; i < 3; i++){
				GameObject tmp =  SparksScript.FetchObject ();
				tmp.transform.position = transform.position;
				tmp.transform.eulerAngles = new Vector3 (0, 0, Random.Range(0, 365));
				tmp.GetComponent<SpriteRenderer> ().color = new Color (1, Random.Range(.6f, .9f), .3f, 1);
				tmp.transform.localScale = new Vector3 (Random.Range(.05f, .15f),Random.Range(.05f, .15f), 1) ;
				tmp.SetActive (true);
				tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range(-1.5f, 1.5f), Random.Range(0, 1), 0);
			}
			yield return null;
		}
	}
}
