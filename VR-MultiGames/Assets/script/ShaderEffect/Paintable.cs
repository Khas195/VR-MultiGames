using System;
using Assets.script;
using UnityEngine;
using System.Collections.Generic;
class Paintable : MonoBehaviour
{
	[SerializeField]
	bool InitOnStart;
    private Material mat;
	private Texture2D drawTexture;
	private Texture2D normalMap;
	private Texture2D heightMap;

	[SerializeField]
	int drawTextureSize;

	bool init = false;

	float curFill;

	float totalFill;

	Color targetColor;

	public float GetFillPercentage ()
	{
		return curFill / totalFill;
	}

    void Start()
	{       
		if (this.GetComponent<Glowable> ()) {
			targetColor = this.GetComponent<Glowable> ().GlowColor;
			targetColor.a = 1.0f;
		}
//		Vector2[,] fractals = new Vector2[100,100];
//		TestFractals (fractals, 0, 100, 0, 100,5);
		if (InitOnStart) {
			Init ();
		}
    }

	public bool HasInit ()
	{
		return init;;
	}

    public void Init()
    {
		// 640 sheet on a 10 scaled object
		// ??? sheet on a 20 scaled Object
		mat = Ultil.GetMaterialWithShader(GetComponent<Renderer>().materials, PaintableDefinition.PaintableShaderName, name);

		drawTexture = new Texture2D (drawTextureSize, drawTextureSize);
		normalMap = new Texture2D (drawTextureSize, drawTextureSize);
		heightMap = new Texture2D (drawTextureSize, drawTextureSize);

		ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));
		ResetTextureToColor(normalMap, new Color(0.5f, 0.5f, 1, 0));
		ResetTextureToColor(heightMap,new Color(0, 0, 0, 0));

		mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
		mat.SetTexture(PaintableDefinition.DrawOnNormalMap, normalMap);
		mat.SetTexture(PaintableDefinition.DrawOnHeigtMap, heightMap);
		totalFill = drawTexture.width * drawTexture.height;
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
    private bool IsPositionValid(int x, int y, int colorIndex, int length, int width, int height)
    {
        return (x >= 0 && x < width) && (y >= 0 && y < height)
               && colorIndex < length;
    }
		

	public void FullFill(){

		Color[] colors = drawTexture.GetPixels ();
		for (int i = 0; i < colors.GetLength (0); ++i) {
			colors [i] = targetColor;
		}
		drawTexture.SetPixels (colors);
		drawTexture.Apply ();
	}

	float ColorDifference (Color colorToCheck, Color targetColor)
	{
		return Mathf.Sqrt (Mathf.Pow (colorToCheck.r - targetColor.r, 2) + Mathf.Pow (colorToCheck.b - targetColor.b, 2) + Mathf.Pow (colorToCheck.g - targetColor.g, 2));
	}

    public bool PaintMapping(Vector2 textureCoord, Texture2D ink, Color color)
    {
		if (!init || !this.enabled) return false;
		int xOrigin = (int)(textureCoord.x * (drawTexture.width)) - (ink.width/2);
		int yOrigin = (int)(textureCoord.y * (drawTexture.height)) - (ink.height/2);

		int right = xOrigin + ink.width;
		int bottom = yOrigin + ink.height;

		var inkLeft = 0;
		var inkBottom = 0;

		if (xOrigin < 0) {
			inkLeft = xOrigin * -1;
			xOrigin = 0;
		}
		if (yOrigin < 0) {
			inkBottom = yOrigin * -1;
			yOrigin = 0;
		}

		right = right >= drawTexture.width ? drawTexture.width - 1 : right;
		bottom = bottom >= drawTexture.height ? drawTexture.height - 1 : bottom;
		int blockWidth = right - xOrigin;
		int blockHeight = bottom - yOrigin;

		Color[] colors = drawTexture.GetPixels(xOrigin, yOrigin, blockWidth, blockHeight);
		Color[] normals = normalMap.GetPixels(xOrigin, yOrigin, blockWidth, blockHeight);
		Color[] heights = heightMap.GetPixels(xOrigin, yOrigin, blockWidth, blockHeight);

		Color[] inkColors = ink.GetPixels (inkLeft, inkBottom, blockWidth , blockHeight);

		for (int x = 0; x < blockWidth; x++)
	    {
			for (int y = 0; y < blockHeight; y++)
	        {
				var colorIndex = x  + y * blockWidth;

				if (inkColors [colorIndex].a == 0) {
					continue;
				}
				if (colors [colorIndex].a <= 0 && inkColors[colorIndex].a > 0) {
					if (ColorDifference (color, targetColor) < 0.5f) {
						curFill++;
					} else {
						curFill--;
					}
				}
				colors[colorIndex] = Color.Lerp(colors[colorIndex], color, inkColors[colorIndex].a);



				colors [colorIndex].a = colors [colorIndex].a + inkColors [colorIndex].a;
				if (inkColors [colorIndex].a > 0) {
					var average = (inkColors [colorIndex].r + inkColors [colorIndex].b + inkColors [colorIndex].g)/3.0f;
					normals [colorIndex] = new Color (average,  average, average, 1);
				}
	        }
		}
		heightMap.SetPixels(xOrigin, yOrigin, blockWidth, blockHeight, heights);
		heightMap.Apply ();
		normalMap.SetPixels(xOrigin, yOrigin, blockWidth, blockHeight, normals);
		normalMap.Apply ();
		drawTexture.SetPixels(xOrigin, yOrigin, blockWidth, blockHeight, colors);
        drawTexture.Apply();
		return true;
    }
}
