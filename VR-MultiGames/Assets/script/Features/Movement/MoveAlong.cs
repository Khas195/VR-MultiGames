using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlong : MonoBehaviour {
	[SerializeField]
	List<Transform> points;
	[SerializeField]
	float speed;
	Transform curTarget;
	Queue<Transform> pointsQueue = new Queue<Transform> ();
	bool positiveDirection;
	// Use this for initialization
	void Start () {
		QueuePoints ();
		curTarget = pointsQueue.Dequeue ();
		positiveDirection = true;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate ((curTarget.position - transform.position).normalized * speed*Time.deltaTime);
		if (Vector3.Distance(transform.position, curTarget.position) <= 0.1){
			if (pointsQueue.Count > 0) {
				curTarget = pointsQueue.Dequeue ();
			} else {
				if (positiveDirection) {
					QueuePointsRevers ();
				} else {
					QueuePoints ();
				}
				positiveDirection = !positiveDirection;
			}
		}
	}

	void QueuePoints ()
	{
		for (int i = 0; i < points.Count; ++i) {
			pointsQueue.Enqueue (points [i]);
		}
	}
	void QueuePointsRevers ()
	{
		for (int i = points.Count - 1; i >= 0; --i) {
			pointsQueue.Enqueue (points [i]);
		}
	}
}
