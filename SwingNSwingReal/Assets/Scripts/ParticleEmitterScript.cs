using UnityEngine;
using System.Collections;

// force obect to also have the object pool
[RequireComponent (typeof (ObjectPoolScript))]
public class ParticleEmitterScript : MonoBehaviour {
	public ObjectPoolScript particlePool;

	public bool onEnableShootParticles = false;
	public bool loopParticles = false;
	public int emitAmount;
	public float emitDuration;
	// spawn locaiton variable


	public bool useRandomPosition;
	public float locationXMin, locationXMax, locationYMin, locationYMax;
	

	// force variables
	public bool useForces = false;
	public float forceRangeXMin, forceRangeXMax, forceRangeYMin, forceRangeYMax;

	// rotation variables
	public bool useRandomRotation = false;
	public float rotationMin, rotationMax;

	// scale variables
	public bool useRandomScale = false;
	public float scaleRangeXMin, scaleRangeXMax, scaleRangeYMin, scaleRangeYMax;

	// color variables
	public bool useRandomColor;
	public Color rColor1, rColor2;



	public bool useInitialBurst = false;
	public int initialBurstAmount = 0;
	// Use this for initialization
	void Start () {
		particlePool=  GetComponent<ObjectPoolScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		// test function
		//if (Input.GetKeyDown (KeyCode.Space)) {
		//	EmitParticles (10);
		//}
	}
	void OnEnable(){
		if (onEnableShootParticles) {
			if (loopParticles) {
				startParticlesLoop ();
			} else {
				EmitParticles (emitAmount);
			}
		}
	}

	// wrapper for a looped particle system
	public void startParticlesLoop(int amount = -1){
		// check for coded value and use emitamount as the new default
		if (amount == -1) {
			amount = emitAmount;
		}

		// check for initial burst
		if (useInitialBurst) {
			EmitBurst (initialBurstAmount);
		}

		// start up the particles
		StartCoroutine ("loopedParticles", EmitOverTime (0));
	}
	public void StopParticleLoop(){
		StopCoroutine ("loopedParticles");
	}

	// wrapper function for emiting constant particles
	public void EmitParticles(int amount, float duration = 0){
		
		StartCoroutine (EmitOverTime (amount));
	}

	// emit amount of particles over time
	IEnumerator EmitOverTime(int amount ){		
		for (int x = 0; x < amount; x++) {
			EmitSingleParticle ();
			yield return null;
		}
	}

	// continuously emit particles
	IEnumerator EmitLoop(){		
		while (true) {
			EmitSingleParticle ();
			yield return null;
		}
	}
	// emites the passed in amount of particles as fast as possible
	void EmitBurst(int amount){
		for (int i = 0; i < amount; i++){
			EmitSingleParticle ();
		}
	}

	// shoots out a single particle according to the paramters set in the class
	void EmitSingleParticle(){
		// get particle from pool
		GameObject tmp =  particlePool.FetchObject ();

		// put particle on systems position or in a random area around it
		if (useRandomPosition) {
			Vector3 tmpPos =transform.position;
			tmpPos.x += Random.Range (locationXMin, locationXMax);
			tmpPos.y += Random.Range (locationYMin, locationYMax);
			tmp.transform.position = tmpPos;
		} else {					
			tmp.transform.position = transform.position;
		}

		// handle particle rotation
		if (useRandomRotation) {
			tmp.transform.eulerAngles = new Vector3 (0, 0, Random.Range (rotationMin, rotationMax));
		}

		// handle color
		if (useRandomColor) {
			tmp.GetComponent<SpriteRenderer> ().color = RandomBetweenTwoColors ();
		}

		// handle scale
		if (useRandomScale) {
			tmp.transform.localScale = new Vector3 (Random.Range (scaleRangeXMin, scaleRangeXMax), Random.Range (scaleRangeYMin, scaleRangeYMax), 1);
		}

		// set object active
		tmp.SetActive (true);

		// handle particle forces
		if (useForces) {
			tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (forceRangeXMin, forceRangeXMax), Random.Range (forceRangeYMin, forceRangeYMax), 0);
		}
	}

	// uses rcolor1 and 2 and randoms a new color betweent them
	Color RandomBetweenTwoColors(){
		Color randomColor;
		// handle red 
		if (rColor1.r > rColor2.r) {
			randomColor.r = Random.Range (rColor2.r, rColor1.r);
		} else {
			randomColor.r = Random.Range (rColor1.r, rColor2.r);
		}
		// handle green 
		if (rColor1.r > rColor2.r) {
			randomColor.g = Random.Range (rColor2.g, rColor1.g);
		} else {
			randomColor.g = Random.Range (rColor1.g, rColor2.g);
		}
		// handle blue 
		if (rColor1.r > rColor2.r) {
			randomColor.b = Random.Range (rColor2.b, rColor1.b);
		} else {
			randomColor.b = Random.Range (rColor1.b, rColor2.b);
		}

		randomColor.a = 1;
		return randomColor;
	}
}
