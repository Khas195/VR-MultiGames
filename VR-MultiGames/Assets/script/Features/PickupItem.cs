using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable {
	void Interact();
	void Pickup(Transform holdingTrans);
	void Drop();

}
public class PickupItem : MonoBehaviour {

	[SerializeField]
	Camera lookAtCamera;
	[SerializeField]
	KeyCode trigger;
	[SerializeField]
	Transform pickupPos;
	[SerializeField]
	public ItemPickupEvent OnItemPickup;
	[SerializeField]
	public ItemDropEvent OnItemDrop;

	IPickupable curPickup;
	GameObject curPickupObject;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		HandlePickup ();
	}

	void HandlePickup ()
	{
		if (!Input.GetKeyDown (trigger))
			return;
		if (curPickup == null) {
			Pickup ();
		} else {
			DropItem ();
		}
	}

	void Pickup ()
	{
		if (!Physics.Raycast (lookAtCamera.transform.position, lookAtCamera.transform.forward, out hit))
			return;
		var targetPickup = hit.collider.gameObject.GetComponent<IPickupable> ();
		if (targetPickup == curPickup || targetPickup == null)
				return;
	
		targetPickup.Pickup (pickupPos);
		OnItemPickup.Invoke (hit.collider.gameObject);
		curPickupObject = hit.collider.gameObject;
		curPickup = targetPickup;
	}

	void DropItem ()
	{
		curPickup.Drop ();
		OnItemDrop.Invoke (curPickupObject);
		curPickupObject = null;
		curPickup = null;
	}
}
