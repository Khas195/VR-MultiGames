using UnityEngine;

namespace script.BoidBehavior
{
	public class WanderBehavior : BoidBehavior
	{
		[Header("Wander setting")] 
		
		[Tooltip("Radius of wander circle in front of boid")]
		[SerializeField] 
		private float _wanderRadius = 4;

		[Tooltip("Distance between boid and wander circle")]
		[SerializeField] 
		private float _wanderCircleDistance = 5;


		[SerializeField] 
		private float _randomTime = 1;
		
		[Header("Wander random point setting")]
		
		[SerializeField] 
		private bool _useYAxis = false;

		[SerializeField] 
		private bool _useRandomOnSphere = false;
		
		[Header("Wander angle setting")]
		
		[SerializeField] 
		private bool _useWanderAngle = false;
		
		[SerializeField] 
		private float _wanderAngleVariation = 15f;

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
			if (!IsEnable || BoidController == null)
			{
				return;
			}

			_wanderCirclePosition = transform.position + BoidController.Velocity.normalized * _wanderCircleDistance;
			
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

			if (!(_lastWander > _randomTime)) return;
			
			_randomPoint = _useRandomOnSphere ? Random.onUnitSphere : Random.insideUnitSphere;
				
			if (!_useYAxis)
			{
				_randomPoint.y = 0;
				_randomPoint = _useRandomOnSphere ? _randomPoint.normalized : _randomPoint;
			}
				
			_randomPoint *= _wanderRadius;
//			_randomPoint = Quaternion.LookRotation(BoidController.Velocity) * _randomPoint;
				
			_randomPoint += _wanderCirclePosition;
			_lastWander = 0;
		}

		private void UpdateRandomAngle()
		{
			_lastWander += Time.deltaTime;

			if (_lastWander > _randomTime)
			{
				_wanderAngle = Random.Range(0, 360);
				_lastWander = 0;
			}
			var quaternion = Quaternion.AngleAxis(_wanderAngle, Vector3.up);
			_randomPoint = _wanderCirclePosition + quaternion * BoidController.Velocity.normalized * _wanderRadius;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Vector3 forward = BoidController ? BoidController.Velocity.normalized : transform.forward;
				
				Gizmos.color = _wanderCircleColor;
				Gizmos.DrawLine(transform.position, transform.position 
				                                    + forward * _wanderCircleDistance);
				
				if (Application.isEditor && !Application.isPlaying)
				{
					Gizmos.DrawWireSphere(transform.position 
					                      + forward * _wanderCircleDistance, _wanderRadius);
				}
				else
				{
					Gizmos.DrawWireSphere(_wanderCirclePosition, _wanderRadius);
					
					Gizmos.color = _displacementColor;
					Gizmos.DrawLine(_wanderCirclePosition, _randomPoint);
					
					Gizmos.color = _wanderVelocity;

					if (_useWanderAngle)
					{
						Gizmos.DrawLine(transform.position, transform.position + BoidController.Velocity  + 
						                                    _randomPoint - _wanderCirclePosition);
					}
					else
					{
						Gizmos.DrawLine(transform.position, _randomPoint);
					}
				}
			}
		}
	}
}
