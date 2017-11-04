using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Paintable))]
public class PaintFillEvent : MonoBehaviour {

	[SerializeField]
	float targetPercentage;
	[SerializeField]
	UnityEvent fillReachTarget;

	Paintable paintable;
	// Use this for initialization
	void Start () {
		paintable = this.GetComponent<Paintable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (paintable.GetFillPercentage () * 100 >= targetPercentage) {
			this.enabled = false;
			fillReachTarget.Invoke ();
		}
	}
}
