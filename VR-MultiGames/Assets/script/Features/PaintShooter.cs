using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintShooter : MonoBehaviour {

	[SerializeField]
	UnityEngine.Events.UnityEvent OnGunColorChange;
	[SerializeField]
	List<KeyCode> colorKeys;

	[SerializeField]
	Camera lookAtCamera;
	[SerializeField]
	Transform shootPos;
	[SerializeField]
	float shootForce;
	[SerializeField]
	[Tooltip("Number of ball per second")]
	float fireRate;
	float nextShot;

	Color curColor;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		curColor = GameSettings.GetInstance ().GetColorAt (0);
		OnGunColorChange.Invoke ();
	}

	// Update is called once per frame
	void Update () {
		for(int i =0; i < colorKeys.Count; ++i){
			if (Input.GetKeyDown (colorKeys [i])) {
				curColor = GameSettings.GetInstance ().GetColorAt (i);
				OnGunColorChange.Invoke ();
			}
		}
		if (Input.GetMouseButton (0) && CanFire()) {
			Fire ();
			nextShot = Time.time + fireRate;
		}
	}
	void Fire()
	{		
		RaycastHit hit;
		if (Ultil.GetObjectCameraIsLookingAt (lookAtCamera, out hit)) {
			var paintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (paintable != null) {
				paintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), curColor);
			}
		}
//		}
//		var ammo = AmunitionPool.GetPool ().RequestAmmo ();
//		ammo.Init (curColor);
//		var rb = ammo.GetGameObject().GetComponent<Rigidbody> ();
//		rb.velocity = Vector3.zero;
//		ammo.GetGameObject().transform.position = shootPos.position; 	
//		rb.AddForce (lookAtCamera.transform.forward * shootForce * 10, ForceMode.Acceleration);
	}
	bool CanFire(){
		return Time.time > nextShot;
	}

	public Color GetGunColor ()
	{
		return curColor;
	}
}
