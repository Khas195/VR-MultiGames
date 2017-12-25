using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Timeline;

namespace script.PathFinding
{
	[Serializable]
	public class Path
	{
		public enum PathType
		{
			PointList, 
			BezierCurve
		}
		
		public enum PathStyle
		{
			None,
			Loop	
		}
		
		#region Inspector
		#region Setting
		
		[SerializeField]
		private PathType _pathType = PathType.PointList;
		[SerializeField]
		private PathStyle _pathStyle = PathStyle.None;
		[SerializeField]
		private BezierCurve.BezierCurveType _bezierCurveType = BezierCurve.BezierCurveType.Custom;
		[SerializeField]
		private float _pathRadius = 3;
		[SerializeField]
		private float _length;
		[SerializeField]
		private List<PathPoint> _pointList = new List<PathPoint>();
		
		#endregion
		
		#region BezierCurve
		
		[Tooltip("Number of point between 2 points")]
		[SerializeField]
		[Range(1, 1000)]
		private int _bezierCurveStepNum = 100;
		
		#endregion
		
		#endregion
		
		[SerializeField]
		private List<Vector3> _precalculatedPath;
		private int _curIndex;
		private bool _increaseStep = true;

		#region GetSet
		
		public PathType pathType
		{
			get { return _pathType; }
			set
			{
				_pathType = value;
				CalculatePath();
			}
		}

		public PathStyle pathStyle
		{
			get { return _pathStyle; }
			set
			{
				_pathStyle = value;
				CalculatePath();
			}
		}

		public BezierCurve.BezierCurveType bezierCurveType
		{
			get { return _bezierCurveType; }
			set
			{
				_bezierCurveType = value;
				CalculatePath();
			}
		}

		public float pathRadius
		{
			get { return _pathRadius; }
			set { _pathRadius = value; }
		}

		public Vector3 curPoint
		{
			get { return _precalculatedPath[_curIndex]; }
		}

		public int curIndex
		{
			get { return _curIndex; }
			set
			{
				if (value < 0 || value >= _precalculatedPath.Count)
					return;
				_curIndex = value;
			}
		}

		public int bezierCurveStepNum
		{
			get { return _bezierCurveStepNum; }
			set
			{
				if (value < 1 || value > 1000) return;
				_bezierCurveStepNum = value;
				CalculatePath();
			}
		}

		public float length
		{
			get { return _length; }
		}

		public List<Vector3> precalculatedPath
		{
			get { return _precalculatedPath; }
		}

		public List<PathPoint> pointList
		{
			get { return _pointList; }
			set
			{
				if(value == null) return;
				
				_pointList = value;
				CalculatePath();
			}
		}
		
		#endregion

		public Vector3 GetNearestPoint(Vector3 origin, out Vector3 prevPoint, out Vector3 nextPoint)
		{
			prevPoint = nextPoint = Vector3.zero;
			
			if (_precalculatedPath.Count < 2) return Vector3.zero;

			var nearestPoint = _precalculatedPath[0];
			nextPoint = _precalculatedPath[1];

			var nearestSqrDist = (nearestPoint - origin).sqrMagnitude;

			for (var i = 1; i < _precalculatedPath.Count; ++i)
			{
				var sqrDist = (_precalculatedPath[i] - origin).sqrMagnitude;

				if (nearestSqrDist <= sqrDist) continue;
				
				nearestPoint = _precalculatedPath[i];
				nearestSqrDist = (nearestPoint - origin).sqrMagnitude;

				if (i < _precalculatedPath.Count - 1)
				{
					nextPoint = _precalculatedPath[i + 1];
				}
				else
				{
					nextPoint = Vector3.zero;
				}

				if (i > 0)
				{
					prevPoint = _precalculatedPath[i - 1];
				}
			}

			return nearestPoint;
		}

		public static Vector3 GetNormalPoint(Vector3 origin, Vector3 start, Vector3 end)
		{
			var startToPosition = origin - start;
			var pathLine = end - start;
			var projection = Vector3.Project(startToPosition, pathLine);
			var normalPoint = start + projection;

			if (Vector3.Dot(projection.normalized, pathLine.normalized) < 0)
			{
				normalPoint = start;
			}
			else if (projection.sqrMagnitude > pathLine.sqrMagnitude)
			{
				normalPoint = end;
			}

			return normalPoint;
		}

		public bool Contains(PathPoint point)
		{
			return _pointList.Contains(point);
		}

		public bool Remove(PathPoint point)
		{
			bool isRemoved = _pointList.Remove(point);
			CalculatePath();
			return isRemoved;
		}

		public void Add(PathPoint point)
		{
			_pointList.Add(point);
			CalculatePath();
		}

		public void CalculatePath()
		{

			if (_precalculatedPath == null)
			{
				_precalculatedPath = new List<Vector3>();
			}
			_precalculatedPath.Clear();
			
			switch (pathType)
			{
				case PathType.PointList:
				{
					foreach (var point in _pointList)
					{
						_precalculatedPath.Add(point.position);
					}
				}
					break;
				case PathType.BezierCurve:
				{
					if (_pathStyle == PathStyle.Loop)
					{
						var tempList = new List<PathPoint>(_pointList);
						tempList.Add(_pointList[0]);
						
						_length = BezierCurve.CalculateBezierPath(tempList, _bezierCurveType, _bezierCurveStepNum,
							out _precalculatedPath);
						
						_precalculatedPath.RemoveAt(_precalculatedPath.Count - 1);
					}
					else
					{
						_length = BezierCurve.CalculateBezierPath(_pointList, _bezierCurveType, _bezierCurveStepNum,
							out _precalculatedPath);
					}
				}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
