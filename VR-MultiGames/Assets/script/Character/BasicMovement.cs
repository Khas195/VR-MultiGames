using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Camera))]
public class BasicMovement : MonoBehaviour {
	public float jumpForce;
	public float moveSpeed;
	public Camera characterCamera;

	Rigidbody rg;
	bool jump;
	Vector3 moveSide;
	Vector3 moveForward;
	// Use this for initialization
	void Start () {
		rg = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		var side = Input.GetAxis ("Horizontal");
		var forward = Input.GetAxis ("Vertical");
		moveSide = characterCamera.transform.right * side;
		moveForward = characterCamera.transform.forward * forward;
		if (Input.GetKeyDown (KeyCode.Space) && IsGrounded ()) {
			jump = true;
		} else {
			jump = false;
		}
	}
	public void FixedUpdate(){
		Move (moveSide + moveForward);
		if (jump) {
			Jump ();
		}
	}
	public void Jump(){
		var vel = rg.velocity;
		vel.y = jumpForce ;
		rg.velocity = vel;
	}
	public void Move(Vector3 direction){
		direction *= moveSpeed ;
		direction.y = rg.velocity.y;
		rg.velocity = direction;
	}
	public bool IsGrounded(){
		return Physics.Raycast(transform.position,  -transform.up, GetComponent<Collider>().bounds.extents.y);
	}
}
