using script.PathFinding;
using UnityEngine;

namespace script.BoidBehavior
{
	public class PathFollowBehavior : BoidBehavior
	{
		[SerializeField]
		private PathInspector _pathInspector;
		
		
		private Path _path;
		
		public override void PerformBehavior()
		{
			throw new System.NotImplementedException();
		}

		private void OnDrawGizmos()
		{
			if (!IsDrawGizmos && _path == null) return;
		}
	}
}
