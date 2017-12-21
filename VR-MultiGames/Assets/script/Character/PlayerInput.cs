using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInput : MonoBehaviour{
	public BasicMovement movement;
	public Camera characterCamera;

	Vector3 moveSide;
	bool jump;
	Vector3 moveForward;
	Interaction currentInteraction;
	public void HandlePaintInteraction(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position,  -transform.up, out hit,  GetComponent<Collider>().bounds.extents.y + 0.1f, LayerMask.GetMask("Obstacle"))) {
			var hitPaintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (hitPaintable != null) {
				Color color =  hitPaintable.GetColorAtTextureCoord (hit.textureCoord2);
				var interaction = PaintInteraction.GetInstance ().GetInteractionBasedOnColor (color);
				currentInteraction = interaction;
			}
		}

	}
	public void Update (){
		HandleMovementInput ();
		HandleJumpInput ();
		HandlePaintInteraction ();
	}
	public void FixedUpdate(){
		ProcessMovement ();
		if (currentInteraction != null) {
			currentInteraction.Interact (this.gameObject);
			currentInteraction = null;
		}
	}
	public void HandleMovementInput ()
	{
		var side = Input.GetAxis ("Horizontal");
		var forward = Input.GetAxis ("Vertical");
		moveSide = characterCamera.transform.right * side;
		moveForward = characterCamera.transform.forward * forward;
	}

	public void HandleJumpInput ()
	{
		if (Input.GetKeyDown(KeyCode.Space) && movement.IsGrounded ()) {
			jump = true;
		}
		else {
			jump = false;
		}
	}

	void ProcessMovement ()
	{
		movement.Move (moveSide + moveForward);
		if (jump) {
			movement.Jump ();
		}
	}

}
