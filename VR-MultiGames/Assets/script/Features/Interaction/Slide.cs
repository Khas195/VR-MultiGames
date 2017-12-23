using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : Interaction {
	public float speed;
	float originalSpeed = 0;
	BasicMovement cacheMovement;
    private bool revertTimed;

    public override void Init (GameObject targetObject)
    {
        if (revertTimed) return;
		cacheMovement = targetObject.GetComponent<BasicMovement> ();
		originalSpeed = cacheMovement.data.moveSpeed;
	}
	public override void Interact(GameObject targetObject){
		if (cacheMovement != null && !revertTimed) {
			cacheMovement.data.moveSpeed = speed;
		} 
	}
    public override void RevertInteraction (GameObject targetObject)
    {
        revertTimed = true;
        Invoke("RevertMoveSpeed", 1.0f);
    }

    private void RevertMoveSpeed()
    {
        if (cacheMovement != null)
        {
            cacheMovement.data.moveSpeed = originalSpeed;
        }
        originalSpeed = 0;
        cacheMovement = null;
        revertTimed = false;
    }
}