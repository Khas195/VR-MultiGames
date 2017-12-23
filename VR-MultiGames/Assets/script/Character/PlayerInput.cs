using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInput : MonoBehaviour{
	public BasicMovement Movement;
	public Camera CharacterCamera;

	Vector3 moveSide;
	bool jump;
	Vector3 moveForward;
    private bool controlEnable = true;

    public void DisableControl()
    {
        this.controlEnable = false;
        moveSide = Vector3.zero;
        moveForward = Vector3.zero;
    }
    public void EnableControl()
    {
        this.controlEnable = true;
    }
    public void Update (){
        if (controlEnable) { 
		    HandleMovementInput ();
		    HandleJumpInput ();
        }
	}
	public void FixedUpdate(){
	    if (controlEnable)
	    {
	        ProcessMovement();
	    }

	}
	public void HandleMovementInput ()
	{
		var side = Input.GetAxis ("Horizontal");
		var forward = Input.GetAxis ("Vertical");
		moveSide = CharacterCamera.transform.right * side;
		moveForward = CharacterCamera.transform.forward * forward;
	}
	public void HandleJumpInput ()
	{
		if (Input.GetKeyDown(KeyCode.Space) && Movement.IsGrounded ()) {
			jump = true;
		}
		else {
			jump = false;
		}
	}
	void ProcessMovement ()
	{
		Movement.Move (moveSide + moveForward);
		if (jump) {
			Movement.Jump (Movement.transform.up, Movement.data.jumpForce);
		}
	}

}
