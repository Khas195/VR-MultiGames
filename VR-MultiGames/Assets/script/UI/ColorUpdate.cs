using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUpdate : MonoBehaviour {
	[SerializeField]
	Camera lookatCamera;
	[SerializeField]
	Text redText;

	RaycastHit hit;
	Paintable target;
	// Use this for initialization
	void Start () {
		
	}

	void SetTextVisibility (float alphaValue)
	{
		var tempColor = redText.color;
		tempColor.a = alphaValue;
		redText.color = tempColor;
	}

	// Update is called once per frame
	void Update () {
		if (Ultil.GetObjectCameraIsLookingAt (lookatCamera, out hit)) {
			var paintable = hit.collider.gameObject.GetComponent<Paintable> ();
			var glowable = hit.collider.gameObject.GetComponent<Glowable> ();
			if (glowable != null) {
				if (paintable != null) {
					target = paintable;SetTextVisibility (1);			
				}
			} else {
				target = null;
				target = paintable;SetTextVisibility (0);
			}
		}
		UpdateTargetFillPercentage ();
	}

	void UpdateTargetFillPercentage ()
	{
		if (target != null) {
			if (target.HasInit ()) {
				redText.text =target.gameObject.name + " : " + Mathf.Floor (target.GetFillPercentage () * 100).ToString () + "%";
			}
		}
	}
}
