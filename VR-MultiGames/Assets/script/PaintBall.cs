using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;


public class PaintBall : MonoBehaviour, IAmmunition {
	[SerializeField]
	float splashRange;
	[SerializeField]
	GameObject effectPrototype;

	Color color;
	Material mat;

	Rigidbody cacheRigidBody;

	// Use this for initialization
	void Start () {
		cacheRigidBody = this.GetComponent<Rigidbody> ();
	}
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision other){     

		var paintable = other.collider.gameObject.GetComponent<Paintable> ();
		if (paintable == null)
			return;


		foreach (ContactPoint cp in other.contacts)
		{
			Explode (cp);
		}
	}
		
	void Explode (ContactPoint contactPoint)
	{
		List<Paintable> painted = new List<Paintable> ();

		var result = TryShootPaint (transform.position, transform.TransformDirection(contactPoint.point * splashRange)); 
		if (result != null) {
			painted.Add (result);
		}


		for (int i = 0; i < 14; ++i) {
			result = TryShootPaint (transform.position, transform.TransformDirection(Random.onUnitSphere * splashRange)); 
			if (result != null) {
				painted.Add (result);
			}
		}
		if (painted.Count > 0) {
			CreateEffect ();
			AmunitionPool.GetPool ().ReturnAmmo (this);
		}
	}

	void CreateEffect ()
	{
		var effect = EffectPool.GetPool ().RequestEffect ();
		effect.SetColor (color);
		effect.GetGameObject().transform.position = transform.position;
	}

	#region IAmmunition implementation

	public void Init (Color color)
	{
		this.color = color;
		if (mat == null) {
			mat = Ultil.GetMaterialWithShader (GetComponent<Renderer> ().materials, PaintableDefinition.BulletShader, name);
		}
		mat.SetColor (PaintableDefinition.ColorProperty, color);
	}

	public void Reset ()
	{
		
	}


	public GameObject GetGameObject ()
	{
		return gameObject;
	}
	#endregion

	Paintable TryShootPaint (Vector3 position, Vector3 direction)
	{		
		RaycastHit hit;
		if (Physics.Raycast (position, direction, out hit,  splashRange )) {
			var hitPaintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (hitPaintable != null && hitPaintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), color)) {
				return hitPaintable;
			}
		}
		return null;
	}
}
