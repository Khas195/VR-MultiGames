using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour {

	[SerializeField]
	UnityEngine.Events.UnityEvent trigger;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			Debug.Log ("Invoke");
			trigger.Invoke ();
		}
	}
	public void Done(){
		Debug.Log (this + " done");
	}
}
