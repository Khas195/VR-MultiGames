using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour {
	[SerializeField]
	float launchForce;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void OnTriggerEnter(Collider other){
		other.attachedRigidbody.velocity = Vector3.zero;
		other.attachedRigidbody.AddForce (transform.up * launchForce, ForceMode.Impulse);
	}
}
