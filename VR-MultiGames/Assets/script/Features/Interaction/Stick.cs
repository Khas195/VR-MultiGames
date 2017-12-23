using UnityEngine;

public class Stick : Interaction
{
    public override void Init(GameObject targetObject)
    {
        targetObject.GetComponent<Rigidbody>().useGravity = false;
    }

    public override void RevertInteraction(GameObject targetObject)
    {
        targetObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public override void Interact(GameObject targetObject)
    {
    }
}