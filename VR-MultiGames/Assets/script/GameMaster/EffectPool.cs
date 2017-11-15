using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEffect{
	void SetColor (Color color);

	GameObject GetGameObject();
	void Reset();
}
public class EffectPool : MonoBehaviour {
	static EffectPool instance;

	[SerializeField]
	GameObject effectPrefab;

	List<IEffect> storage = new List<IEffect> ();
	List<IEffect> inUse = new List<IEffect>();

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	public static EffectPool GetPool ()
	{
		return instance;
	}

	public IEffect RequestEffect(){
		IEffect result;
		if (storage.Count > 0) {
			result = storage [storage.Count - 1];
			storage.Remove (result);
		} else {
			result = Instantiate (effectPrefab, transform).GetComponent<IEffect> ();
		}
		inUse.Add (result);
		result.GetGameObject().SetActive (true);
		return result;
	}
	public void ReturnEffect(IEffect effect){
		inUse.Remove (effect);
		storage.Add (effect);
		effect.GetGameObject().SetActive (false);
		effect.Reset ();
	}
	// Update is called once per frame
	void Update () {
		instance = this;
		if (effectPrefab.GetComponent<IEffect> () == null) {
			Debug.LogError ("Cant find IEffect interface in Ammunition Prefab - AmunitionPool");
			this.enabled = false;
		}
	}
}
