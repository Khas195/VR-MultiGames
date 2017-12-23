using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
	public virtual void Init (GameObject targetObject)
	{
		throw new System.NotImplementedException ();
	}

	public virtual bool IsDone ()
	{
		throw new System.NotImplementedException ();
	}

	public virtual void RevertInteraction (GameObject targetObject)
	{
		throw new System.NotImplementedException ();
	}

	public virtual void Interact(GameObject targetObject){
		throw new System.NotImplementedException ();
	}


}