using UnityEngine;

namespace script.BoidBehavior
{
	public class WanderBehavior : BoidBehavior
	{
		[Header("Wander setting")] 
		
		[SerializeField] 
		private bool _useWanderAngle = false;
		
		[SerializeField] 
		private bool _isDisableYAxis = false;
		
		[Tooltip("Radius of wander circle in front of boid")]
		[SerializeField] 
		private float _wanderRadius = 4;

		[Tooltip("Distance between boid and wander circle")]
		[SerializeField] 
		private float _wanderCircleDistance = 5;

		[SerializeField] 
		private float _wanderAngleVariation = 15f;

		[SerializeField] 
		private float _randomTime = 1;

		private float _wanderAngle;
		private float _lastWander = 0;
		private Vector3 _randomPoint = Vector3.zero;
		private Vector3 _wanderCirclePosition = Vector3.zero;
		private Vector3 _desiredVelocity = Vector3.zero;
		
		[Header("Gizmos")]
		[SerializeField]
		private Color _wanderCircleColor = Color.white;
		[SerializeField]
		private Color _displacementColor = Color.red;
		[SerializeField]
		private Color _wanderVelocity = Color.yellow;
		
		public override void PerformBehavior()
		{
			if (BoidController == null)
			{
				return;
			}

			_wanderCirclePosition = transform.position + transform.forward * _wanderCircleDistance;
			
			_desiredVelocity = (_randomPoint - transform.position).normalized * BoidController.Movement.MaxSpeed;
			
			SteeringForce = _desiredVelocity - BoidController.Velocity;

			if (_useWanderAngle)
			{
				UpdateRandomAngle();
			}
			else
			{
				UpdateRandomPoint();
			}
		}

		private void UpdateRandomPoint()
		{
			_lastWander += Time.deltaTime;

			if (_lastWander > _randomTime)
			{
				if (BoidController._Rigidbody.useGravity && _isDisableYAxis)
				{
					_randomPoint = Random.insideUnitCircle;
					_randomPoint.y = 0;
					_randomPoint = _randomPoint.normalized * _wanderRadius;
				}
				else
				{
					_randomPoint = Random.insideUnitCircle * _wanderRadius;
				}
				_randomPoint += _wanderCirclePosition;
				_lastWander = 0;
			}
		}

		private void UpdateRandomAngle()
		{
			_lastWander += Time.deltaTime;

			if (_lastWander > _randomTime)
			{
				_wanderAngle += Random.value * _wanderAngleVariation * (Random.Range(0, 2) - 1) - _wanderAngleVariation * 0.5f;
				if (_wanderAngle > 360f)
				{
					_wanderAngle -= 360f;
				}
				var quaternion = Quaternion.AngleAxis(_wanderAngle, Vector3.up);
				_randomPoint = _wanderCirclePosition + quaternion * transform.forward * _wanderRadius;
				_lastWander = 0;
			}
		}

		private void OnDrawGizmos()
		{
			if (IsDrawGizmos)
			{
				Gizmos.color = _wanderCircleColor;
				Gizmos.DrawLine(transform.position, transform.position + transform.forward * _wanderCircleDistance);
				
				if (Application.isEditor && !Application.isPlaying)
				{
					Gizmos.DrawWireSphere(transform.position + transform.forward * _wanderCircleDistance,
						_wanderRadius);
				}
				else
				{
					Gizmos.DrawWireSphere(_wanderCirclePosition, _wanderRadius);
					
					Gizmos.color = _displacementColor;
					Gizmos.DrawLine(_wanderCirclePosition, _randomPoint);
					
					Gizmos.color = _wanderVelocity;
					Gizmos.DrawLine(transform.position, _randomPoint);
				}
			}
		}
	}
}
