using UnityEditor;
using UnityEngine;

namespace script.PathFinding
{
	public class PathInspector : MonoBehaviour
	{
		[SerializeField] 
		private Path _path = new Path();

		public Path path
		{
			get { return _path; }
		}

		private void OnDrawGizmos()
		{
			if(!_path.isDrawGizmos || _path.precalculatedPath.Count < 2) return;
			
			Gizmos.color = Color.green;
			for (int i = 0; i < _path.precalculatedPath.Count - 1; ++i)
			{
				Gizmos.DrawLine(_path.precalculatedPath[i], _path.precalculatedPath[i + 1]);
			}

			if (_path.pathStyle == Path.PathStyle.Loop)
			{
				Gizmos.DrawLine(_path.precalculatedPath[0], _path.precalculatedPath[_path.precalculatedPath.Count - 1]);
			}
		}
	}
}
