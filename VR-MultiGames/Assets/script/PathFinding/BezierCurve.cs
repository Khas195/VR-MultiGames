using System;
using System.Collections.Generic;
using UnityEngine;

namespace script.PathFinding
{
	public class BezierCurve 
	{
		public enum BezierCurveType
		{
			RelaxedBSpline,
			Custom
		}

		public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			t = Mathf.Clamp01(t);

			Vector3 part1 = Mathf.Pow(1 - t, 2) * p1;
			Vector3 part2 = 2 * (1 - t) * t * p2;
			Vector3 part3 = Mathf.Pow(t, 2) * p3;

			return part1 + part2 + part3;
		}

		public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector4 p4)
		{
			t = Mathf.Clamp01(t);

			Vector3 part1 = Mathf.Pow(1 - t, 3) * p1;
			Vector3 part2 = 3 * Mathf.Pow(1 - t, 2) * t * p2;
			Vector3 part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * p3;
			Vector3 part4 = Mathf.Pow(t, 3) * p4;

			return part1 + part2 + part3 + part4;
		}
		
		public static float CalculateQuadraticBezierPoints(int stepNum, Vector3 p1, Vector3 p2, Vector3 p3, 
			out Vector3[] calculatedPath)
		{
			calculatedPath = new Vector3[stepNum];
			if (stepNum <= 0)
			{
				return 0;
			}
			
			float length = 0;

			for (int i = 0; i < stepNum; ++i)
			{
				calculatedPath[i] = CalculateQuadraticBezierPoint((float) i / (stepNum - 1), p1, p2, p3);

				if (i > 0)
				{
					length += (calculatedPath[i] - calculatedPath[i - 1]).magnitude;
				}
			}

			return length;
		}

		public static float CalculateCubicBezierPoints(int stepNum, Vector3 p1, Vector3 p2, Vector3 p3, Vector4 p4,
			out Vector3[] calculatedPath)
		{
			calculatedPath = new Vector3[stepNum];
			if (stepNum <= 0)
			{
				return 0;
			}

			float length = 0;

			for (int i = 0; i < stepNum; ++i)
			{
				calculatedPath[i] = CalculateCubicBezierPoint((float) i / (stepNum - 1), p1, p2, p3, p4);

				if (i > 0)
				{
					length += (calculatedPath[i] - calculatedPath[i - 1]).magnitude;
				}
			}

			return length;
		}

		public static float CalculateBezierPath(List<PathPoint> pointList, int stepNum, BezierCurveType type, bool isLoop,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (stepNum < 1 || pointList.Count < 2) return -1;
			
			switch (type)
			{
				case BezierCurveType.Custom:
					return CalculateCustomBezierPath(pointList, stepNum, isLoop, out calculatedPath);
				case BezierCurveType.RelaxedBSpline:
					return CalculateRelaxedBSpline(pointList, stepNum, isLoop, out calculatedPath);
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		private static float CalculateBezierCurveBetween(PathPoint A, PathPoint B, int stepNum, out Vector3[] calculatedPath)
		{
			float length = 0;
			
			if (A.handle2 != Vector3.zero)
			{
				if (B.handle1 != Vector3.zero)
				{
					length += CalculateCubicBezierPoints(stepNum, A.position, A.globalHandle2,
						B.globalHandle1, B.position, out calculatedPath);
				}
				else
				{
					length += CalculateQuadraticBezierPoints(stepNum, A.position, A.globalHandle2,
						B.position, out calculatedPath);
				}
			}
			else
			{
				if (B.handle1 != Vector3.zero)
				{
					length += CalculateQuadraticBezierPoints(stepNum, A.position, B.globalHandle1, 
						B.position, out calculatedPath);
				}
				else
				{
					calculatedPath = new[] {A.position, B.position};
					length += (A.position - B.position).magnitude;
				}
			}

			return length;
		}

		public static float CalculateCustomBezierPath(List<PathPoint> pointList, int stepNum, bool isLoop,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (pointList == null || stepNum < 1 || pointList.Count < 2) return -1;

			float length = 0;
			var lastPosition = pointList.Count - 1;
			
			calculatedPath.Add(pointList[0].position);
			
			for (var i = 0; i < lastPosition; ++i)
			{
				Vector3[] tempPath;
				
				length += CalculateBezierCurveBetween(pointList[i], pointList[i + 1], stepNum, out tempPath);

				for (var j = 1; j < tempPath.Length; ++j)
				{
					calculatedPath.Add(tempPath[j]);
				}
			}

			if (isLoop)
			{
				Vector3[] tempPath;
				
				length += CalculateBezierCurveBetween(pointList[lastPosition], pointList[0], stepNum, out tempPath);

				for (var j = 1; j < tempPath.Length - 1; ++j)
				{
					calculatedPath.Add(tempPath[j]);
				}
			}
			
			return length;
		}

		public static float CalculateRelaxedBSpline(List<PathPoint> pointList, int stepNum, bool isLoop,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (pointList == null || stepNum < 1 || pointList.Count < 2) return -1;

			var recalutedPointList = RecalculatePathPointRelaxedBSpline(pointList, isLoop);

			return CalculateCustomBezierPath(recalutedPointList, stepNum, isLoop, out calculatedPath);
		}

		private static void GetMidPoints(Vector3 p1, Vector3 p2, out Vector3 mp1, out Vector3 mp2)
		{
			mp1 = p1 * 2 / 3 + p2 / 3;
			mp2 = p1 / 3 + p2 * 2 / 3;
		}

		private static List<PathPoint> RecalculatePathPointRelaxedBSpline(List<PathPoint> pointList, bool isLoop)
		{
			if (pointList == null || pointList.Count <= 1) return pointList;

			if (pointList.Count == 2)
			{
				return pointList;
			}

			var resultList = new List<PathPoint>(pointList.Count);
			var lastPosition = pointList.Count - 1;

			Vector3 mp1, mp2, prevMp2;


			var tempPoint = new PathPoint();
			if (isLoop)
			{
				GetMidPoints(pointList[lastPosition].position, pointList[0].position, out mp1, out mp2);
				prevMp2 = mp2;
				GetMidPoints(pointList[0].position, pointList[1].position, out mp1, out mp2);
				tempPoint.position = (prevMp2 + mp1) / 2;
				tempPoint.globalHandle1 = prevMp2;
				tempPoint.globalHandle2 = mp1;
			}
			else
			{
				GetMidPoints(pointList[0].position, pointList[1].position, out mp1, out mp2);
				tempPoint.position = pointList[0].position;
				tempPoint.globalHandle2 = mp1;
			}
			
			resultList.Add(tempPoint);

			for (var i = 1; i < lastPosition; ++i)
			{
				prevMp2 = mp2;
				
				GetMidPoints(pointList[i].position, pointList[i + 1].position, out mp1, out mp2);
				
				tempPoint = new PathPoint();
				tempPoint.position = (prevMp2 + mp1) / 2;
				tempPoint.globalHandle1 = prevMp2;
				tempPoint.globalHandle2 = mp1;
				
				resultList.Add(tempPoint);
			}
				
			tempPoint = new PathPoint();
			prevMp2 = mp2;
			if (isLoop)
			{
				GetMidPoints(pointList[lastPosition].position, pointList[0].position, out mp1, out mp2);
				tempPoint.position = (prevMp2 + mp1) / 2;
				tempPoint.globalHandle1 = prevMp2;
				tempPoint.globalHandle2 = mp1;
			}
			else
			{
				tempPoint.position = pointList[lastPosition].position;;
				tempPoint.globalHandle1 = prevMp2;
			}
				
			resultList.Add(tempPoint);

			return resultList;
		}
	}
}
