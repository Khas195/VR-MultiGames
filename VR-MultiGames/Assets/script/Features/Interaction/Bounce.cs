using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : Interaction {
	public float bounceForce;
    public float time;

    private float beginTime;
	Rigidbody rg;
	BasicMovement movement;
    private bool bounced= false;
    private PlayerInput controller;
    private Vector3 cacheVelocity;
    private InteractionHandler interactHandler;

    public override void Interact(GameObject targetObject)
	{
	    beginTime += Time.deltaTime;
	    if (bounced) return;
        Debug.DrawRay(targetObject.transform.position, interactHandler.GetLastImpactNormal(), Color.green, 10.0f);
        movement.Jump(interactHandler.GetLastImpactNormal(), bounceForce);
        controller.DisableControl();
        bounced = true;

	}
	public override bool IsDone ()
	{
		return beginTime >= time;
	}
    public override void RevertInteraction(GameObject targetObject)
    {
        bounced = false;
        controller.EnableControl();
        beginTime = 0;
        controller = null;
    }

	public override void Init (GameObject targetObject)
	{
	    interactHandler = targetObject.GetComponent<InteractionHandler>();
        controller = targetObject.GetComponent<PlayerInput>();
        rg = targetObject.GetComponent<Rigidbody> ();
	    cacheVelocity = rg.velocity;
		movement = targetObject.GetComponent<BasicMovement> ();
	}
}
