using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
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
		dissolveMat.SetInt ("_DoDissolve", 1);
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
				dissolveMat.SetInt ("_DoDissolve", 0);
				this.GetComponent<Paintable> ().Init ();
	            doOnce = true;
	        }
	    }
	}
}
