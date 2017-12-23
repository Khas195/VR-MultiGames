using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PaintShooter : MonoBehaviour {

	[SerializeField]
	protected Camera lookAtCamera;
	[SerializeField]
	[Tooltip("Number of ball per second")]
	protected float fireRate;
	protected float nextShot;
	[SerializeField]
	protected Color gunColor;

	// Use this for initialization
	public void Start () {
		Cursor.visible = false;
		this.SetGunColor(GameSettings.GetInstance ().GetColorAt (0));
	}

	// Update is called once per frame
	public void Update () {
		if (Input.GetMouseButton (0) && CanFire()) {
			Fire ();
		}
	}
	public virtual void Fire()
	{		
		RaycastHit hit;
		if (Ultil.GetObjectCameraIsLookingAt (lookAtCamera, out hit)) {
			var paintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (paintable != null) {
				paintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), gunColor);
			}
		}
		nextShot = Time.time + fireRate;
	}
	public virtual  bool CanFire(){
		return Time.time > nextShot;
	}

	public virtual Color GetGunColor ()
	{
		return gunColor;
	}
	public virtual void SetGunColor(Color newColor){
		gunColor = newColor;
	}
}
