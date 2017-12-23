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

    public Vector3 test;
	// Use this for initialization
	void Start () {
		rg = this.GetComponent<Rigidbody> ();
	}

    void Update()
    {
    }
    public void Jump(Vector3 direction, float force){
		direction *= force;
		rg.velocity = direction;
	}
	public void Move(Vector3 direction){
		direction *= data.moveSpeed ;
        direction.y = rg.velocity.y;
        rg.velocity = direction;
	}
	public bool IsGrounded(){
		return Physics.Raycast(transform.position,  -transform.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
	}
}
