using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hover : MonoBehaviour
{
    public float speed;
    public float amplitude;
    [SerializeField]
    private Vector3 pivotPos;

    // Use this for initialization
	void Start ()
	{
	    pivotPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var pos = this.transform.position;
	    pos.y = pivotPos.y + (Mathf.Sin(Time.time * speed) * amplitude);
	    transform.position = pos;
	}
}
