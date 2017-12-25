using System.Collections.Generic;
using script.PathFinding;
using UnityEngine;

namespace script.BoidBehavior
{
	public class PathFollowBehavior : BoidBehavior
	{
		[Header("Settings")]
		[SerializeField] 
		private PathFinder _pathFinder = null;
		
		[SerializeField]
		private PathInspector _pathInspector = null;

		// Normal scale on anticipation
		[Tooltip("Anticpation distance scale importance to smooth replacement")]
		[SerializeField]
		private float _normalScaleOnAnticipation = 1.5f;

		// Steering force scale on anticipation
		[Tooltip("Anticpation result force importance to smooth replacement")]
		[SerializeField]
		private float _steeringForceScaleOnAnticipation = 0.01f;

		[SerializeField]
		private Path _path;

		[SerializeField] 
		private bool _isInOrder;

		[SerializeField] 
		private int _maxOrderOffset = 2;
		
		[SerializeField]
		private float _minDistanceFromPoint = 1;
		
		[Header("Gizmos")]
		
		[SerializeField]
		private Color _normalPointColor = Color.blue;
		
		[SerializeField]
		private Color _desiredVelocityColor = Color.cyan;
		
		private int _curIndex = 0;
		private Vector3 _normalPoint = Vector3.zero;
		private Vector3 _desiredVelocity = Vector3.zero;

		public Path path
		{
			get { return _path; }
			set
			{
				_path = value;
				if (_path.pointList.Count > 0)
				{
					_normalPoint = _path.precalculatedPath[0];
					_curIndex = 0;
				}
			}
		}

		private void Start()
		{
			if (_path.pointList.Count == 0)
			{
				if (_pathInspector)
				{
					_path = _pathInspector.path;
				}
				else if (_pathFinder && BoidController.Target != null)
				{
					List<PathPoint> pointList;

					_pathFinder.CalculatePath(transform.position, BoidController.Target.transform.position, out pointList);

					_path = new Path {pointList = pointList};
				}

				if (_path.pointList.Count > 0)
				{
					_normalPoint = _path.precalculatedPath[0];
				}
			}
		}

		public override void PerformBehavior()
		{
			if (!IsEnable || BoidController == null) return;

			float factor = 1;

			if (CalculatePathFollowVelocity(out _normalPoint, out factor))
			{
				_desiredVelocity = (_normalPoint - transform.position).normalized * BoidController.Movement.MaxSpeed;
				SteeringForce = _desiredVelocity - BoidController.Velocity;
				SteeringForce *= factor;
			}
			else
			{
				SteeringForce = _desiredVelocity = Vector3.zero;
			}
		}
		
		private bool CalculatePathFollowVelocity(out Vector3 normalPoint, out float factor)
		{
			var saveNormal = _normalPoint;
			normalPoint = Vector3.zero;
			factor = 1;

			if (_path.pointList.Count < 2) return false;

			var predictedPosition = transform.position + BoidController.Velocity;

			Vector3 prevPoint;
			Vector3 nextPoint;
			Vector3 nearestPoint;

			if ((saveNormal - transform.position).sqrMagnitude < _minDistanceFromPoint * _minDistanceFromPoint)
			{
				if (_isInOrder)
				{
					int start = _curIndex + 1, end;

					if (_path.pathType == Path.PathType.BezierCurve)
					{
						end = start + _maxOrderOffset * _path.bezierCurveStepNum;
					}
					else
					{
						end = start + _maxOrderOffset;
					}

					nearestPoint = _path.GetNearestPoint(predictedPosition, start, end, out _curIndex, out prevPoint, out nextPoint);
				}
				else
				{
					nearestPoint = _path.GetNearestPoint(predictedPosition, -1, -1, out _curIndex, out prevPoint, out nextPoint);
				}
			
				var normalPointA = Path.GetNormalPoint(predictedPosition, prevPoint, nearestPoint);
				var normalPointB = Path.GetNormalPoint(predictedPosition, nearestPoint, nextPoint);

				normalPoint = GetNearestPoint(predictedPosition, normalPointA, normalPointB);
			}
			else
			{
				normalPoint = saveNormal;
			}

			var normal = normalPoint - predictedPosition;
			var sqrRadius = _path.pathRadius * _path.pathRadius;

			if (normal.sqrMagnitude > sqrRadius) return true;

			if ((normal * _normalScaleOnAnticipation).sqrMagnitude <= sqrRadius) return false;
			
			factor = _steeringForceScaleOnAnticipation;
			return true;
		}

		private Vector3 GetNearestPoint(Vector3 origin, Vector3 _PointA, Vector3 _PointB)
		{
			float sqrDistA = (_PointA - origin).sqrMagnitude;
			float sqrDistB = (_PointB - origin).sqrMagnitude;

			return (sqrDistA < sqrDistB) ? _PointA : _PointB;
		}

		private void OnDrawGizmos()
		{
			if (!IsDrawGizmos || _path.pointList.Count == 0) return;

			Gizmos.color = _normalPointColor;
			Gizmos.DrawWireSphere(_normalPoint, 1);

			var predictedPosition = transform.position + BoidController.Velocity;
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(predictedPosition, 1);
			Gizmos.DrawLine(predictedPosition, _normalPoint);
			Gizmos.DrawLine(transform.position, predictedPosition);

			Gizmos.color = _desiredVelocityColor;
			Gizmos.DrawLine(transform.position, transform.position + _desiredVelocity);
			
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(_normalPoint, _minDistanceFromPoint);

			if (!_pathInspector)
			{
				for (int i = 0; i < _path.precalculatedPath.Count - 1; ++i)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(_path.precalculatedPath[i], _path.precalculatedPath[i + 1]);

					Gizmos.color = Color.red;
					Vector3 leftBorderStartPoint = _path.precalculatedPath[i] + Quaternion.AngleAxis(-90, Vector3.up)
					                               * (_path.precalculatedPath[i + 1] - _path.precalculatedPath[i]).normalized
					                               * _path.pathRadius;
					Vector3 rightBorderStartPoint = _path.precalculatedPath[i] + Quaternion.AngleAxis(90, Vector3.up)
					                                * (_path.precalculatedPath[i + 1] - _path.precalculatedPath[i]).normalized
					                                * _path.pathRadius;

					Gizmos.DrawLine(leftBorderStartPoint,
						leftBorderStartPoint + _path.precalculatedPath[i + 1] - _path.precalculatedPath[i]);
					Gizmos.DrawLine(rightBorderStartPoint,
						rightBorderStartPoint + _path.precalculatedPath[i + 1] - _path.precalculatedPath[i]);
				}

				if (_path.pathStyle == Path.PathStyle.Loop)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(_path.precalculatedPath[0], _path.precalculatedPath[_path.precalculatedPath.Count - 1]);

					Gizmos.color = Color.red;
					Vector3 leftBorderStartPoint = _path.precalculatedPath[0] + Quaternion.AngleAxis(-90, Vector3.up)
					                               * (_path.precalculatedPath[_path.precalculatedPath.Count - 1] -
					                                  _path.precalculatedPath[0]).normalized
					                               * _path.pathRadius;
					Vector3 rightBorderStartPoint = _path.precalculatedPath[0] + Quaternion.AngleAxis(90, Vector3.up)
					                                * (_path.precalculatedPath[_path.precalculatedPath.Count - 1] -
					                                   _path.precalculatedPath[0]).normalized
					                                * _path.pathRadius;

					Gizmos.DrawLine(leftBorderStartPoint,
						leftBorderStartPoint + _path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]);
					Gizmos.DrawLine(rightBorderStartPoint,
						rightBorderStartPoint + _path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]);
				}
			}
		}
	}
}
