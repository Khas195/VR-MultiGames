using System.Collections.Generic;
using System.Linq;
using script.ControllerScript;
using UnityEngine;

namespace script.BoidBehavior
{
	[RequireComponent(typeof(Rigidbody))]
	public class BoidController : Controller
	{
		private List<BoidBehavior> _behaviorList = new List<BoidBehavior>();
		
		[Header("Target")]
		[SerializeField] 
		private GameObject _target = null;

		[Header("Behavior Settings")] 
		[SerializeField] 
		private float _maxSteeringForce = 1f;
		
		[Header("Gizmos")]		
		[SerializeField]
		private Color _steeringForceColor = Color.magenta;
		
		public float MaxSteeringForce
		{
			get { return _maxSteeringForce; }
		}

		public GameObject Target
		{
			get { return _target; }
			set { _target = value; }
		}

		public Vector3 Velocity
		{
			get { return _Rigidbody.velocity; }
		}

		private void Start()
		{
			_behaviorList = GetComponents<BoidBehavior>().ToList();
		}

		private void Update()
		{
			CheckBehaviorList();
			UpdateBehavior();
			ApplyBehavior();
		}

		private void UpdateBehavior()
		{
			foreach (var behavior in _behaviorList)
			{
				if(behavior == null || !behavior.IsEnable) continue;
				
				behavior.PerformBehavior();
			}
		}

		private void ApplyBehavior()
		{
			Vector3 steeringForce = Vector3.zero;
			
			foreach (var behavior in _behaviorList)
			{
				if(behavior == null || !behavior.IsEnable) continue;

				if (_behaviorList.Count == 1)
				{
					steeringForce += behavior.SteeringForce;
				}

				steeringForce += behavior.SteeringForce * behavior.BlendScale;
			}

			if (_Rigidbody.useGravity)
			{
				steeringForce.y = 0;
			}
			
			steeringForce = Vector3.ClampMagnitude(steeringForce, _maxSteeringForce);

			Movement.Move(steeringForce);
		}

		private void CheckBehaviorList()
		{
			_behaviorList = GetComponents<BoidBehavior>().ToList();
		}

		private void OnDrawGizmos()
		{
			if (IsDrawGizmos)
			{
				Gizmos.color = _steeringForceColor;
				
				Vector3 steeringForce = Vector3.zero;
			
				foreach (var behavior in _behaviorList)
				{
					if(!behavior.IsEnable) continue;

					if (_behaviorList.Count == 1)
					{
						steeringForce += behavior.SteeringForce;
					}

					steeringForce += behavior.SteeringForce * behavior.BlendScale;
				}

				Gizmos.DrawLine(transform.position, transform.position + steeringForce);
			}
		}
	}
}
