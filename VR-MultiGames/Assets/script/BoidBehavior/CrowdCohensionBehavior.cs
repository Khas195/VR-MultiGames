using System.Collections.Generic;
using UnityEngine;

namespace script.BoidBehavior
{
	public class CrowdCohensionBehavior : BoidBehavior 
	{
		[SerializeField] 
		private bool _isAdaptSpeedToCrowdSpeed = false;
		
		[Tooltip("Minimum number of boid to consider them as a crowd")]
		[SerializeField] 
		private uint _minNeightbour = 1;
		
		[SerializeField]
		private float _neighbourRadius = 6;

		private Vector3 _desiredVelocity = Vector3.zero;
		
		[Header("Gizmos")]		
		[SerializeField]
		private Color _cohensionForceColor = Color.blue;
		
		[SerializeField]
		private Color _sphereColor = Color.blue;

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null) return;

			Vector3 cohensionPoint, averageVelocity;
			if (CalculateCohension(out cohensionPoint, out averageVelocity))
			{
				if (_isAdaptSpeedToCrowdSpeed)
				{
					_desiredVelocity = (cohensionPoint - transform.position).normalized * averageVelocity.magnitude;
					_desiredVelocity = Vector3.ClampMagnitude(_desiredVelocity, BoidController.Movement.MaxSpeed);
				}
				else
				{
					_desiredVelocity = (cohensionPoint - transform.position).normalized * BoidController.Movement.MaxSpeed;
				}
				SteeringForce = _desiredVelocity - BoidController.Velocity;
			}
			else
			{
				SteeringForce = Vector3.zero;
			}
		}

		private bool CalculateCohension(out Vector3 cohensionPoint, out Vector3 averageVelocity)
		{
			cohensionPoint = Vector3.zero;
			averageVelocity = Vector3.zero;
			var neighbourList = BoidUnit.GetNeighbour(gameObject, _neighbourRadius);

			if (neighbourList.Count < _minNeightbour)
			{
				return false;
			}

			foreach (var neighbour in neighbourList)
			{
				cohensionPoint += neighbour.transform.position;
				averageVelocity += neighbour.Velocity;
			}

			cohensionPoint /= neighbourList.Count;
			averageVelocity /= neighbourList.Count;

			return true;
		}

		private void OnDrawGizmos()
		{
			if (IsEnable && IsDrawGizmos)
			{
				Gizmos.color = _cohensionForceColor;
				Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);

				Gizmos.color = _sphereColor;
				Gizmos.DrawWireSphere(transform.position, _neighbourRadius);
			}
		}
	}
}
