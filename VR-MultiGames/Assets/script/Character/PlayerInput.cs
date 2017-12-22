using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInput : MonoBehaviour{
	public BasicMovement movement;
	public Camera characterCamera;

	Vector3 moveSide;
	bool jump;
	Vector3 moveForward;
	List<Interaction> curInteractions = new List<Interaction> ();


	public void Start () {
	}
	public void HandlePaintInteraction(){
		RaycastHit hit;
		Interaction interaction = null;
		if (Physics.Raycast (transform.position, -transform.up, out hit, GetComponent<Collider> ().bounds.extents.y + 0.1f, LayerMask.GetMask ("Obstacle"))) {
			var hitPaintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (hitPaintable != null) {
				Color color = hitPaintable.GetColorAtTextureCoord (hit.textureCoord2);
				interaction = PaintInteraction.GetInstance ().GetInteractionBasedOnColor (color);
			}
		} 

		if (interaction != null && !curInteractions.Contains (interaction)) {
			interaction.Init (gameObject);
			curInteractions.Add (interaction);
		}
	}
	public void Update (){
		HandleMovementInput ();
		HandleJumpInput ();
		HandlePaintInteraction ();
	}
	public void FixedUpdate(){
		ProcessMovement ();
		ProcessInteraction ();
	}

	void ProcessInteraction ()
	{
		var finishedInteraction = new List<Interaction> ();
		foreach (var i in curInteractions) {
			i.Interact (gameObject);
			if (i.IsDone ()) {
				finishedInteraction.Add (i);
			}
		}
		foreach (var i in finishedInteraction) {
			i.RevertInteraction (gameObject);
			curInteractions.Remove (i);
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
