using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PaintBall : MonoBehaviour, IHighlightable, IPickupable {

	[SerializeField]
	GameObject highlightedBall;

	Rigidbody cacheRigidBody;

	Transform originParent;

	bool pickedUp;

	// Use this for initialization
	void Start () {
		cacheRigidBody = this.GetComponent<Rigidbody> ();
		originParent = this.transform.parent;
	}
	// Update is called once per frame
	void Update () {
	}
	public void TurnOn ()
	{
		this.highlightedBall.SetActive (true);
	}
	public void TurnOff ()
	{
		this.highlightedBall.SetActive (false);
	}

	#region IPickupable implementation

	public void Interact ()
	{
		throw new System.NotImplementedException ();
	}

	public void Pickup (Transform holdingTrans)
	{
		originParent = this.transform.parent;
		this.transform.SetParent (holdingTrans);
		this.transform.localPosition = Vector3.zero;
		cacheRigidBody.useGravity = false;
		cacheRigidBody.velocity = Vector3.zero;
		cacheRigidBody.isKinematic = true;
	}

	public void Drop ()
	{
		this.transform.SetParent (originParent);
		cacheRigidBody.useGravity = true;
		cacheRigidBody.isKinematic = false;
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
		RaycastHit hit;
		List<Paintable> painted = new List<Paintable> ();
		if (Physics.Raycast (transform.position, contactPoint.point, out hit, 1f + transform.localScale.x)) {
			var hitPaintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (hitPaintable != null) {
				hitPaintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), Color.red);
				painted.Add (hitPaintable);
			}
		}
		for (int i = 0; i < 14; ++i) {
			if (Physics.Raycast (contactPoint.point, Random.onUnitSphere, out hit, 1f + transform.localScale.x)) {
				var hitPaintable = hit.collider.gameObject.GetComponent<Paintable> ();
				if (hitPaintable != null && !painted.Contains(hitPaintable)) {
					hitPaintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), Color.red);
					painted.Add (hitPaintable);
				}
			}
		}
	}
	#endregion
}
