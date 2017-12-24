using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Spin : MonoBehaviour
{

    public Vector3 axis;
    public float speed;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(axis * speed * Time.deltaTime);
	}
}
