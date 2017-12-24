using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
	[SerializeField] private static Transform _respawnPoint = null;

	public static Transform respawnPoint
	{
		get { return _respawnPoint; }
		set { _respawnPoint = value; }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			var player = other.gameObject.transform;
			player.position = _respawnPoint.position;
			player.forward = _respawnPoint.forward;
			player.right = _respawnPoint.right;
		}
	}
}
