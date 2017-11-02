using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunColorDisplay : MonoBehaviour {
	[SerializeField]
	PaintShooter gun;
	[SerializeField]
	Image crossHair;
	// Use this for initialization
	void Start () {

		crossHair.color = Color.red;
		var tempColor = crossHair.color;
		tempColor.a = 1;
		crossHair.color = tempColor;
		gun.SetGunColor (Color.red);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnGunColorChanged(){
	}
}
