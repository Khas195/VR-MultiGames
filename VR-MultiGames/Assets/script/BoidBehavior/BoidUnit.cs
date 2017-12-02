using System.Collections.Generic;
using UnityEngine;

namespace script.BoidBehavior
{
	[RequireComponent(typeof(Rigidbody))]
	public class BoidUnit : MonoBehaviour
	{
		private static List<BoidUnit> _boidList = new List<BoidUnit>();
		private Rigidbody _rigidbody = null;

		public Vector3 Velocity
		{
			get { return _rigidbody.velocity; }
		}

		public Vector3 Orientation
		{
			get { return transform.forward; }	
		}

		public static List<BoidUnit> BoidList
		{
			get { return _boidList; }
		}

		public static List<BoidUnit> GetNeighbour(GameObject gobject, float radius)
		{
			List<BoidUnit> neighbourList = new List<BoidUnit>();
			float sqrRadius = radius * radius;
			Vector3 origin = gobject.transform.position;
			
			foreach (var boid in _boidList)
			{
				if(boid == null || boid.gameObject == gobject) continue;

				float sqrDist = (boid.transform.position - gobject.transform.position).sqrMagnitude;

				if (sqrDist <= sqrRadius)
				{
					neighbourList.Add(boid);
				}
			}

			return neighbourList;
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			if (!_boidList.Contains(this))
			{
				_boidList.Add(this);
			}
		}

		private void OnEnable()
		{
			if (!_boidList.Contains(this))
			{
				_boidList.Add(this);
			}
		}

		private void OnDisable()
		{
			if (_boidList.Contains(this))
			{
				_boidList.Remove(this);
			}
		}

		private void OnDestroy()
		{
			if (_boidList.Contains(this))
			{
				_boidList.Remove(this);
			}
		}
	}
}
