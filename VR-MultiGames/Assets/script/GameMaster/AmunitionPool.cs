using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmunitionPool : MonoBehaviour {

	static AmunitionPool instance;

	[SerializeField]
	GameObject ammoPrefab;

	List<PaintBall> storage = new List<PaintBall> ();
	List<PaintBall> inUse = new List<PaintBall>();

	public static AmunitionPool GetPool(){
		return instance;
	}

	public PaintBall RequestAmmo(){
		PaintBall result;
		if (storage.Count > 0) {
			result = storage [storage.Count - 1];
			storage.Remove (result);
		} else {
			result = Instantiate (ammoPrefab, transform).GetComponent<PaintBall>();
		}
		inUse.Add (result);
		result.OnActive ();
		return result;
	}
	public void ReturnAmmo(PaintBall ammo){
		inUse.Remove (ammo);
		storage.Add (ammo);
		ammo.OnDeactive ();
	}

	void Awake(){
		instance = this;
		if (ammoPrefab.GetComponent<PaintBall> () == null) {
			Debug.LogError ("Cant find Paintball  in Paintball Prefab - AmunitionPool");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
