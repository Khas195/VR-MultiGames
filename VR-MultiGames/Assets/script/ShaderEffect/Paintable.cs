using System;
using Assets.script;
using UnityEngine;
class Paintable : MonoBehaviour
{
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

		drawTexture = new Texture2D(640, 640);
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
        Debug.LogError("Cannot find Material with Shader " + PaintableDefinition.PaintableShaderName + " for " + name);
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

        int xOrigin = (int)(textureCoord.x * (drawTexture.width)) - ink.width/2;
        int yOrigin = (int)(textureCoord.y * (drawTexture.height)) - ink.height/2;
        Color[] colors = drawTexture.GetPixels();
        Color[] inkColor = ink.GetPixels();
        for (int x = 0; x < ink.width; x++)
        {
            for (int y = 0; y < ink.height; y++)
            {
                var drawIndex = (x + xOrigin) + (y + yOrigin) * drawTexture.height;
                var inkIndex = x + y * ink.height;
                if (!IsPositionValid(x + xOrigin, y + yOrigin, drawIndex, colors.Length, drawTexture.width, drawTexture.height) ) continue;

                if (inkColor[inkIndex].a > 0)
                {
                    colors[drawIndex] = (randomColor * inkColor[inkIndex]);
                    colors[drawIndex].a = 1;
                }
            }
        }
        drawTexture.SetPixels(colors);
        drawTexture.Apply();
    }
}
