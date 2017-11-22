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
			if (!IsEnable || BoidController == null)
			{
				return;
			}
			
			Vector3 arrivalForce;
			float slowingFactor;
			
			CalculateArrivalForce(out arrivalForce, out slowingFactor);

			_desiredVelocity = arrivalForce.normalized * BoidController.Movement.MaxSpeed * slowingFactor;

			SteeringForce = _desiredVelocity - BoidController.Velocity;
		}

		private bool CalculateArrivalForce(out Vector3 arrivalForce, out float slowingFactor)
		{
			arrivalForce = Vector3.zero;
			slowingFactor = 0;
			
			if (BoidController.Target == null) return false;
			
			arrivalForce = (BoidController.Target.transform.position - transform.position);

			if (_slowingDistance > 0)
			{
				slowingFactor = Mathf.Clamp(arrivalForce.magnitude / _slowingDistance, 0f, 1f);
			}
			else
			{
				slowingFactor = Mathf.Clamp(arrivalForce.magnitude, 0f, 1f);
			}

			return true;
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
