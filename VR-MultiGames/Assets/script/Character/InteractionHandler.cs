using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    readonly List<Interaction> curInteractions = new List<Interaction>();

    private Vector3 lastVelocityOnImpact;
    private Vector3 lastNormalOnImpact;
    // Use this for initialization
    void Start () {
		
	}
    public void OnCollisionEnter(Collision other)
    {
        foreach (var p in other.contacts)
        {
            HandlePaintInteraction(p.point - transform.position);
            lastNormalOnImpact = p.normal;
        }
        lastVelocityOnImpact = other.relativeVelocity;
    }

    public void HandlePaintInteraction(Vector3 direction)
    {
        RaycastHit hit;
        Interaction interaction = null;
        if (Physics.Raycast(transform.position, direction, out hit, GetComponent<Collider>().bounds.extents.y + 0.1f,
            LayerMask.GetMask("Obstacle")))
        {
            var hitPaintable = hit.collider.gameObject.GetComponent<Paintable>();
            if (hitPaintable != null)
            {
                Color color = hitPaintable.GetColorAtTextureCoord(hit.textureCoord2);
                interaction = PaintInteraction.GetInstance().GetInteractionBasedOnColor(color);
            }
        }

        if (interaction != null && !curInteractions.Contains(interaction))
        {
            interaction.Init(gameObject);
            curInteractions.Add(interaction);
        }
    }

    // Update is called once per frame
    void Update () {

        HandlePaintInteraction(-transform.up);
    }

    void FixedUpdate()
    {
        ProcessInteraction();
    }
    void ProcessInteraction()
    {
        var finishedInteraction = new List<Interaction>();
        foreach (var i in curInteractions)
        {
            i.Interact(gameObject);
            if (i.IsDone())
            {
                finishedInteraction.Add(i);
            }
        }
        foreach (var i in finishedInteraction)
        {
            i.RevertInteraction(gameObject);
            curInteractions.Remove(i);
        }
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
