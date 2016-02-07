using UnityEngine;
using System.Collections;

// force obect to also have the object pool
[RequireComponent (typeof (ObjectPoolScript))]
public class ParticleEmitterScript : MonoBehaviour {
	 ObjectPoolScript particlePool;

	public bool onEnableShootParticles = false;
	public bool loopParticles = false;
	public int emitAmount;
	public float emitDuration;
	// spawn locaiton variable


	public bool useRandomPosition;
	public Vector3 positionMin, positionMax;
	

	// force variables
	public bool useForces = false;
	public Vector3 forceMin, forceMax;

	// rotation variables
	public bool useRandomRotation = false;
	public float rotationMin, rotationMax;

	// scale variables
	public bool useRandomScale = false;
	public Vector3 scaleMin, scaleMax;

	// color variables
	public bool useRandomColor;
	public Color rColor1, rColor2;



	public bool useInitialBurst = false;
	public int initialBurstAmount = 0;
	// Use this for initialization
	void Awake () {
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
				EmitParticles (emitAmount, emitDuration);
			}
		}
	}
	//-------------------------Loop particle Functions ------------------------
	// wrapper for a looped particle system
	public void startParticlesLoop(float amount = -1){
		
		// check for coded value and use emitamount as the new default
		if (amount == -1) {
			amount = emitAmount;
		}

		// check for initial burst
		if (useInitialBurst) {
			EmitBurst (initialBurstAmount);
		}

		// start up the particles
		StartCoroutine (EmitLoop (emitDuration/amount));
	}
	public void StopParticleLoop(){
		StopCoroutine ("EmitLoop");
	}
	// continuously emit particles
	IEnumerator EmitLoop(float particlesPerSecond){		
		while (true) {
			EmitSingleParticle ();
			yield return new WaitForSeconds(particlesPerSecond);
		}
	}

	// ---------------------------regular emit functions ------------------------------
	// wrapper function for emiting constant particles
	public void EmitParticles(int amount, float duration = 1, float forceRatio = 1,  float scaleRatio = 1){
		if (useInitialBurst) {
			EmitBurst (initialBurstAmount);
		}
		StartCoroutine (EmitOverTime (amount, duration, forceRatio, scaleRatio));
	}

	// emit amount of particles over time
	IEnumerator EmitOverTime(int amount, float duration, float forceRatio = 1,  float scaleRatio = 1){
		int amountDec = amount;
		float particlesPerFrame;
		if (duration > 0) {
			particlesPerFrame = (amount * .0166f) / duration;
		} else {
			particlesPerFrame = amount;
		}

		Debug.Log (particlesPerFrame);
		while (amountDec > 0) {
			for (int i = 0; i < particlesPerFrame; i++) {
				EmitSingleParticle (forceRatio, scaleRatio);
				amountDec--;
			}
			yield return new WaitForSeconds(duration/amount);
		}
	}


	// emites the passed in amount of particles as fast as possible
	public void EmitBurst(int amount){
		for (int i = 0; i < amount; i++){
			EmitSingleParticle ();
		}
	}

	// shoots out a single particle according to the paramters set in the class
	void EmitSingleParticle(float forceRatio = 1, float scaleRatio = 1){
		// get particle from pool
		GameObject tmp =  particlePool.FetchObject ();

		// put particle on systems position or in a random area around it
		if (useRandomPosition) {
			Vector3 tmpPos =transform.position;
			tmpPos.x += Random.Range (positionMin.x, positionMax.x);
			tmpPos.y += Random.Range (positionMin.y, positionMax.y);
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
			tmp.transform.localScale = new Vector3 (Random.Range (scaleMin.x * scaleRatio, scaleMax.x * scaleRatio), Random.Range (scaleMin.y * scaleRatio, scaleMax.y * scaleRatio), 1);
		}

		// set object active
		tmp.SetActive (true);

		// handle particle forces
		if (useForces) {
			tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (forceMin.x * forceRatio, forceMax.x * forceRatio), Random.Range (forceMin.y * forceRatio, forceMax.y * forceRatio), 0);
		}
	}

	// ------------------- misc ---------------------
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
