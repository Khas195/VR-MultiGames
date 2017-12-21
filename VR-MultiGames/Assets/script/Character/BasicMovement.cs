using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BasicMovement : MonoBehaviour {
	public float jumpForce;
	public float moveSpeed;
	public Camera characterCamera;

	Rigidbody rg;
	// Use this for initialization
	void Start () {
		rg = this.GetComponent<Rigidbody> ();
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
		return Physics.Raycast(transform.position,  -transform.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
	}


}
