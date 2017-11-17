using UnityEngine;

namespace script.BoidBehavior
{
	public class CrowdSeparationBehavior : BoidBehavior 
	{
		[Tooltip("Minimum number of boid to consider them as a crowd")]
		[SerializeField] 
		private uint _minNeightbour = 1;
		
		[SerializeField]
		private float _neighbourRadius = 4;

		private Vector3 _desiredVelocity = Vector3.zero;
		
		[Header("Gizmos")]		
		[SerializeField]
		private Color _separationForceColor = Color.red;
		
		[SerializeField]
		private Color _sphereColor = Color.red;
		
		public override void PerformBehavior()
		{
			if (BoidController == null) return;

			Vector3 separationForce;
			if (CalculateSeparation(out separationForce))
			{
				_desiredVelocity = separationForce.normalized * BoidController.Movement.MaxSpeed;
				SteeringForce = _desiredVelocity - BoidController.Velocity;
			}
			else
			{
				SteeringForce = Vector3.zero;
			}
		}

		private bool CalculateSeparation(out Vector3 separationForce)
		{
			var neighbourList = BoidUnit.GetNeighbour(gameObject, _neighbourRadius);
			separationForce = Vector3.zero;

			if (neighbourList.Count < _minNeightbour)
			{
				return false;
			}

			foreach (var neighbour in neighbourList)
			{
				
				Vector3 temp = transform.position - neighbour.transform.position;
				temp *= 1 - Mathf.Min(temp.sqrMagnitude / (_neighbourRadius * _neighbourRadius), 1);
				separationForce += temp;
			}

			separationForce /= neighbourList.Count;
			return true;
		}

		private void OnDrawGizmos()
		{
			if (IsDrawGizmos)
			{
				Gizmos.color = _separationForceColor;
				Gizmos.DrawLine(transform.position, transform.position + SteeringForce);

				Gizmos.color = _sphereColor;
				Gizmos.DrawWireSphere(transform.position, _neighbourRadius);
			}
		}
	}
}
