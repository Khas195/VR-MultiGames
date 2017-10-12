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
	[SerializeField]
	float throwForce;
	IPickupable curPickup;
	GameObject curPickupObject;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (trigger)) {
			HandlePickup ();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			if (!Physics.Raycast (lookAtCamera.transform.position, lookAtCamera.transform.forward, out hit))
				return;
			var paintable = hit.collider.gameObject.GetComponent<Paintable> ();
			if (paintable == null)
				return;

			paintable.PaintMapping (hit.textureCoord2, GameSettings.GetInstance ().GetRandomInk (), Color.red);
		}
		if (curPickup != null && Input.GetMouseButtonDown(0)) {
			Throw ();
		}
	}

	void Throw ()
	{
		var target = curPickupObject;
		DropItem ();
		target.GetComponent<Rigidbody> ().AddForce (Camera.main.transform.forward * throwForce * 10, ForceMode.Impulse);
	}

	void HandlePickup ()
	{
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
