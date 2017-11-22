using UnityEngine;

namespace script.BoidBehavior
{
	public class ObstacleSeparationBehavior : BoidBehavior
	{

		[Header("Setting")]
		[SerializeField]
		private LayerMask _obstacleLayer;

		[SerializeField]
		private float _sphereRadius = 1;

		[SerializeField] 
		private float _maxAngle = 45;

		[Header("Gizmos")] 
		[SerializeField]
		private Color _normalColor = Color.white;
		
		[SerializeField]
		private Color _hitColor = Color.red;
		
		private Vector3 _desiredVelocity = Vector3.zero;
		
		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null)
			{
				return;
			}

			Vector3 avoidanceForce;
			if (CalculateAvoidanceBarrierForce(out avoidanceForce))
			{
				_desiredVelocity = avoidanceForce.normalized * BoidController.Movement.MaxSpeed;
				SteeringForce = _desiredVelocity - BoidController.Velocity;
			}
			else
			{
				_desiredVelocity = Vector3.zero;
				SteeringForce = Vector3.zero;
			}
		}
		
		private bool CalculateAvoidanceBarrierForce(out Vector3 avoidanceForce)
		{
			RaycastHit info;
			avoidanceForce = Vector3.zero;

			Collider[] obstacleList = Physics.OverlapSphere(transform.position, _sphereRadius, _obstacleLayer);

			foreach (var obstacle in obstacleList)
			{
				var closestPoint = obstacle.ClosestPointOnBounds(transform.position);
				var avoidanceVector = transform.position - closestPoint;

				if (Vector3.Angle(avoidanceVector, Vector3.up) < _maxAngle) continue;
				
				avoidanceForce += avoidanceVector.normalized * (1 - avoidanceVector.magnitude / _sphereRadius);
			}
			
			return avoidanceForce != Vector3.zero;
		}

		private void OnDrawGizmos()
		{
			if (!IsEnable || !IsDrawGizmos) return;

			Gizmos.color = SteeringForce == Vector3.zero ? _normalColor : _hitColor;
			
			Gizmos.DrawWireSphere(transform.position, _sphereRadius);
			Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);
		}
	}
}
