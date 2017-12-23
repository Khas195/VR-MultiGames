using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    readonly List<Interaction> curInteractions = new List<Interaction>();
    private Interaction curInteract;
    private Vector3 lastVelocityOnImpact;
    private Vector3 lastNormalOnImpact;

    private BasicMovement movement;

    // Use this for initialization
    void Start ()
    {
        movement = this.GetComponent<BasicMovement>();
    }
    public void OnCollisionStay(Collision other)
    {
        lastVelocityOnImpact = other.relativeVelocity;
        foreach (var p in other.contacts)
        {
            HandlePaintInteraction(p.point - transform.position);
        }
    }
    void OnCollisionExit()
    {
        if (curInteract == null) return;
        curInteract.RevertInteraction(this.gameObject);
        curInteract = null;
    }
    public void HandlePaintInteraction(Vector3 direction)
    {
        RaycastHit hit;
        Interaction interaction = null;
        if (Physics.Raycast(transform.position, direction, out hit, GetComponent<Collider>().bounds.extents.y + 0.1f,
            LayerMask.GetMask("Obstacle")))
        {
            lastNormalOnImpact = hit.normal;
            var hitPaintable = hit.collider.gameObject.GetComponent<Paintable>();
            
            if (hitPaintable != null)
            {
                Color color = hitPaintable.GetColorAtTextureCoord(hit.textureCoord2);
                interaction = PaintInteraction.GetInstance().GetInteractionBasedOnColor(color);
            }
        }

        if (interaction == null )
        {
            if (curInteract == null) return;
            curInteract.RevertInteraction(this.gameObject);
            curInteract = null;
        }
        else
        {
            var difference = curInteract != interaction;
            if (curInteract != null  && difference)
                curInteract.RevertInteraction(gameObject);

            curInteract = interaction;
            if (!difference) return;
            curInteract.Init(gameObject);
            curInteract.Interact(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
    }
    


    public Vector3 GetLastImpactVelocity()
    {
        return lastVelocityOnImpact;
    }

    public Vector3 GetLastImpactNormal()
    {
        return lastNormalOnImpact;
    }
}
