using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Hover : MonoBehaviour
{
    public float speed;
    public float amplitude;
    // Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var pos = this.transform.position;
	    pos.y = (Mathf.Sin(Time.time * speed) * amplitude);
	    transform.position = pos;
	}
}
