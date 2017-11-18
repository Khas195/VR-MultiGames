using UnityEngine;

namespace script.BoidBehavior
{
	public class EvadeBehavior : BoidBehavior 
	{
		private Vector3 _prevTargetPosition = Vector3.zero;
		
		private Vector3 _desiredVelocity = Vector3.zero;
		
		private Vector3 _predictedTargetPosition = Vector3.zero;
		
		[Header("Gizmos")]
		[SerializeField]
		private Color _evadeColor = Color.yellow;
		
		[SerializeField]
		private Color _futureTarget = Color.red;

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null || BoidController.Target == null)
			{
				return;
			}

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
			
			float targetDistance = (targetCurPosition - transform.position).magnitude;
			float timeToTarget = targetDistance / BoidController.Movement.MaxSpeed;

			_predictedTargetPosition = targetCurPosition + targetVelocity * timeToTarget;
			_desiredVelocity = (transform.position - _predictedTargetPosition).normalized 
			                   * BoidController.Movement.MaxSpeed;

			SteeringForce = _desiredVelocity - BoidController.Velocity;

			_prevTargetPosition = targetCurPosition;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Gizmos.color = _evadeColor;
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
