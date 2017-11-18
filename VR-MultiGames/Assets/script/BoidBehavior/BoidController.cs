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

		[Tooltip("Scale the rotation speed to look at velocity direction")] 
		[SerializeField]
		private float _rotationSyncScale = 5;
		
		[Header("Gizmos")]		
		[SerializeField]
		private Color _steeringForceColor = Color.magenta;
		
		public float MaxSteeringForce
		{
			get { return _maxSteeringForce; }
		}

		public float RotationSyncScale
		{
			get { return _rotationSyncScale; }
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
			UpdateBehavior();
			ApplyBehavior();
		}

		private void UpdateBehavior()
		{
			foreach (var behavior in _behaviorList)
			{
				if(behavior == null || !behavior.enabled) continue;
				
				behavior.PerformBehavior();
			}
		}

		private void ApplyBehavior()
		{
			Vector3 steeringForce = Vector3.zero;
			
			foreach (var behavior in _behaviorList)
			{
				if(behavior == null || !behavior.enabled) continue;

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

			if (_Rigidbody.velocity.sqrMagnitude > 1)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_Rigidbody.velocity), 
					Time.deltaTime * _rotationSyncScale);
			}
		}

		private void OnDrawGizmos()
		{
			if (IsDrawGizmos)
			{
				Gizmos.color = _steeringForceColor;
				
				Vector3 steeringForce = Vector3.zero;
			
				foreach (var behavior in _behaviorList)
				{
					if(!behavior.enabled) continue;

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
