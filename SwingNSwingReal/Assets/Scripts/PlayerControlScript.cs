using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	public Rigidbody2D RB;
	public float groundSpeed, jumpHeight, DashSpeed;
	public GameObject ghost, grappleAnchor, ChainHixbox, swingEffect, chargeSprite, chargeParticles;
	public Rigidbody2D thrownSword;
	public int playerNumber;
	public bool BlockReelIn = false;
	float xStick, yStick, deadSize = .25f;
	bool movementInputEnabled = true, doubleJump = false, grounded = false, onWallRight = false, onWallLeft = false, leftRightEnabled = true, swinging = false, facingRight, dash = false, 
	canAttack = true, swingEnabled = true, gameOver = false, chainAnimAllowed = true, hasSword = true, isChargingThrow = false, fullyCharged = false;
	ScoreScript SS;
	public Sprite[] SwordAnimations;
	InputScript IS;
	public float platformSpeed;

	public ObjectPoolScript chainLinkPool;

	int groundMask, playerGroundMask;
	int groundedBuffer = 0, wallBuffer = 0, wallEjectBuffer = 0;
	Vector2 swingPoint, grappleDirection;
	Vector3 prevPosition, swordThrowAngle;
	float SwingRadius, throwTimerStart;
	public LineRenderer LR;
	public SpriteRenderer swordColor;
	SpriteRenderer SR;
	//ObjectPoolScript SwingEffectPool;


	// Use this for initializationswing
	void Start () {
		groundMask = 1 << 8;
		playerGroundMask = 1 << 9; // maybe nine maybe just a number

		IS = GetComponent<InputScript> ();
		SR = GetComponent<SpriteRenderer> ();
		SS = GameObject.Find ("ScoreObject").GetComponent<ScoreScript>();
		SS.AddPlayer (gameObject);

		//SwingEffectPool = GameObject.Find ("LinePooler").GetComponent<ObjectPoolScript> ();
	}

	void OnEnable(){
		grounded = false;
		onWallLeft = false;
		onWallRight = false;
		leftRightEnabled = true;
		movementInputEnabled = true;
		swinging = false;
		swingEnabled = true;
		chainAnimAllowed = true;
		isChargingThrow = false;
		canAttack = true;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameOver) {
			

			// do a check for ground
			RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, .35f, groundMask);
			if (groundCheck.collider != null && groundCheck.collider.tag == "MovingPlatform"){
				Debug.Log ("ding");
				platformSpeed = groundCheck.collider.GetComponent<Rigidbody2D> ().velocity.x;	
			}else{
				platformSpeed = 0;
			}
			if (groundCheck.collider != null && groundedBuffer <= 0) {
				grounded = true;
				doubleJump = true;
				dash = true;
			} else if (groundCheck.collider == null) {
				grounded = false;
			} else if (groundedBuffer > 0) {
				groundedBuffer--;
			}

			// block for checking ground
			if (!grounded) {
			

				// do a check for a wall hang
				RaycastHit2D wallCheckRightTop = Physics2D.Raycast (transform.position + new Vector3 (0, .2f, 0), Vector2.right, .35f, groundMask);
				RaycastHit2D wallCheckRightBottom = Physics2D.Raycast (transform.position - new Vector3 (0, .2f, 0), Vector2.right, .35f, groundMask);
				RaycastHit2D wallCheckLeftTop = Physics2D.Raycast (transform.position + new Vector3 (0, .2f, 0), Vector2.left, .35f, groundMask);
				RaycastHit2D wallCheckLeftBottom = Physics2D.Raycast (transform.position - new Vector3 (0, .2f, 0), Vector2.left, .35f, groundMask);
				if ((wallCheckRightTop.collider != null || wallCheckRightBottom.collider != null) && wallBuffer <= 0) {
					onWallRight = true;
					doubleJump = true;
					if (swinging) {
						BreakLine ();
					}
				} else if ((wallCheckLeftTop.collider != null || wallCheckLeftBottom.collider != null) && wallBuffer <= 0) {				 
					onWallLeft = true;
					doubleJump = true;
					if (swinging) {
						BreakLine ();
					}
				} else if (wallBuffer > 0) {
					wallBuffer--;
				} else if (wallCheckRightTop.collider == null && wallCheckRightBottom.collider == null && wallCheckLeftTop.collider == null && wallCheckLeftBottom.collider == null && (onWallLeft || onWallRight)) {
					onWallLeft = false;
					onWallRight = false;
				}
			} else {
				onWallRight = false;
				onWallLeft = false;
			}


			UpdateChain ();
			

			prevPosition = transform.position;
		}
	}

	// ----------------- // section of controls for swinging chain \\ ----------------- \\
	void UpdateChain(bool firstFrame = false){
		// change up velocity based on swinging state
		if (swinging) {
			swingPoint = grappleAnchor.transform.position;
			CheckLineBreaks ();


			ReelInChain ();
			LineGraphicsUpdate ();
			if (Vector2.Distance (transform.position, swingPoint) > SwingRadius) {

				transform.position = Vector2.MoveTowards (transform.position, swingPoint, Vector2.Distance (transform.position, swingPoint) - SwingRadius);
				RB.velocity = ((transform.position - prevPosition) / Time.deltaTime);
				if (firstFrame) {
					RB.velocity = RB.velocity * 25;
					/*if (facingRight && RB.velocity.x < 10) {
						RB.velocity = new Vector3(10, RB.velocity.y, 0);
					} else if (RB.velocity.x > -10){
						RB.velocity = new Vector3(-10, RB.velocity.y, 0);
					}*/
				}
			}
		}
	}
	// seperate button for reeling in the line
	public void ReelInChain(){
		if (swinging) {
			// reel in line
			if (SwingRadius > 2 && !BlockReelIn) {
				SwingRadius -= .05f;
			}
		}
	}
	// handles inital button press for swinging 
	public void ChainSwingPress(){
		if (!onWallLeft && !onWallRight && swingEnabled) {
			if (grounded) {
				grappleDirection = new Vector2 (0, 1);
			} else if (facingRight) {
				grappleDirection = new Vector2 (1, 1);
			} else {
				grappleDirection = new Vector2 (-1, 1);
			}

			RaycastHit2D swingHit = Physics2D.Raycast (transform.position, grappleDirection, Mathf.Infinity, groundMask);
			swingPoint = swingHit.point;
			SwingRadius = swingHit.distance;
			grappleAnchor.transform.position = swingPoint;
			grappleAnchor.transform.parent = swingHit.collider.gameObject.transform;
			grappleAnchor.SetActive (true);
			LR.enabled = true;
			ChainHixbox.SetActive (true);
			StartCoroutine (SwingingCooldown ());
			swinging = true;
			leftRightEnabled = false;
			LineGraphicsUpdate ();
			//UpdateChain (true);
		}
	}
	void LineGraphicsUpdate(){
		ChainHixbox.transform.localScale = new Vector3(Vector2.Distance (transform.position, swingPoint), 1 , 1);
		ChainHixbox.transform.position = Vector3.MoveTowards (transform.position, swingPoint, Vector2.Distance (transform.position, swingPoint) / 2); 
		Vector2 diff = transform.position - new Vector3(swingPoint.x, swingPoint.y, 0);
		ChainHixbox.transform.rotation = Quaternion.Euler(0, 0,90 - Mathf.Atan2 (diff.x, diff.y) * Mathf.Rad2Deg);
		LR.SetPosition (0, transform.position);
		LR.SetPosition (1, swingPoint);
		LR.material.mainTextureScale = new Vector2(Vector2.Distance (transform.position, swingPoint),1);
	}

	public void BreakLine( bool usePosition = false, Vector3 cutPosition = default(Vector3)){
		if (swinging) {
			grappleAnchor.SetActive (false);
			ChainHixbox.SetActive (false);
			LR.enabled = false;
			swinging = false;
			leftRightEnabled = true;
			grappleAnchor.transform.parent = null;
			AnimateLineBreak (usePosition, cutPosition);


		}
	}

	void AnimateLineBreak( bool usePosition = false, Vector3 cutPosition = default(Vector3), Vector3 newPosition = default(Vector3)){
		if (chainAnimAllowed) {
			chainAnimAllowed = false;
			float x = 0;
			Vector2 diff = transform.position - new Vector3 (swingPoint.x, swingPoint.y, 0);
			Quaternion rot = Quaternion.Euler (0, 0, 180 - Mathf.Atan2 (diff.x, diff.y) * Mathf.Rad2Deg);
			Rigidbody2D lastLink = null;
			bool first = true;
			float cutPoint = 0;
			if (usePosition) {
				cutPoint = Vector3.Distance (swingPoint, cutPosition);
			}
			float lineLength = Vector2.Distance (transform.position, swingPoint);
			if (newPosition != Vector3.zero){
				lineLength -= Vector2.Distance (transform.position, newPosition);
			}
			while (x < lineLength) {

				GameObject tmp = chainLinkPool.FetchObject ();
				tmp.transform.position = Vector3.MoveTowards (swingPoint, transform.position, x);
				tmp.transform.rotation = rot;
				tmp.GetComponent<HingeJoint2D> ().connectedBody = null;
				tmp.GetComponent<HingeJoint2D> ().enabled = true;

				if (first) {

					tmp.SetActive (true);

					tmp.GetComponent<SelfTurnOffScript> ().StartCountdown ();
					first = false;
				} else if (lastLink != null) {
					if (usePosition && Mathf.Abs (x - cutPoint) < 1f) {
						Debug.Log ("break");
						tmp.GetComponent<HingeJoint2D> ().enabled = false;
						tmp.SetActive (true);
						tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-1f, 1f), Random.Range (1f, 3f), 0);
					} else {
						tmp.GetComponent<HingeJoint2D> ().connectedBody = lastLink;
						tmp.SetActive (true);
					}
					tmp.GetComponent<SelfTurnOffScript> ().StartCountdown (3);
				}


				//tmp.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-1f, 1f), Random.Range (1f, 3f), 0);

				x += .5f;

				lastLink = tmp.GetComponent<Rigidbody2D> ();

			}
			StartCoroutine (chainAnimTimer());
		}
	}
	IEnumerator chainAnimTimer(){
		yield return new WaitForSeconds (.1f);
		chainAnimAllowed = true;
	}
	void CheckLineBreaks(){
		Vector2 dir =  new Vector3(swingPoint.x, swingPoint.y, 1) - transform.position;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, Mathf.Infinity, groundMask);
		if (hit.point != swingPoint) {
			AnimateLineBreak (false, Vector3.zero, hit.point);
			grappleAnchor.transform.parent = hit.collider.gameObject.transform;
			swingPoint = hit.point;
			SwingRadius = hit.distance;
			grappleAnchor.transform.position = swingPoint;
		}

	}
	// ----------------- // section of controls for jumping \\ ----------------- \\
	public void Jump(){
		// check if grounded
		if (grounded) {
			RB.velocity = new Vector2 (RB.velocity.x, 0);
			RB.AddForce (new Vector2 (0, jumpHeight));
			grounded = false;
			groundedBuffer = 1;
			wallBuffer = 10;
		} else if (onWallLeft || onWallRight) {
			WallJump ();
		} else if (doubleJump) {
			BreakLine ();
			RB.velocity = new Vector2 (RB.velocity.x, 0);
			RB.AddForce (new Vector2 (0, jumpHeight));
			doubleJump = false;
		}
	}
	void WallJump(){
		RB.velocity = new Vector2 (RB.velocity.x, 0);
		dash = true;
		if (onWallRight) {
			RB.AddForce (new Vector2 (-200, jumpHeight));
			onWallRight = false;
		} else {
			RB.AddForce (new Vector2 (200, jumpHeight));
			onWallLeft = false;
		}
		leftRightEnabled = false;
		wallBuffer = 2;
		StartCoroutine (LeftRightEnabler ());
	}

	// ----------------- // section of controls for doing sword throw \\ ----------------- \\
	public void SwordThrowPress(){
		if (hasSword && canAttack){
			throwTimerStart = Time.time;
			//leftRightEnabled = false;
			chargeSprite.SetActive(true);
			isChargingThrow = true;
		}
	}

	public void SwordThrowHold(){
		if (isChargingThrow) {
			if ((Time.time - throwTimerStart) > .5f) {
				fullyCharged = true;
				chargeParticles.SetActive (true);
			}
		}
	}
	public void SwordThrowRelease(){
		if (hasSword && isChargingThrow) {
			if (fullyCharged) {
				SwordThrow (swordThrowAngle);

				fullyCharged = false;
			}
			//throwTimerStart = Time.time;
			//leftRightEnabled = false;
			chargeSprite.SetActive (false);

			chargeParticles.SetActive (false);
			isChargingThrow = false;
		}
	}
	// function to shoot sword out
	void SwordThrow(Vector3 direction ){
		if (hasSword) {
			hasSword = false;

			// tmp change
			float chargeTime = 15f;
			StartCoroutine (SwordThrowAnim (direction, chargeTime));
		}
	}
	IEnumerator SwordThrowAnim(Vector3 direction, float forceMultiplyer){
		Debug.Log (direction + "   " + forceMultiplyer);
		thrownSword.transform.position = transform.position;
		thrownSword.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg - 90);
		thrownSword.gameObject.SetActive (true);
		thrownSword.velocity = thrownSword.transform.up * forceMultiplyer;
		yield return null;
	}
	public void ReturnSword(){
		hasSword = true;
	}
	// ----------------- // section of controls for doing regular attacks \\ ----------------- \\


	// attack wrapper
	public void StartSwingAttack(){
		if (dash && canAttack && !isChargingThrow && hasSword){
			StartCoroutine (CircleAttack ());
		}
	}
	IEnumerator CircleAttack(){
		canAttack = false;
		swingEffect.transform.eulerAngles = new Vector3 (180, 0, 0);
		swingEffect.SetActive (true);
		float yRot = 180;
		if (!facingRight) {
			yRot = 0;
		}
		int animTimer = 0;
		SpriteRenderer sword = swingEffect.GetComponent<SpriteRenderer> ();
		for (int x = -45; x <= 170; x += 11) {

			yield return null;
			if (animTimer < 5) {
				sword.sprite = SwordAnimations [animTimer];
			} else if (animTimer > 5 && animTimer < 15) {
				sword.sprite = SwordAnimations [5];
			} else if (animTimer > 10) {
				if (16 - animTimer >= 0) {
					sword.sprite = SwordAnimations [16 - animTimer];
				} else {
					sword.sprite = SwordAnimations [0];
				}
			}
			animTimer++;
			swingEffect.transform.eulerAngles = new Vector3 (180, yRot, x);
		}

		yield return null;
		yield return null;
		yield return null;
		yield return null;

		swingEffect.SetActive (false);
		yield return new WaitForSeconds (.2f);
		canAttack = true;
	}

	IEnumerator SwingingCooldown(){
		swingEnabled = false;
		yield return new WaitForSeconds (.4f);
		swingEnabled = true;
	}



	// ----------------- // section of controls for doing regular movement \\ ----------------- \\
	// function that handles all movement related functions
	public void MovementControl(float xAxis, float yAxis){
		// aiming 
		if (((Mathf.Abs (xAxis) > .65f) || (Mathf.Abs (yAxis) > .65f))) {
			swordThrowAngle = new Vector3 (xAxis, yAxis, 0);

		}
		if (movementInputEnabled) {
			// axis crontrols for horizontal movement
			if (Mathf.Abs (xAxis) > deadSize && !onWallRight && !onWallLeft && leftRightEnabled) {
				xStick = xAxis;
			} else if (grounded) {
				xStick = 0;
			} else {
				xStick = RB.velocity.x / groundSpeed;
			}

			if (xStick > 0) {
				facingRight = true;
				chargeSprite.transform.eulerAngles = new Vector3(0, 180,-30 );
				chargeSprite.transform.localPosition = new Vector3(-.3f, 0, 0);
			} else if (xStick < 0) {
				facingRight = false;

				chargeSprite.transform.eulerAngles = new Vector3(0, 0,-30 );
				chargeSprite.transform.localPosition = new Vector3(.3f, 0, 0);
			}



			if (((RB.velocity.x <= xStick * groundSpeed) && (xStick > 0) || (RB.velocity.x >= xStick * groundSpeed) && (xStick < 0)) || grounded) {
				RB.velocity = new Vector3 ((xStick * groundSpeed) + platformSpeed, RB.velocity.y, 0);
			}




			if (onWallLeft || onWallRight) {
				if (onWallLeft && xAxis > .75f) {
					wallEjectBuffer++;
				}else if (onWallRight && xAxis < -.75){
					wallEjectBuffer++;
				}

				if (wallEjectBuffer >= 4 && onWallLeft) {
					RB.velocity = new Vector2 (1f, -1f);
					wallEjectBuffer = 0;
				} else if (wallEjectBuffer >= 4 && onWallRight){
					RB.velocity = new Vector2 (-1f, -1f);
					wallEjectBuffer = 0;
				}else {
					RB.velocity = new Vector2 (0f, -1f);
				}
			}


		}

	}


	IEnumerator LeftRightEnabler(){
		yield return new WaitForSeconds (.05f);
		leftRightEnabled = true;
	}




	// ----------------- // general and misc functions \\ ----------------- \\
	public int GetPlayerNumber(){
		return playerNumber;
	}

	void OnDisable(){
		StopAllCoroutines ();
		BreakLine ();
		ChainHixbox.SetActive (false);
		swingEffect.SetActive (false);
		grappleAnchor.SetActive (false);

	}
	public void DisableControls(){
		movementInputEnabled = false;
		gameOver = true;
		IS.GameOver ();
		BreakLine ();
	}


	public void SetColor(Color newColor){
		swordColor.color = newColor;
	}
}
