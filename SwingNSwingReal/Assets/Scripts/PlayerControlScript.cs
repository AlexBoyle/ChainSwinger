using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class PlayerControlScript : MonoBehaviour {

	public Rigidbody2D RB;
	public float groundSpeed, jumpHeight, DashSpeed;
	public GameObject ghost, grappleAnchor, ChainHixbox, swingEffect;
	public Rigidbody2D thrownSword;
	public int playerNumber;
	public bool BlockReelIn = false;
	float xStick, yStick, deadSize = .25f;
	bool movementInputEnabled = true, doubleJump = false, grounded = false, onWallRight = false, onWallLeft = false, leftRightEnabled = true, swinging = false, facingRight, dash = false, 
	canAttack = true, swingEnabled = true, gameOver = false, chainAnimAllowed = true, hasSword, isChargingThrow = false;
	ScoreScript SS;
	public Sprite[] SwordAnimations;

	public ObjectPoolScript chainLinkPool;

	int groundMask, playerGroundMask;
	int groundedBuffer = 0, wallBuffer = 0;
	Vector2 swingPoint, grappleDirection;
	Vector3 prevPosition, swordThrowAngle;
	float SwingRadius, throwTimerStart;
	public LineRenderer LR;
	SpriteRenderer SR;
	//ObjectPoolScript SwingEffectPool;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	// Use this for initializationswing
	void Start () {
		groundMask = 1 << 8;
		playerGroundMask = 1 << 9; // maybe nine maybe just a number
		playerIndex = (PlayerIndex)playerNumber;
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
		canAttack = true;
		swingEnabled = true;
		chainAnimAllowed = true;
		hasSword = true;
		isChargingThrow = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameOver) {
			prevState = state;
			state = GamePad.GetState (playerIndex, GamePadDeadZone.None);

			// do a check for ground
			RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, .35f, groundMask);
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


			if (movementInputEnabled) {
				// axis crontrols for horizontal movement
				if (Mathf.Abs (state.ThumbSticks.Left.X) > deadSize && !onWallRight && !onWallLeft && leftRightEnabled && !isChargingThrow) {
					xStick = state.ThumbSticks.Left.X;
				} else if (grounded) {
					xStick = 0;
				} else {
					xStick = RB.velocity.x / groundSpeed;
				}

				if (xStick > 0) {
					facingRight = true;
				} else if (xStick < 0) {
					facingRight = false;
				}

				if (((RB.velocity.x <= xStick * groundSpeed) && (xStick > 0) || (RB.velocity.x >= xStick * groundSpeed) && (xStick < 0)) || grounded) {
						RB.velocity = new Vector3 (xStick * groundSpeed, RB.velocity.y, 0);
				}


				// jump button
				if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A != ButtonState.Pressed && !isChargingThrow) {
					// check if grounded
					if (grounded && !isChargingThrow) {
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

				if (onWallLeft || onWallRight) {
					RB.velocity = new Vector2 (0f, -.75f);
				}


			}

			AttackControls ();
			// change up velocity based on swinging state
			if (swinging) {
				CheckLineBreaks ();

				// reel in line
				if (SwingRadius > 1 && !BlockReelIn) {
					SwingRadius -= .05f;
				}

				LineGraphicsUpdate ();
				if (Vector2.Distance (transform.position, swingPoint) > SwingRadius) {
					
					transform.position = Vector2.MoveTowards (transform.position, swingPoint, Vector2.Distance (transform.position, swingPoint) - SwingRadius);
					RB.velocity = (transform.position - prevPosition) / Time.deltaTime;
				}
			}
			
		
			prevPosition = transform.position;
		}
	}

	// ----------------- // section of controls for doing attacks and swinging chain \\ ----------------- \\
	void AttackControls(){
		// trigger press for starting the charge on throwing the sword
		if (state.Triggers.Left > .5f && prevState.Triggers.Left <= .5f && hasSword) {    //  && Controller){

			//throwTimerStart = Time.time;
			//leftRightEnabled = false;
			//isChargingThrow = true;
		}
		// aiming 
		if (((Mathf.Abs (state.ThumbSticks.Left.X) > .65f) || (Mathf.Abs (state.ThumbSticks.Left.Y) > .65f))) {

			swordThrowAngle = new Vector3 (state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0);
		}

		// aiming button controls released
		if ((state.Triggers.Left <= .5f && prevState.Triggers.Left > .5f) && canAttack) {    //  && Controller){
			isChargingThrow = false;
			leftRightEnabled = true;
			SwordThrow(swordThrowAngle);
		}


		// input for  swinging
		if (prevState.Triggers.Right <= .5f && state.Triggers.Right > .5f && !onWallLeft && !onWallRight && swingEnabled) {
			if (grounded) {
				grappleDirection = new Vector2 (0,1);
			}else if (facingRight) {
				grappleDirection = new Vector2 (1, 1);
			} else {
				grappleDirection = new Vector2 (-1, 1);
			}

			RaycastHit2D swingHit = Physics2D.Raycast (transform.position, grappleDirection, Mathf.Infinity, groundMask);
			swingPoint = swingHit.point;
			SwingRadius = swingHit.distance;
			grappleAnchor.transform.position = swingPoint;
			grappleAnchor.SetActive (true);
			LR.enabled = true;
			ChainHixbox.SetActive (true);
			StartCoroutine (SwingingCooldown ());
			swinging = true;
			leftRightEnabled = false;
		}
		// input for releasing swing
		if (prevState.Triggers.Right > .5f && state.Triggers.Right <= .5f) {
			BreakLine ();
		}
		// inptut for secondary released
		if (state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed) {

		}
		// input for shooting secondary
		if (state.Buttons.RightShoulder == ButtonState.Pressed) {
		}
		// menu control
		if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released && Time.timeScale == 1) {


		}

		// x button for swinging
		if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released && dash && canAttack && !isChargingThrow && hasSword) {

			StartCoroutine (CircleAttack ());

			// old call for dash attack
			//StartCoroutine (DashAttack(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y));

		}
		if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released) {

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

	IEnumerator LeftRightEnabler(){
		yield return new WaitForSeconds (.05f);
		leftRightEnabled = true;
	}
	IEnumerator SwingingCooldown(){
		swingEnabled = false;
		yield return new WaitForSeconds (.4f);
		swingEnabled = true;
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
			swingPoint = hit.point;
			SwingRadius = hit.distance;
			grappleAnchor.transform.position = swingPoint;
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
		yield return new WaitForSeconds (.25f);
		canAttack = true;
	}

	// function to shoot sword out
	void SwordThrow(Vector3 direction ){
		if (hasSword) {
			hasSword = false;
			float chargeTime =  Time.time - throwTimerStart;
			if (chargeTime > 1f) {
				chargeTime = 1f;
			}
			// tmp change
			chargeTime = 15f;
			StartCoroutine (SwordThrowAnim (direction, chargeTime));
		}
	}
	IEnumerator SwordThrowAnim(Vector3 direction, float forceMultiplyer){
		Debug.Log (direction + "   " + forceMultiplyer);
		thrownSword.transform.position = transform.position;
		thrownSword.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg - 90);
		thrownSword.gameObject.SetActive (true);
		thrownSword.velocity = thrownSword.transform.up * forceMultiplyer;

		/*
		float currentDistance = 0;
		GameObject lastLink = thrownSword.gameObject;
		for (int x  = 0; x < 50; x++){
			float newDistance = Vector3.Distance (transform.position, thrownSword.transform.position);
			float difference = newDistance - currentDistance;

			if ( difference > .5) {
				currentDistance = newDistance;
				while (difference > 0){
					GameObject tmp = chainLinkPool.FetchObject ();
					tmp.transform.position = Vector3.MoveTowards (lastLink.transform.position, transform.position, .5f);

					Vector2 diff = transform.position - new Vector3 (lastLink.transform.position.x, lastLink.transform.position.y, 0);
					Quaternion rot = Quaternion.Euler (0, 0, 180 - Mathf.Atan2 (diff.x, diff.y) * Mathf.Rad2Deg);

					tmp.transform.rotation = rot;
					tmp.GetComponent<HingeJoint2D> ().connectedBody = null;
					tmp.GetComponent<HingeJoint2D> ().enabled = true;
					tmp.GetComponent<HingeJoint2D> ().connectedBody = lastLink.GetComponent<Rigidbody2D>();
					tmp.SetActive (true);
					lastLink = tmp;
					difference -= .5f;
				}

			}
			yield return null;
		}*/
		yield return null;
		// start reeling in chain
		//hasSword = true;


		
	}

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
		BreakLine ();
	}

	public void ReturnSword(){
		hasSword = true;
	}
}
