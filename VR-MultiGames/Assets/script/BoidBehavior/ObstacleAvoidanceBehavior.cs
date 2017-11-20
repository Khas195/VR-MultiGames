using UnityEngine;

namespace script.BoidBehavior
{
	public class ObstacleAvoidanceBehavior : BoidBehavior
	{
		[Header("Setting")]
		[SerializeField]
		private LayerMask _obstacleLayer;

		[SerializeField]
		private float _viewSphereRadius = 1;

		[SerializeField]
		private float _viewSphereMaxDistance = 10;

		[SerializeField]
		private float _maxFloorAngle = 45;

		[SerializeField]
		private float _spherecastOffset = 1;

		[Header("Gizmos")] 
		[SerializeField]
		private Color _normalViewSphereColor = Color.white;
		
		[SerializeField]
		private Color _badViewSphereColor = Color.red;
		
		[SerializeField]
		private Color _goodViewSphereColor = Color.green;
		
		private Vector3 _desiredVelocity = Vector3.zero;
		private Vector3 _viewSpherePosition = Vector3.zero;
		

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null)
			{
				return;
			}

			Vector3 avoidanceForce;
			float viewSphereDistance;
			if (CalculateAvoidanceForce(out avoidanceForce, out viewSphereDistance))
			{
				_desiredVelocity = avoidanceForce.normalized * BoidController.Movement.MaxSpeed
					* (1 - viewSphereDistance / (_viewSphereMaxDistance + _spherecastOffset));
				SteeringForce = _desiredVelocity - BoidController.Velocity;
			}
			else
			{
				SteeringForce = Vector3.zero;
			}
		}

		private bool CalculateAvoidanceForce(out Vector3 avoidanceForce, out float viewSphereDistance)
		{
			RaycastHit info;
			avoidanceForce = Vector3.zero;
			viewSphereDistance = 0;

			if (Physics.SphereCast(transform.position - BoidController.Velocity.normalized
			                       * _spherecastOffset, _viewSphereRadius, BoidController.Velocity.normalized,
				out info, _viewSphereMaxDistance + _spherecastOffset, _obstacleLayer))
			{
				_viewSpherePosition = transform.position + BoidController.Velocity.normalized * info.distance;
				viewSphereDistance = info.distance;
				
				if (Vector3.Angle(info.normal, transform.up) > _maxFloorAngle ||
				    Vector3.Angle(info.normal, Vector3.up) > _maxFloorAngle)
				{
					avoidanceForce = Vector3.Reflect(BoidController.Velocity, info.normal);
					return true;
				}
			}
			else
			{
				_viewSpherePosition = Vector3.zero;
			}
			return false;
		}

		private void OnDrawGizmos()
		{
			if (!IsEnable || !IsDrawGizmos) return;
			
			if (_viewSpherePosition == Vector3.zero)
			{
				Gizmos.color = _normalViewSphereColor;
				Gizmos.DrawLine(transform.position - BoidController.Velocity.normalized * _spherecastOffset, 
					transform.position - BoidController.Velocity.normalized * _spherecastOffset
					+ BoidController.Velocity.normalized * (_viewSphereMaxDistance + _spherecastOffset));
				
				Gizmos.DrawWireSphere(transform.position - BoidController.Velocity.normalized * _spherecastOffset
				                      + BoidController.Velocity.normalized * (_viewSphereMaxDistance + _spherecastOffset), 
					_viewSphereRadius);
			}
			else
			{
				Gizmos.color = SteeringForce == Vector3.zero ? _goodViewSphereColor : _badViewSphereColor;
				
				Gizmos.DrawLine(transform.position - BoidController.Velocity.normalized * _spherecastOffset, 
					transform.position - BoidController.Velocity.normalized * _spherecastOffset
					+ BoidController.Velocity.normalized * (_viewSphereMaxDistance + _spherecastOffset));
				
				Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);
				Gizmos.DrawWireSphere(_viewSpherePosition, _viewSphereRadius);
			}
		}
	}
}
