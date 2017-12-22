using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MovementData {
	public float jumpForce;
	public float moveSpeed;
}

[RequireComponent(typeof(Rigidbody))]
public class BasicMovement : MonoBehaviour {
	public MovementData data;
	public Camera characterCamera;
	Rigidbody rg;
	// Use this for initialization
	void Start () {
		rg = this.GetComponent<Rigidbody> ();
	}
	public void Jump(){
		var vel = rg.velocity;
		vel.y = data.jumpForce ;
		rg.velocity = vel;
	}
	public void Move(Vector3 direction){
		direction *= data.moveSpeed ;
		direction.y = 0;
		rg.MovePosition (transform.position + direction * Time.deltaTime);
	}
	public bool IsGrounded(){
		return Physics.Raycast(transform.position,  -transform.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
	}
}
