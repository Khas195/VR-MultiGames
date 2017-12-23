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
			var player = other.gameObject.transform;
			player.position = _startingPoint.position;
			player.forward = _startingPoint.forward;
			player.right = _startingPoint.right;
		}
	}
}
