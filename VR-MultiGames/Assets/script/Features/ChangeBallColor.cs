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
		IBullet ball = other.gameObject.GetComponent<IBullet> ();
		if (ball != null) {
			ball.SetColor (targetColor);
		}
	}
}
