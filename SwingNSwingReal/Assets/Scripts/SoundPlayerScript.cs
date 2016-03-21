using UnityEngine;
using System.Collections;

public class SoundPlayerScript : MonoBehaviour {
	public AudioSource AS;
	public AudioClip jump;
	public AudioClip swordAttack;
	public AudioClip chainAttach;
	public AudioClip chainBreak;
	public AudioClip swordHit;
	public AudioClip death;
	public AudioClip ghostRespawn;
	public AudioClip swordThrow;
	public AudioClip swordBringOut;
	public AudioClip swordPutAway;
	public AudioClip swordPickUp;
	public AudioClip swordFullyCharged;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlayJump(){
		AS.PlayOneShot (jump);
	}
	public void PlaySwordAttack(){
		AS.PlayOneShot (swordAttack);
	}
	public void PlayChainAttach(){
		AS.PlayOneShot (chainAttach);
	}
	public void PlayChainBreak(){
		AS.PlayOneShot (chainBreak);
	}
	public void PlaySwordHit(){
		AS.PlayOneShot (swordHit);
	}
	public void PlayDeath(){
		AS.PlayOneShot (death);
	}
	public void PlayGhostRespawn(){
		AS.PlayOneShot (ghostRespawn);
	}
	public void PlaySwordThrow(){
		AS.PlayOneShot (swordThrow);
	}
	public void PlaySwordBringOut(){
		AS.PlayOneShot (swordBringOut);
	}
	public void PlaySwordPutAway(){
		AS.PlayOneShot (swordPutAway);
	}
	public void PlaySwordPickUp(){
		AS.PlayOneShot (swordPickUp);
	}
	public void PlaySwordFullyCharged(){
		AS.PlayOneShot (swordFullyCharged);
	}
}
