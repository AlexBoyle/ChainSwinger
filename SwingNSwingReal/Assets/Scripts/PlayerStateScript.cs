using UnityEngine;
using System.Collections;

public class PlayerStateScript : MonoBehaviour
{
	//0 = on Ground
	//1 = in Air
	//2 = on Wall
	private int State;
	private bool FacingRight;

	public Animator anim;

	void Start ()
	{
		anim = gameObject.GetComponent<Animator>();
		State = 0;
		FacingRight = false;
	}

	void Update ()
	{
		UpdateAnimations();
	}

	private void UpdateAnimations()
	{
		anim.SetBool("FacingRight", FacingRight);
	}

	public void SetFacingRight(bool facer)
	{
		FacingRight = facer;
	}

	public void UpdateState(int newstate)
	{
		State = newstate;
	}
}
