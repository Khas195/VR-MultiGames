using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowComposite : MonoBehaviour {
	[Range (0, 10)]
	public float Intensity = 2;

	float fullBlur;
	private Material _compositeMat;

	public float LerpFactor = 10;
	[SerializeField]
	 UnityEngine.Events.UnityEvent AllGlow;
	float target = 0;
	void OnEnable()
	{
		_compositeMat = new Material(Shader.Find("Hidden/GlowComposite"));
	}

	void Update(){
		fullBlur = Mathf.Lerp (0, target, Time.deltaTime * LerpFactor);
		if (Input.GetKeyDown(KeyCode.Q)){
			target = target == 1 ? 0 : 1;
			AllGlow.Invoke ();
		}
	}
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		_compositeMat.SetFloat("_Intensity", Intensity);
		_compositeMat.SetFloat("_FullBlur", fullBlur);
		Graphics.Blit(src, dst, _compositeMat, 0);
	}
}
