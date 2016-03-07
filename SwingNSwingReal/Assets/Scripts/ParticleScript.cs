using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {
	public bool scaleModification = false;
	public Vector3 scaleSpeed = Vector3.zero;
	public bool simulateGravity = false;
	public float gravitySpeed = 0, currentGravityAmount = 0;
	public float xMovement = 0, yMovement = 0;
	SpriteRenderer SR;
	// Use this for initialization
	void Awake(){
		SR = GetComponent<SpriteRenderer> ();
	}
		
	
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (scaleModification && (transform.localScale.x > .01f || transform.localScale.y > .01f)) {
			transform.localScale += scaleSpeed;
		}
		if (simulateGravity){
			if (currentGravityAmount < 1f) {
				currentGravityAmount += gravitySpeed;
			}
			transform.position = new Vector3 (transform.position.x, transform.position.y - currentGravityAmount, transform.position.z);
		}
		transform.Translate(new Vector3(xMovement, yMovement, 0));
	}

	IEnumerator TimedDisable(float delay){
		yield return new WaitForSeconds (delay);
		gameObject.SetActive (false);
	}
	public void InitialSetup(float lifeDelay, Vector3 forces, Color newColor, bool newScaleModification = false, Vector3 newScaleSpeed = default(Vector3), bool newSimulateGravity = false, float newGravitySpeed = 0){
		xMovement = forces.x;
		yMovement = forces.y;
		SR.color = newColor;
		scaleModification = newScaleModification;
		scaleSpeed = newScaleSpeed;
		simulateGravity = newSimulateGravity;
		gravitySpeed = newGravitySpeed;
		currentGravityAmount = 0;
		StartCoroutine (TimedDisable(lifeDelay));
	}
}
