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

	List<GameObject> storage = new List<GameObject> ();
	List<GameObject> inUse = new List<GameObject>();

	public static LaserPool GetPool(){
		return instance;
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
