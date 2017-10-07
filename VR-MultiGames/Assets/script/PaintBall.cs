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

	#endregion
}
