using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Glowable : MonoBehaviour {

	public Color GlowColor;
	public float LerpFactor = 10;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;

	void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in Renderers)
		{	
			_materials.AddRange(renderer.materials);
		}
	}
	bool glowable = true;
	public void Glow()
	{
		if (!glowable)
			return;
		_targetColor = GlowColor;
		enabled = true;
	}

	public void StopGlow()
	{
	    if (_currentColor == Color.black) return;
		_targetColor = Color.black;
		enabled = true;
	}   
	public void NolongerGlow(){
		glowable = false;
	}
	/// <summary>
	/// Loop over all cached materials and update their color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		foreach (var mat in _materials)
		{
		    mat.SetColor("_GlowColor", _currentColor);
		}

		if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}
	}

    public bool IsGlowing()
    {
        return _currentColor == Color.black;
    }
}
