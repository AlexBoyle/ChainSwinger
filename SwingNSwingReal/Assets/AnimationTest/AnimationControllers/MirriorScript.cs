using UnityEngine;
using System.Collections;

public class MirriorScript : StateMachineBehaviour {

	private bool facingright;
	
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{	
		facingright = animator.gameObject.GetComponent<SpriteRenderer>().flipX;// = animator.GetBool("FacingRight");
		//Debug.Log(animator.gameObject.GetComponent<SpriteRenderer>().flipX);
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(facingright != animator.GetBool("FacingRight"))
		{
			facingright = !facingright;
			animator.gameObject.GetComponent<SpriteRenderer>().flipX = animator.GetBool("FacingRight");
		}
	}
}
