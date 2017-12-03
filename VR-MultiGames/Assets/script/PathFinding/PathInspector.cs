using UnityEditor;
using UnityEngine;

namespace script.PathFinding
{
	public class PathInspector : MonoBehaviour
	{
		[SerializeField] 
		private Path _path = new Path();

		[SerializeField] 
		private bool _isDrawGizmos = false;

		public bool isDrawGizmos
		{
			get { return _isDrawGizmos; }
			set { _isDrawGizmos = value; }
		}

		public Path path
		{
			get { return _path; }
		}

		private void OnDrawGizmos()
		{
			if(!_isDrawGizmos || _path.precalculatedPath.Count < 2) return;
			
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
				
				Gizmos.DrawLine(leftBorderStartPoint, leftBorderStartPoint + _path.precalculatedPath[i + 1] - _path.precalculatedPath[i]);
				Gizmos.DrawLine(rightBorderStartPoint, rightBorderStartPoint + _path.precalculatedPath[i + 1] - _path.precalculatedPath[i]);
			}

			if (_path.pathStyle == Path.PathStyle.Loop)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(_path.precalculatedPath[0], _path.precalculatedPath[_path.precalculatedPath.Count - 1]);
				
				Gizmos.color = Color.red;
				Vector3 leftBorderStartPoint = _path.precalculatedPath[0] + Quaternion.AngleAxis(-90, Vector3.up) 
				                               * (_path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]).normalized 
				                               * _path.pathRadius;
				Vector3 rightBorderStartPoint = _path.precalculatedPath[0] + Quaternion.AngleAxis(90, Vector3.up) 
				                                * (_path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]).normalized
				                                * _path.pathRadius;
				
				Gizmos.DrawLine(leftBorderStartPoint, leftBorderStartPoint + _path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]);
				Gizmos.DrawLine(rightBorderStartPoint, rightBorderStartPoint + _path.precalculatedPath[_path.precalculatedPath.Count - 1] - _path.precalculatedPath[0]);
			}
		}
	}
}
