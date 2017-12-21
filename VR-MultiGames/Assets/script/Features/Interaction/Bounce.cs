using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : Interaction {
	public float bounceForce;
	public override void Interact(GameObject targetObject){
		var movement = targetObject.GetComponent<BasicMovement> ();
		if (movement != null) {
			var originForce = movement.jumpForce;
			movement.jumpForce = bounceForce;
			movement.Jump ();
			movement.jumpForce = originForce;
		}
	}
}
