using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAmmunition {
	void Init(Color color);
	GameObject GetGameObject();
	void Reset();

}
public class AmunitionPool : MonoBehaviour {

	static AmunitionPool instance;

	[SerializeField]
	GameObject ammoPrefab;

	List<IAmmunition> storage = new List<IAmmunition> ();
	List<IAmmunition> inUse = new List<IAmmunition>();

	public static AmunitionPool GetPool(){
		return instance;
	}

	public IAmmunition RequestAmmo(){
		IAmmunition result;
		if (storage.Count > 0) {
			result = storage [storage.Count - 1];
			storage.Remove (result);
		} else {
			result = Instantiate (ammoPrefab, transform).GetComponent<IAmmunition>();
		}
		inUse.Add (result);
		result.GetGameObject().SetActive (true);
		result.Init (GameSettings.GetInstance().GetRandomColor());
		return result;
	}
	public void ReturnAmmo(IAmmunition ammo){
		inUse.Remove (ammo);
		storage.Add (ammo);
		ammo.GetGameObject().SetActive (false);
		ammo.Reset ();
	}

	void Awake(){
		instance = this;
		if (ammoPrefab.GetComponent<IAmmunition> () == null) {
			Debug.LogError ("Cant find IAmmunition interface in Ammunition Prefab - AmunitionPool");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
