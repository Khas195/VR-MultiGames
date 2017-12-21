using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Laser : MonoBehaviour, IBullet
{
	Vector3 shootOrigin;
	Vector3 flyDirection;
	float flySpeed;
	[SerializeField]
	Light laserLight;

	[SerializeField] 
	private LayerMask _paintableLayermMask;

	List<Material> materials = new List<Material>();
	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public void Start(){
		Renderers = GetComponents<Renderer>();
		foreach (var renderer in Renderers)
		{	
			materials.AddRange(renderer.materials);
		}	
	}
	public void SetSpeed (float laserSpeed)
	{
		this.flySpeed = laserSpeed;
	}
	public void SetLaserColor (Color laserColor)
	{
		this.laserLight.color = laserColor;
		foreach (var mat in materials) {
			mat.SetColor ("_Color", laserColor);
		}
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
		
	public void SetDirection(Vector3 direction){
		// direction should be normalized
		this.flyDirection = direction;
		this.transform.rotation = Quaternion.LookRotation (direction);
	}
	
	
	#region IBullet implementation
	public void SetColor (Color newColor)
	{
		SetLaserColor (newColor);
	}
	#endregion
}

