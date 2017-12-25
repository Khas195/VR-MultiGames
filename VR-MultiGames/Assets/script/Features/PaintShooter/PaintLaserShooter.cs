using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLaserShooter : PaintShooter {
	[SerializeField]
	float laserSpeed;
	[SerializeField]
	Transform shootPos;
	// normalized
	Vector3 direction;
	// Use this for initialization
    [SerializeField]
    private GameObject gunObject;

    [SerializeField]
    private AudioSource audio;
    public Renderer[] Renderers
    {
        get;
        private set;
    }
    private List<Material> gunMaterials = new List<Material>();
	void Start () {
		base.Start ();
	    audio = this.GetComponent<AudioSource>();
	    Renderers = gunObject.GetComponents<Renderer>();

	    foreach (var renderer in Renderers)
	    {
	        gunMaterials.AddRange(renderer.materials);
	    }
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
		laser.SetLaserColor (this.gunColor);
		laser.SetDirection (lookAtCamera.transform.forward.normalized);
	    SoundsManager.GetInstance().PlayClip(audio, ActionInGame.PlayerShootLaser);
		nextShot = Time.time + fireRate;
	}

    public override void SetGunColor(Color newColor)
    {
        base.SetGunColor(newColor);
        foreach (var mat in gunMaterials)
        {
            mat.SetColor("_Color", gunColor);
        }
    }
}
