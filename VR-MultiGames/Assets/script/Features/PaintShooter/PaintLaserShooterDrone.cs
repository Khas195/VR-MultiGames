using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLaserShooterDrone : PaintShooter
{
    public bool constantFire;
	[SerializeField]
	float laserSpeed;
	[SerializeField]
	Transform shootPos;
	// normalized
	Vector3 direction;
	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update () 
	{
		if (constantFire && CanFire())
            Fire();
	}

	public override void Fire()
	{		
		var laser = LaserPool.GetPool ().RequestAmmo ().GetComponent<Laser> ();
		laser.transform.position = shootPos.position;
		laser.SetSpeed (laserSpeed);
		laser.SetLaserColor (gunColor);
		laser.SetDirection (transform.forward);
        SoundsManager.GetInstance().PlayClip(ActionInGame.DroneShootLaser, transform.position);

		nextShot = Time.time + fireRate;
	}
}
