using System.Collections;
using System.Collections.Generic;
using script.BoidBehavior;
using script.PathFinding;
using UnityEngine;

public class BirdSetting : MonoBehaviour
{
	[SerializeField]
	private List<BoidController> _birdFlock;

	[SerializeField]
	private List<PathInspector> _pathList;

	private int _curPathIndex = 0;

	private void Start()
	{
		
		foreach (var bird in _birdFlock)
		{
			var pathFollow = bird.GetComponent<PathFollowBehavior>();
			
			if(!pathFollow) continue;

			pathFollow.path = _pathList[_curPathIndex].path;
		}
	}

	public void UpdateCurrentPath(int pathIndex)
	{
		Debug.Log("Update path " + pathIndex);
		if (_pathList.Count == 0) return;
		if(pathIndex < 0 || pathIndex >= _pathList.Count) return;
		if(_curPathIndex == pathIndex) return;

		_curPathIndex = pathIndex;
		foreach (var bird in _birdFlock)
		{
			var pathFollow = bird.GetComponent<PathFollowBehavior>();
			
			if(!pathFollow) continue;

			pathFollow.path = _pathList[_curPathIndex].path;
		}
	}
}
