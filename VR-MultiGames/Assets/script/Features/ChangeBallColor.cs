using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBallColor : MonoBehaviour {
	[SerializeField]
	Color targetColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnTriggerEnter(Collider other){ 
		PaintBall ball = other.gameObject.GetComponent<PaintBall> ();
		if (ball != null) {
			ball.SetColor (targetColor);
		}
	}
}
