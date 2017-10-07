using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IHighlightable{
	void TurnOn ();
	void TurnOff();
}

public class HighlightLookAt : MonoBehaviour {
	[SerializeField]
	Camera lookatCamera;

	IHighlightable curHighlight;

	RaycastHit hit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!Physics.Raycast (lookatCamera.transform.position, lookatCamera.transform.forward, out hit)) {
			return;
		}
		var targetHighlight = hit.collider.gameObject.GetComponent<IHighlightable> ();

		if (curHighlight == targetHighlight) {
			return;
		}

		if (targetHighlight != null) {
			targetHighlight.TurnOn ();
		}

		TurnOffCurHighlight ();

		curHighlight = targetHighlight;
	}
	void OnDisable(){
		TurnOffCurHighlight ();
		curHighlight = null;
	}

	void TurnOffCurHighlight ()
	{
		if (curHighlight != null) {
			curHighlight.TurnOff ();
		}
	}
}
