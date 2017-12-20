using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBallColor : MonoBehaviour {
	[SerializeField]
	Color targetColor;
	
	public void OnTriggerEnter(Collider other){ 
		IBullet ball = other.gameObject.GetComponent<IBullet> ();
		if (ball != null) {
			ball.SetColor (targetColor);
		}
	}
}
