using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Laser : MonoBehaviour
{
	Vector3 shootOrigin;
	Vector3 flyDirection;
	float flySpeed;
	[SerializeField]
	Light laserLight;

	void Start(){
	}
	public void SetSpeed (float laserSpeed)
	{
		this.flySpeed = laserSpeed;
	}
	public void SetLaserColor (Color laserColor)
	{
		this.laserLight.color = laserColor;
	}
	public void Update(){
		transform.position += flyDirection * Time.deltaTime * flySpeed;
	}
	public void OnCollisionEnter(Collision other){
		var paintable = other.collider.gameObject.GetComponent<Paintable> ();
		if (paintable == null)
			return;

		Ultil.TryShootPaint (this.transform.position, flyDirection, laserLight.color, 10f);

		LaserPool.GetPool ().ReturnAmmo (this.gameObject);
	}
	public void SetRayCastInfo(Vector3 position, Vector3 direction){
		// direction should be normalized
		this.shootOrigin = position;
		this.flyDirection = direction;
		this.transform.rotation = Quaternion.LookRotation (direction);
	}
}

