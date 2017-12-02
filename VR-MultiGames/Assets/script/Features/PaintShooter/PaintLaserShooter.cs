using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLaserShooter : PaintShooter {
	[SerializeField]
	float laserSpeed;
	[SerializeField]
	Color laserColor;
	[SerializeField]
	Transform shootPos;
	// normalized
	Vector3 direction;
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
		var laser = LaserPool.GetPool ().RequestAmmo ().GetComponent<Laser> ();
		laser.transform.position = shootPos.position;
		laser.SetSpeed (laserSpeed);
		laser.SetLaserColor (laserColor);
		laser.SetRayCastInfo ( this.lookAtCamera.transform.position, lookAtCamera.transform.forward);
		nextShot = Time.time + fireRate;
	}
}
