using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTo : MonoBehaviour {

	[SerializeField]
	Transform target;
	[SerializeField]
	[Tooltip("Time takes to reach destination (seconds)")]
	float duration;
	[SerializeField]
	UnityEngine.Events.UnityEvent tweenDone;

	float startTime = 0;
	Vector3 startPos;

	Vector3 change;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		var time = Time.time - startTime;
		if (time >= duration) {
			this.enabled = false;
			transform.position = target.position;
			tweenDone.Invoke ();
			return;
		}
		var newPos = new Vector3 (Ultil.EaseInQuad(time,startPos.x, change.x, duration),
			Ultil.EaseInQuad(time,startPos.y, change.y, duration),
			Ultil.EaseInQuad(time,startPos.z, change.z, duration));
		transform.position = newPos;

	}

	bool HasReachDestination ()
	{
		return Vector3.Distance (target.position, transform.position) < 0.1;
	}


	public void OnEnable(){
		startTime = Time.time;
		startPos = this.transform.position;
		change = target.position - startPos;
	}

	public void OnDisable(){
	}

}
