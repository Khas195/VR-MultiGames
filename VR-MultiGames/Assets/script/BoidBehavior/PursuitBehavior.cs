using UnityEngine;

namespace script.BoidBehavior
{
	public class PursuitBehavior : BoidBehavior
	{
		private Vector3 _prevTargetPosition = Vector3.zero;
		
		private Vector3 _desiredVelocity = Vector3.zero;
		
		private Vector3 _predictedTargetPosition = Vector3.zero;

		private float _lastUpdate;

		[Header("Setting")]
		
		[Tooltip("Time to update the predicted position of the target")]
		[SerializeField] 
		private float _updatePredictionTime = 1;
		
		[Header("Gizmos")]
		[SerializeField]
		private Color _pursuitColor = Color.green;
		
		[SerializeField]
		private Color _futureTarget = Color.green;

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null)
			{
				return;
			}

			_lastUpdate += Time.deltaTime;

			if (_lastUpdate > _updatePredictionTime)
			{
				CalculateTargetPosition();
				_lastUpdate = 0;
			}

			_desiredVelocity = (_predictedTargetPosition - transform.position).normalized
			                   * BoidController.Movement.MaxSpeed;

			SteeringForce = _desiredVelocity - BoidController.Velocity;
		}

		private void CalculateTargetPosition()
		{
			if (!BoidController.Target) return;
			
			Vector3 targetCurPosition = BoidController.Target.transform.position;
			Vector3 targetVelocity;
			
			var targetRigidbody = BoidController.Target.GetComponent<Rigidbody>();
			
			if (targetRigidbody == null)
			{
				targetVelocity = targetCurPosition - _prevTargetPosition;
			}
			else
			{
				targetVelocity = targetRigidbody.velocity;
			}
			
			var targetDistance = (targetCurPosition - transform.position).magnitude;
			var timeToTarget = targetDistance / BoidController.Movement.MaxSpeed;
			
			_predictedTargetPosition = targetCurPosition + targetVelocity * timeToTarget;

			_prevTargetPosition = targetCurPosition;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Gizmos.color = _pursuitColor;
				Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);

				if (BoidController == null || BoidController.Target == null)
				{
					return;
				}
				
				Gizmos.color = _futureTarget;
				Gizmos.DrawSphere(_predictedTargetPosition, 0.2f);

			}
		}
	}
}
