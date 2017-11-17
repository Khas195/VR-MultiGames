using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
	[SerializeField] private Transform _startingPoint;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.transform.position = _startingPoint.position;
			other.transform.forward = _startingPoint.forward;
			other.transform.right = _startingPoint.right;
		}
	}
}
