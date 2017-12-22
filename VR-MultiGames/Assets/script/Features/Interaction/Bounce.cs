using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : Interaction {
	public float bounceForce;
	public override void Interact(GameObject targetObject){
		var movement = targetObject.GetComponent<BasicMovement> ();
		if (movement != null) {
			var originForce = movement.data.jumpForce;
			movement.data.jumpForce = bounceForce;
			if (movement.IsGrounded ()) {
				movement.Jump ();
			}
			movement.data.jumpForce = originForce;
		}
	}
	public override bool IsDone ()
	{
		return true;
	}

	public override void RevertInteraction (GameObject gameObject)
	{
	}

	public override void Init (GameObject gameObject)
	{
	}
}
