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

	[SerializeField] 
	private LayerMask _paintableLayermMask;
	
	public void SetSpeed (float laserSpeed)
	{
		this.flySpeed = laserSpeed;
	}
	public void SetLaserColor (Color laserColor)
	{
		this.laserLight.color = laserColor;
	}

	public void Update()
	{
		var distanceCover = Time.deltaTime * flySpeed;
		var velocity = flyDirection * distanceCover;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, flyDirection, out hit, distanceCover, _paintableLayermMask))
		{
			var paintable = hit.collider.gameObject.GetComponent<Paintable>();
			if (paintable != null)
			{
				Ultil.TryShootPaint(transform.position, flyDirection, laserLight.color, 10f);

				LaserPool.GetPool().ReturnAmmo(gameObject);
				return;
			}
		}
		transform.position += velocity;
	}

/*	public void OnCollisionEnter(Collision other){
		var paintable = other.collider.gameObject.GetComponent<Paintable> ();
		if (paintable == null)
			return;
		Ultil.TryShootPaint (this.transform.position, flyDirection, laserLight.color, 10f);

		LaserPool.GetPool ().ReturnAmmo (this.gameObject);
	}*/
	public void SetDirection(Vector3 direction){
		// direction should be normalized
		this.flyDirection = direction;
		this.transform.rotation = Quaternion.LookRotation (direction);
	}
}

