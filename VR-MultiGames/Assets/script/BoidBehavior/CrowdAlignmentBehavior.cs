using System.Collections.Generic;
using UnityEngine;

namespace script.BoidBehavior
{
	public class CrowdAlignmentBehavior : BoidBehavior
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
		private Color _alignmentForceColor = Color.green;
		
		[SerializeField]
		private Color _sphereColor = Color.green;
		
		public override void PerformBehavior()
		{
			if (BoidController == null) return;

			Vector3 alignmentVelocity;
			if (CalculateAlignment(out alignmentVelocity))
			{
				_desiredVelocity = Vector3.ClampMagnitude(alignmentVelocity, BoidController.Movement.MaxSpeed);
				SteeringForce = _desiredVelocity - BoidController.Velocity;
			}
			else
			{
				SteeringForce = Vector3.zero;
			}
		}

		private bool CalculateAlignment(out Vector3 alignmentVelocity)
		{
			var boidList = BoidUnit.BoidList;
			alignmentVelocity = Vector3.zero;
			var neighbourList = BoidUnit.GetNeighbour(gameObject, _neighbourRadius);

			if (neighbourList.Count < _minNeightbour)
			{
				return false;
			}

			foreach (var neighbour in neighbourList)
			{
				if (_isAdaptSpeedToCrowdSpeed)
				{
					alignmentVelocity += neighbour.Velocity;
				}
				else
				{
					alignmentVelocity += neighbour.Orientation;
				}
			}

			alignmentVelocity /= neighbourList.Count;
			return true;
		}

		private void OnDrawGizmos()
		{
			if (IsDrawGizmos)
			{
				Gizmos.color = _alignmentForceColor;
				Gizmos.DrawLine(transform.position, transform.position + SteeringForce);

				Gizmos.color = _sphereColor;
				Gizmos.DrawWireSphere(transform.position, _neighbourRadius);
			}
		}
	}
}
