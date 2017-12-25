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
    [SerializeField]
    private AudioSource audio;
	// Use this for initialization
	void Start ()
	{
	    audio = this.GetComponent<AudioSource>();
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
	    audio.clip = SoundsManager.GetInstance().GetClip(ActionInGame.DroneShootLaser);
        audio.Play();

		nextShot = Time.time + fireRate;
	}
}
