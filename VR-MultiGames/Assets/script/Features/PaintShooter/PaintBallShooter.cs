using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBallShooter : PaintShooter {

	[SerializeField]
	float shootForce;
	[SerializeField]
	Transform shootPos;
	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
	public override void Fire()
	{		
		var ball = AmunitionPool.GetPool ().RequestAmmo ();
		ball.SetColor (gunColor);
		ball.transform.position = shootPos.position;
		ball.ApplyForce (lookAtCamera.transform.forward * shootForce * 10);
		nextShot = Time.time + fireRate;
	}
}
