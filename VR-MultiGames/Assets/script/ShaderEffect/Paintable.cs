using System;
using Assets.script;
using UnityEngine;

class Paintable : MonoBehaviour
{
	[SerializeField]
	int paintSizeMultiplier = 1;
    private Material mat;
    private Texture2D drawTexture;

	bool init = false;

    void Start()
	{       
    }

    public void Init()
    {
		mat = GetPaintableMaterial(GetComponent<Renderer>().materials);

		var mainTex = mat.GetTexture(PaintableDefinition.MainTexture);
		var scaleX = transform.localScale.x;
		var scaleY = transform.localScale.y;
		var ratio = scaleX / scaleY;

		drawTexture = new Texture2D(Mathf.RoundToInt(GameSettings.GetInstance().InkSplatSize * ratio * 1/paintSizeMultiplier), GameSettings.GetInstance().InkSplatSize * 1/paintSizeMultiplier);
		ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));

		mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
		this.init = true;
    }

    private void ResetTextureToColor(Texture2D texture, Color color)
    {
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(colors);
        texture.Apply();
    }
    private Material GetPaintableMaterial(Material[] materials)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].shader.name.Equals(PaintableDefinition.PaintableShaderName))
            {
                return materials[i];
            }
        }
        UnityEngine.Debug.LogError("Cannot find Material with Shader " + PaintableDefinition.PaintableShaderName + " for " + name);
        return GameDefinition.DefaultMaterial;
    }

    public void Update()
    {
    }
    private bool IsPositionValid(int x, int y, int colorIndex, int length, int width, int height)
    {
        return (x >= 0 && x < width) && (y >= 0 && y < height)
               && colorIndex < length;
    }
    public void PaintMapping(Vector2 textureCoord, Texture2D ink, Color randomColor)
    {
		if (!init) return;
		int xOrigin = (int)(textureCoord.x * (drawTexture.width)) - (ink.width/2);
		int yOrigin = (int)(textureCoord.y * (drawTexture.height)) - (ink.height/2);
	
		Color[] inkColor = ink.GetPixels();

		xOrigin = xOrigin >= 0 ? xOrigin : 0;
		yOrigin = yOrigin >= 0 ? yOrigin : 0;

		int right = xOrigin + ink.width;
		int bottom = yOrigin + ink.height;

		right = right >= drawTexture.width ? drawTexture.width - 1 : right;
		bottom = bottom >= drawTexture.height ? drawTexture.height - 1 : bottom;

		int blockWidth = right - xOrigin;
		int blockHeight = bottom - yOrigin;

		Color[] colors = drawTexture.GetPixels(xOrigin, yOrigin, blockWidth, blockHeight);

		for (int x = 0; x < blockWidth; x++)
	    {
			for (int y = 0; y < blockHeight; y++)
	        {
				var inkIndex = x  + y * blockWidth;
				colors[inkIndex] = Color.Lerp(colors[inkIndex], randomColor, inkColor[inkIndex].a);
				colors[inkIndex].a = inkColor[inkIndex].a + colors[inkIndex].a;
	        }
	    }

		drawTexture.SetPixels(xOrigin, yOrigin, blockWidth, blockHeight, colors);
        drawTexture.Apply();
    }
}
