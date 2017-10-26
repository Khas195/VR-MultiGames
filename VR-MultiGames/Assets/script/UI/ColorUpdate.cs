using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUpdate : MonoBehaviour {
	[SerializeField]
	Paintable redBox;
	[SerializeField]
	Text redText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (redBox.HasInit ()) {
			redText.text = "RedBox: " + Mathf.Floor(redBox.GetFillPercentage () * 100).ToString() + "%";
		}
	}
}
