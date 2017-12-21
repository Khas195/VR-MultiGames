using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSphere : MonoBehaviour {

	public Color _fieldColor;

	private Color _currentColor;
	private List<Material> _materials = new List<Material>();
	public Renderer[] Renderers
	{
		get;
		private set;
	}
	public Renderer[] ChildRenderers
	{
		get;
		private set;
	}
	// Use this for initialization
	void Start () {
		ChildRenderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in ChildRenderers)
		{	
			_materials.AddRange(renderer.materials);
		}
		Renderers = GetComponents<Renderer>();

		foreach (var renderer in Renderers)
		{	
			_materials.AddRange(renderer.materials);
		}
	}
	
	// Update is called once per frame
	void Update () {
		_currentColor = Color.Lerp(_currentColor, _fieldColor, Time.deltaTime);

		for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetColor("_Color", _currentColor);
		}

		if (_currentColor.Equals(_fieldColor))
		{
			enabled = false;
		}
	}
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.Equals ("Player")) {
			var gunList = other.GetComponentsInChildren<PaintShooter> ();
			foreach (var gun in gunList) {
				gun.SetGunColor (_fieldColor);
			}
		}
	}
}
