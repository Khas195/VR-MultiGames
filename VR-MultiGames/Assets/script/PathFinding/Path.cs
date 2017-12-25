using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Schema;
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

		private int GetValidIndex(int index)
		{
			if (index < _precalculatedPath.Count) return index;

			var resultIndex = index;
			
			var offset = resultIndex - _precalculatedPath.Count;

			if (pathStyle == PathStyle.Loop)
			{
				resultIndex = offset;
			}
			else
			{
				resultIndex = _precalculatedPath.Count - 1;
			}

			return resultIndex;
		}

		public Vector3 GetNearestPoint(Vector3 origin, int from, int to , out int index, out Vector3 prevPoint, 
			out Vector3 nextPoint)
		{
			index = 0;
			prevPoint = nextPoint = _precalculatedPath.Count > 0 ? _precalculatedPath[0] : Vector3.zero;
			
			if (_precalculatedPath.Count < 2) return prevPoint;

			var lastIndex = _precalculatedPath.Count - 1;
			var start = from > 0 ? from % _precalculatedPath.Count : 0;
			var end = to > -1 ? to % _precalculatedPath.Count : lastIndex;

			index = start;

			var nearestDistance = (origin - _precalculatedPath[index]).sqrMagnitude;
			var curIndex = index + 1;
			
			while (curIndex != end + 1)
			{
				if (curIndex > lastIndex)
				{
					curIndex = 0;
				}

				var tempDistance = (origin - _precalculatedPath[curIndex]).sqrMagnitude;

				if (nearestDistance > tempDistance)
				{
					index = curIndex;
					nearestDistance = tempDistance;

					prevPoint = _precalculatedPath[curIndex - 1 < 0 ? lastIndex : curIndex - 1];
					nextPoint = _precalculatedPath[curIndex + 1 > lastIndex ? 0 : curIndex + 1];
				}
				
				++curIndex;
			}

			return _precalculatedPath[index];
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

					if (_pathStyle == PathStyle.Loop && _pointList.Count > 1)
					{
						_precalculatedPath.Add(_pointList[0].position);
					}
				}
					break;
				case PathType.BezierCurve:
				{
					if (_pathStyle == PathStyle.Loop)
					{
						_length = BezierCurve.CalculateBezierPath(_pointList, _bezierCurveStepNum, _bezierCurveType, true,
							out _precalculatedPath);
					}
					else
					{
						_length = BezierCurve.CalculateBezierPath(_pointList, _bezierCurveStepNum, _bezierCurveType, false,
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
