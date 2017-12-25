using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : Interaction {
	public float bounceForce;
	BasicMovement movement;
    private InteractionHandler interactHandler;
    private PlayerInput pi;

    public override void Interact(GameObject targetObject)
    {
        movement.Jump(interactHandler.GetLastImpactNormal(), bounceForce);
        pi.DisableControl();
        SoundsManager.GetInstance().PlayClip(ActionInGame.PlayBounce, targetObject.transform.position);
        Invoke("EnableControl", 1.0f);
    }
    public override void RevertInteraction(GameObject targetObject)
    {
    }

    public void EnableControl()
    {
        pi.EnableControl();
    }
	public override void Init (GameObject targetObject)
	{
	    interactHandler = targetObject.GetComponent<InteractionHandler>();
		movement = targetObject.GetComponent<BasicMovement> ();
	    pi = targetObject.GetComponent<PlayerInput>();
	}
}
