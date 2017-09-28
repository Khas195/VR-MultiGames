using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField]
    private Material paintableMaterial;
    [SerializeField]
    private Material dissolveMat;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float max;
	[SerializeField]
    private float currentY = 0;
    private bool doOnce = false;

	// Use this for initialization
	void Start () {

	    Debug.Log(dissolveMat.GetShaderPassEnabled("Test"));
        dissolveMat.SetShaderPassEnabled("Test", false);

	    Debug.Log(dissolveMat.GetShaderPassEnabled("Test"));
    }
	
	// Update is called once per frame
	void Update () {
	    if (currentY < max)
	    {
	        dissolveMat.SetFloat("_DissolveY", currentY);
	        currentY += Time.deltaTime * speed;
	    }
	    else
	    {
	        if (!doOnce)
	        {
	            Material[] mats = this.GetComponent<Renderer>().materials;
	            mats[0] = paintableMaterial;
	            this.GetComponent<Renderer>().materials = mats;
	            this.GetComponent<Paintable>().Init();
	            this.GetComponent<Paintable>().enabled = true;
	            doOnce = true;
	        }
	    }
	}
}
