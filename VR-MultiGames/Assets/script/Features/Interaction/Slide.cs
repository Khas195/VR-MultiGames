using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : Interaction {
	public float time;
	public float speed;
	float beginTime;
	float originalSpeed = 0;
	BasicMovement cacheMovement;
	public override void Init (GameObject targetObject)
	{
		cacheMovement = targetObject.GetComponent<BasicMovement> ();
		originalSpeed = cacheMovement.data.moveSpeed;
	}
	public override void Interact(GameObject targetObject){
		beginTime += Time.deltaTime;
		if (cacheMovement != null) {
			cacheMovement.data.moveSpeed = speed;
		} else {
			beginTime = time;
		}
	}
    public override void RevertInteraction (GameObject targetObject)
	{		
		if (cacheMovement != null ) {
			cacheMovement.data.moveSpeed = originalSpeed;
		}
		beginTime = 0;
		originalSpeed = 0;
		cacheMovement = null;
	}
	public override bool IsDone ()
	{
		return beginTime >= time;
	}
}