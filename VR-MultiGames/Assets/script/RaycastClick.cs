using System.Collections.Generic;
using UnityEngine;

public class RaycastClick : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> inks;
    [SerializeField]
	private List<Color> colors;
    // Use this for initialization


	RaycastHit hit;
    void Start()
    {
		Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
	void Paint (Paintable paintee, Vector2 textCoord)
	{
		Texture2D randomInk = inks [Random.Range (0, inks.Count)];
		Color randomColor = colors [Random.Range (0, colors.Count)];
		paintee.PaintMapping (textCoord, randomInk, randomColor);
	}
}