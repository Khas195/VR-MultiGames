using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ColorToInteraction {
	public Color color;
	public Interaction interaction;
}

public class PaintInteraction : MonoBehaviour {
	public List<ColorToInteraction> interactionList;

	static PaintInteraction instance;
	public static PaintInteraction GetInstance(){
		return instance;
	}

	public Interaction GetInteractionBasedOnColor (Color color)
	{
		foreach (var i in interactionList) {
			if (Ultil.CalColorDifference (color, i.color) < 0.5f) {
				return i.interaction;
			}
		}
		return null;
	}

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
