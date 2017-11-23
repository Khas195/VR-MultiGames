using UnityEngine;
using UnityEngine.Timeline;

namespace script.PathFinding
{
	public class Path : MonoBehaviour 
	{
		public enum PathType
		{
			PointList, 
			BezierCurve,
		}

		public enum BezierCurveType
		{
			Quadratic,
			Cubic
		}
		
		public enum PathStyle
		{
			None,
			Loop,
			BackFront			
		}
		
		[SerializeField]
		float bool _

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
	}
}
