using System.Collections.Generic;
using UnityEngine;
public class RaycastClick : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> inks;
    [SerializeField]
    private List<Color> colors;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        RaycastHit hit;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
        var paintableObject = hit.transform.GetComponent<Paintable>();
        if (paintableObject)
        {
            Texture2D randomInk = inks[Random.Range(0, inks.Count)];
            Color randomColor = colors[Random.Range(0, colors.Count)];
            paintableObject.PaintMapping(hit.textureCoord2, randomInk, randomColor);
        }
    }
}