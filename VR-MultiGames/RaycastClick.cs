using UnityEngine;

public class RaycastClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Input.GetMouseButton(0)) return;

	    RaycastHit hit;

	    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;

	    var paintableObject = hit.transform.GetComponent<Paintable>();
	    if (paintableObject)
	    {
	        paintableObject.Paint(hit.textureCoord2);
	    }
	}
}
