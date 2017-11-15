using System;
using Assets.script;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


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

	public bool HasInit ()
	{
		return init;;
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
		if (this.GetComponent<Glowable> ()) {
			targetColor = this.GetComponent<Glowable> ().GlowColor;
			targetColor.a = 1.0f;
		}
		int xOrigin;
		int yOrigin;
		int inkLeft;
		int inkBottom;
		int blockWidth;
		int blockHeight;

		CalculatePaintBlock (textureCoord, ink,out xOrigin, out yOrigin, out inkLeft, out inkBottom, out blockWidth, out blockHeight);

		Color[] dstColors = drawTexture.GetPixels (xOrigin, yOrigin, blockWidth, blockHeight);
		Color[] normals = normalMap.GetPixels (xOrigin, yOrigin, blockWidth, blockHeight);
		Color[] srcColors = ink.GetPixels (inkLeft, inkBottom, blockWidth, blockHeight);

		for (int x = 0; x < blockWidth; x++) {
			for (int y = 0; y < blockHeight; y++) {
				var colorIndex = x + y * blockWidth;
				if (srcColors [colorIndex].a <= 0) {
					continue;
				}

				AdjustCurrentFill (dstColors[colorIndex], targetColor, color);

				dstColors [colorIndex] = Color.Lerp (dstColors [colorIndex], color, srcColors [colorIndex].a);
				dstColors [colorIndex].a = dstColors [colorIndex].a + srcColors [colorIndex].a;
				if (srcColors [colorIndex].a > 0) {
					var average = (srcColors [colorIndex].r + srcColors [colorIndex].b + srcColors [colorIndex].g) / 3.0f;
					normals [colorIndex] = new Color (average, average, 1, 1);
				}
			}
		}

		normalMap.SetPixels (xOrigin, yOrigin, blockWidth, blockHeight, normals);
		normalMap.Apply ();
		drawTexture.SetPixels (xOrigin, yOrigin, blockWidth, blockHeight, dstColors);
		drawTexture.Apply ();

		return true;
    }

	void CalculatePaintBlock (Vector2 textureCoord, Texture2D ink,out int xOrigin, out int yOrigin, out int inkLeft, out int inkBottom, out int blockWidth, out int blockHeight)
	{
		xOrigin = (int)(textureCoord.x * (drawTexture.width)) - (ink.width / 2);
		yOrigin = (int)(textureCoord.y * (drawTexture.height)) - (ink.height / 2);
		int right = xOrigin + ink.width;
		int bottom = yOrigin + ink.height;
		inkLeft = 0;
		inkBottom = 0;
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
		blockWidth = right - xOrigin;
		blockHeight = bottom - yOrigin;
	}

	void AdjustCurrentFill (Color src, Color targetColor, Color paintColor)
	{
		if (src.a <= 0 && ColorDifference (paintColor, targetColor) < 0.5f) {
			curFill++;
		}
		else
			if (src.a > 0) {
				if (IsColorCloseToSame (src, targetColor) && IsColorCloseToSame (paintColor, targetColor) == false) {
					curFill--;
				}
				else
					if (IsColorCloseToSame (src, targetColor) == false && IsColorCloseToSame (paintColor, targetColor) == true) {
						curFill++;
					}
			}
		Mathf.Clamp (curFill, 0, totalFill);
	}

	bool IsColorCloseToSame (Color colorA, Color colorB)
	{
		return ColorDifference (colorA, colorB) < 0.5f;
	}
}
