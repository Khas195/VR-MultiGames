using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HighlightLookAt : MonoBehaviour {
	[SerializeField]
	Camera lookatCamera;

	RaycastHit hit;
	Glowable current;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Ultil.GetObjectCameraIsLookingAt (lookatCamera, out hit)) {
			var glowable = hit.collider.gameObject.GetComponent<Glowable> ();
			if (glowable != current) {
				TurnOffCurGlowable ();
			}

			if (glowable != null) {
				glowable.Glow ();
				current = glowable;
			}
		} else {
			TurnOffCurGlowable ();
		}
	}

	void TurnOffCurGlowable ()
	{
		if (current != null) {
			current.StopGlow ();
			current = null;
		}
	}
}
