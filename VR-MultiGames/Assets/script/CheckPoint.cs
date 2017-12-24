using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	[SerializeField] private Transform _point = null;

	public Transform point
	{
		get { return _point; }
		set { _point = value; }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!_point)
			{
				Respawn.respawnPoint = this.transform;
			}
			else
			{
				Respawn.respawnPoint = _point;
			}
		}
	}
}
