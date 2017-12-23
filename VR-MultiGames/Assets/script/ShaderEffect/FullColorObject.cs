using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Paintable))]
public class FullColorObject : MonoBehaviour
{
    [SerializeField] private Color color;
    private Paintable paintable;

    // Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
