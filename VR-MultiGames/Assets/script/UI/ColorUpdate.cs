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
	PaintFillEvent fillEvent;
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
					target = paintable;
					fillEvent = hit.collider.gameObject.GetComponent<PaintFillEvent> ();
					SetTextVisibility (1);			
				}
			} else {
				target = null;
				SetTextVisibility (0);
			}
		}
		UpdateTargetFillPercentage ();
	}

	void UpdateTargetFillPercentage ()
	{
		if (target != null) {
			if (target.HasInit ()) {
				redText.text =target.gameObject.name + " : " + Mathf.Floor (Mathf.Clamp01(target.GetFillPercentage ()/fillEvent.GetTargetPercent() ) * 100).ToString () + "%";
			}
		}
	}
}
