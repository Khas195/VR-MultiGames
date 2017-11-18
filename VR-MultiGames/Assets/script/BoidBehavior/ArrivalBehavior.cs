using UnityEngine;

namespace script.BoidBehavior
{
	public class ArrivalBehavior : BoidBehavior
	{
		[SerializeField] 
		private float _slowingDistance = 10;
		
		[Header("Gizmos")]
		[SerializeField]
		private Color _arrivalColor = Color.yellow;
		
		[SerializeField]
		private Color _arrivalSphereColor = Color.white;
		
		
		private Vector3 _desiredVelocity = Vector3.zero;

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null || BoidController.Target == null)
			{
				return;
			}

			Vector3 targetVector = (BoidController.Target.transform.position - transform.position);
			float stoppingFactor;
			
			if (_slowingDistance > 0)
			{
				stoppingFactor = Mathf.Clamp(targetVector.magnitude / _slowingDistance, 0f, 1f);
			}
			else
			{
				stoppingFactor = Mathf.Clamp(targetVector.magnitude, 0f, 1f);
			}

			_desiredVelocity = targetVector.normalized * BoidController.Movement.MaxSpeed * stoppingFactor;

			SteeringForce = _desiredVelocity - BoidController.Velocity;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Gizmos.color = _arrivalColor;
				Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);

				if (BoidController == null || BoidController.Target == null) return;
				
				Gizmos.color = _arrivalSphereColor;
				Gizmos.DrawWireSphere(BoidController.Target.transform.position, _slowingDistance);
			}
		}
	}
}
