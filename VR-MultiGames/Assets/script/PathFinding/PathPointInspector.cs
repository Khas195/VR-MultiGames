using UnityEngine;

namespace script.PathFinding
{
	[ExecuteInEditMode]
	public class PathPointInspector : MonoBehaviour
	{
		[SerializeField]
		private PathPoint _point = new PathPoint();

		public PathPoint point
		{
			get { return _point; }
		}

		private void Awake()
		{
			_point.transform = transform;
		}
	}
}
