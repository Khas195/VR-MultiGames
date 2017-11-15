using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DissolveEffect : MonoBehaviour
{
    [SerializeField]
    private Shader dissolveShader;
	[SerializeField]
	float dissolveSize;
	[SerializeField]
	float currentY;
	[SerializeField]
	float dissolveMax;
	[SerializeField]
	float duration;
	[SerializeField]
	float startDissolve;
	[SerializeField]
	Texture2D dissolveTexture;

	float speed;
	// Use this for initialization
	void Start () {
    }
	void OnEnable(){
		if (dissolveShader != null) {
			this.GetComponent<Camera> ().SetReplacementShader (dissolveShader, "RenderType");
			Shader.SetGlobalFloat ("_DissolveY", currentY);
			Shader.SetGlobalFloat("_StartingY", startDissolve);
			Shader.SetGlobalFloat("_DissolveSize", dissolveSize);
			Shader.SetGlobalTexture("_DissolveTexture", dissolveTexture);
		}
		currentY = startDissolve;
		speed = (dissolveMax - startDissolve) / duration;
	}
	void OnDisable(){
		this.GetComponent<Camera> ().ResetReplacementShader ();
	}
	// Update is called once per frame
	void Update () {
		currentY += speed * Time.deltaTime;
		Shader.SetGlobalFloat ("_DissolveY", currentY);
		if (currentY >= dissolveMax) {
			this.enabled = false;
		}
	}
}
