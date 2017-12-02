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

		public static float CalculateBezierPath(List<PathPoint> pointList, BezierCurveType type, int stepNum,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (stepNum < 1 || pointList.Count < 2) return -1;
			
			switch (type)
			{
				case BezierCurveType.Custom:
					return CalculateCustomBezierPath(pointList, stepNum, out calculatedPath);
				case BezierCurveType.RelaxedBSpline:
					return CalculateRelaxedBSpline(pointList, stepNum, out calculatedPath);
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		public static float CalculateCustomBezierPath(List<PathPoint> pointList, int stepNum,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (pointList == null || stepNum < 1 || pointList.Count < 2) return -1;

			float length = 0;
			
			calculatedPath.Add(pointList[0].position);
			
			for (int i = 0; i < pointList.Count - 1; ++i)
			{
				Vector3[] tempPath;
				if (pointList[i].handle2 != Vector3.zero)
				{
					if (pointList[i + 1].handle1 != Vector3.zero)
					{
						length += CalculateCubicBezierPoints(stepNum, pointList[i].position, pointList[i].globalHandle2,
							pointList[i + 1].globalHandle1, pointList[i + 1].position, out tempPath);
					}
					else
					{
						length += CalculateQuadraticBezierPoints(stepNum, pointList[i].position, pointList[i].globalHandle2,
							pointList[i + 1].position, out tempPath);
					}
				}
				else
				{
					if (pointList[i + 1].handle1 != Vector3.zero)
					{
						length += CalculateQuadraticBezierPoints(stepNum, pointList[i].position, pointList[i + 1].globalHandle1, 
							pointList[i + 1].position, out tempPath);
					}
					else
					{
						tempPath = new[] {pointList[i].position, pointList[i + 1].position};
						length += (pointList[i].position - pointList[i + 1].position).magnitude;
					}
				}

				for (var j = 1; j < tempPath.Length; ++j)
				{
					calculatedPath.Add(tempPath[j]);
				}
			}
			
			return length;
		}

		public static float CalculateRelaxedBSpline(List<PathPoint> pointList, int stepNum,
			out List<Vector3> calculatedPath)
		{
			calculatedPath = new List<Vector3>();
			if (pointList == null || stepNum < 1 || pointList.Count < 2) return -1;

			var recalutedPointList = RecalculatePathPointRelaxedBSpline(pointList);

			return CalculateCustomBezierPath(recalutedPointList, stepNum, out calculatedPath);
		}

		private static void GetMidPoints(Vector3 p1, Vector3 p2, out Vector3 mp1, out Vector3 mp2)
		{
			mp1 = p1 * 2 / 3 + p2 / 3;
			mp2 = p1 / 3 + p2 * 2 / 3;
		}

		private static List<PathPoint> RecalculatePathPointRelaxedBSpline(List<PathPoint> pointList)
		{
			if (pointList == null || pointList.Count <= 1) return pointList;

			if (pointList.Count == 2)
			{
				return pointList;
			}

			List<PathPoint> resultList = new List<PathPoint>(pointList.Count);

			Vector3 mp1, mp2;

			GetMidPoints(pointList[0].position, pointList[1].position, out mp1, out mp2);

			var tempPoint = new PathPoint();
			tempPoint.position = pointList[0].position;
			tempPoint.globalHandle2 = mp1;
			
			resultList.Add(tempPoint);

			for (int i = 1; i < pointList.Count - 1; ++i)
			{
				var prevMp2 = mp2;
				
				GetMidPoints(pointList[i].position, pointList[i + 1].position, out mp1, out mp2);
				
				tempPoint = new PathPoint();
				tempPoint.position = (prevMp2 + mp1) / 2;
				tempPoint.globalHandle1 = prevMp2;
				tempPoint.globalHandle2 = mp1;
				
				resultList.Add(tempPoint);
			}
				
			tempPoint = new PathPoint();
			tempPoint.position = pointList[pointList.Count - 1].position;;
			tempPoint.globalHandle1 = mp2;
				
			resultList.Add(tempPoint);

			return resultList;
		}
	}
}
