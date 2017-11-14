using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;


public class PaintBall : MonoBehaviour {
	[SerializeField]
	float splashRange;
	[SerializeField]
	GameObject effectPrototype;

	Color color;
	Material mat;

	Rigidbody cacheRigidBody;

	// Use this for initialization
	void Awake(){
		cacheRigidBody = this.GetComponent<Rigidbody> ();
	}
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	}

	public void ApplyForce (Vector3 direction)
	{
		cacheRigidBody.velocity = Vector3.zero;	
		cacheRigidBody.AddForce (direction, ForceMode.Impulse);;
	}

	public void OnDeactive ()
	{
		gameObject.SetActive (false);
	}

	public void OnActive ()
	{
		gameObject.SetActive (true);
		SetColor (GameSettings.GetInstance().GetRandomColor());
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

		var result = TryShootPaint (transform.position, cacheRigidBody.velocity.normalized * splashRange); 
		if (result != null) {
			painted.Add (result);
		}


		for (int i = 0; i < GameSettings.GetInstance().NumberOfSplash; ++i) {
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

	public void SetColor (Color color)
	{
		this.color = color;
		if (mat == null) {
			mat = Ultil.GetMaterialWithShader (GetComponent<Renderer> ().materials, PaintableDefinition.BulletShader, name);
		}
		mat.SetColor (PaintableDefinition.ColorProperty, color);
	}

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
