using UnityEngine;

namespace script.BoidBehavior
{
	public class SeekBehavior : BoidBehavior 
	{
		private Vector3 _desiredVelocity = Vector3.zero;
		
		[Header("Gizmos")]
		[SerializeField]
		private Color _seekColor = Color.green;
		
		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null || BoidController.Target == null)
			{
				return;
			}

			_desiredVelocity = (BoidController.Target.transform.position - transform.position).normalized *
			                   BoidController.Movement.MaxSpeed;

			SteeringForce = _desiredVelocity - BoidController.Velocity;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Gizmos.color = _seekColor;
				Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);
			}
		}
	}
}
