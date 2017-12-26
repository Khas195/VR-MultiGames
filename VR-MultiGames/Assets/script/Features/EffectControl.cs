using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class EffectControl : MonoBehaviour, IEffect {

	ParticleSystem ps;


	// Use this for initialization
	void Start () {
		ps = this.GetComponent<ParticleSystem> ();	
	}
	void OnEnable(){
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive()) {
			EffectPool.GetPool ().ReturnEffect (this);
		}
	}

	#region IEffect implementation

	public void SetColor (Color color)
	{
		if (ps == null) {
			ps = this.GetComponent<ParticleSystem> ();	
		}
		var main = ps.main;
		main.startColor = color;
	}

	public GameObject GetGameObject ()
	{
		return gameObject;
	}

	public void Reset ()
	{
		ps.Clear ();
	}

	#endregion
}

