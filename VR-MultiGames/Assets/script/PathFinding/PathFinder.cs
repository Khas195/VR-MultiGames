using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace script.PathFinding
{
	public class PathFinder : MonoBehaviour
	{
		[SerializeField] 
		private float _recalculationFrequency;
		
		[Tooltip("Distance max to find closest point on nav mesh is wanted destination is not on nav mesh")]
		[SerializeField]
		private float _destinationSamplePositionDistance = 32;

		private static PathFinder _pathFinder;
		
		private NavMeshPath _navMeshPath;

		public bool SamplePoint(Vector3 destination)
		{
			NavMeshHit navMeshHit;
			return NavMesh.SamplePosition(destination, out navMeshHit, _destinationSamplePositionDistance, NavMesh.AllAreas);
		}
		
		public bool CalculatePath(Vector3 origin, Vector3 destination, out List<PathPoint> pointList)
		{
			pointList = new List<PathPoint>();
			_navMeshPath = new NavMeshPath();

			NavMeshHit navMeshHit;
			if (NavMesh.CalculatePath(origin, destination, NavMesh.AllAreas, _navMeshPath))
			{
				foreach (var corner in _navMeshPath.corners)
				{
					var newPathPoint = new PathPoint();
					newPathPoint.position = corner;
					pointList.Add(newPathPoint);
				}
			}
			return false;
		}
	}
}
