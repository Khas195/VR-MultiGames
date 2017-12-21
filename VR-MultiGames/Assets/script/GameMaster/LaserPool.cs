using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaserPool : MonoBehaviour {

	static LaserPool instance;

	[SerializeField]
	GameObject ammoPrefab;
	[SerializeField]
	float returnRadius;
	List<GameObject> storage = new List<GameObject> ();
	List<GameObject> inUse = new List<GameObject>();

	public static LaserPool GetPool(){
		return instance;
	}
	public void Start(){
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireSphere(transform.position, returnRadius);
	}

	void HandleOutOfBoundLaser ()
	{
		var returnList = new List<GameObject> ();
		foreach (var o in inUse) {
			if (Vector3.Distance (o.transform.position, transform.position) > returnRadius) {
				returnList.Add (o);
			}
		}
		foreach (var ro in returnList) {
			ReturnAmmo (ro);
		}
	}

	public void Update(){
		HandleOutOfBoundLaser ();
	}
	public GameObject RequestAmmo(){
		GameObject result;
		if (storage.Count > 0) {
			result = storage [storage.Count - 1];
			storage.Remove (result);
		} else {
			result = Instantiate (ammoPrefab, transform);
		}
		inUse.Add (result);
		result.SetActive (true);
		return result;
	}
	public void ReturnAmmo(GameObject ammo){
		inUse.Remove (ammo);
		storage.Add (ammo);
		ammo.SetActive (false);
	}

	void Awake(){
		instance = this;
	}

}
