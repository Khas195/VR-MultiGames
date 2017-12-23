using System;
using Assets.script;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Paintable : MonoBehaviour
{
	[SerializeField]
	bool InitOnStart;
    private Material mat;
	private Texture2D drawTexture;

	[SerializeField]
	int drawTextureSize;

	bool init = false;

	[SerializeField]
	bool shouldCalFill;
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

	    var initScale = GameSettings.GetInstance().standardScale / this.transform.localScale.x;
	    int initSize = (int) (GameSettings.GetInstance().standardPaintSize / initScale);

        drawTexture = new Texture2D(initSize, initSize);

		ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));
		mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
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
    
	// the input texutrecoord must be from hit.textureCoord2
	public Color GetColorAtTextureCoord(Vector2 textureCoord2){
		if (!init || !this.enabled) return Color.black;
		var x = (int)(textureCoord2.x * (drawTexture.width));
		var y = (int)(textureCoord2.y * (drawTexture.height));
		return drawTexture.GetPixel (x, y);
	}
	public bool PaintMapping(Vector2 textureCoord2, Texture2D ink, Color color)
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

		CalculatePaintBlock (textureCoord2, ink,out xOrigin, out yOrigin, out inkLeft, out inkBottom, out blockWidth, out blockHeight);

		Color[] dstColors = drawTexture.GetPixels (xOrigin, yOrigin, blockWidth, blockHeight);
		Color[] srcColors = ink.GetPixels (inkLeft, inkBottom, blockWidth, blockHeight);

		StartCoroutine ( TweenPaint( xOrigin, yOrigin, blockWidth,  blockHeight,  dstColors, srcColors, color));
		return true;
    }


	IEnumerator  TweenPaint( int xOrigin, int yOrigin, int blockWidth, int  blockHeight,Color[]  dstColors, Color[]   srcColors, Color color){
		
		int centerX = blockWidth / 2;
		int centerY = blockHeight / 2;
		int blockToColor = centerX / 2;
		ISmoothPaint smoother = new MeanFilter ();
		while (blockToColor < blockWidth) {
			int left = Mathf.Clamp(centerX - blockToColor, 0, centerX);
			int right = Mathf.Clamp(centerX + blockToColor, centerX, blockWidth);
			int bottom =  Mathf.Clamp(centerY - blockToColor, 0, centerY);
			int top =  Mathf.Clamp(centerY + blockToColor, centerY, blockHeight);

			MapTexture (color, blockWidth, blockHeight, dstColors, srcColors, left, bottom, top, right);
			for ( int i = 0; i < GameSettings.GetInstance().NumOfSmoothIteration; i++){
				smoother.Smooth (blockWidth, blockHeight, dstColors, srcColors, left, bottom, top, right);
			}
			drawTexture.SetPixels (xOrigin, yOrigin, blockWidth, blockHeight, dstColors);
			drawTexture.Apply ();
			blockToColor += blockWidth/6;
		    yield return 0;
        }
	    if (shouldCalFill)
	    {
	        UpdateFillPercent(drawTexture.GetPixels());
	    }

	    yield return 0;

    }
	void UpdateFillPercent (Color[] colors)
	{		
		int filledPixels = 0;
		for (int i = 0; i < colors.GetLength (0); ++i) {
			if ( Ultil.CalColorDifference(targetColor, colors[i]) < 1.0f ){
				filledPixels++;
			}
		}
		curFill = filledPixels;
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
	void MapTexture (Color color, int blockWidth, int blockHeight, Color[] dstColors, Color[] srcColors, int left, int bottom, int top, int right)
	{
		for (int x = left; x < right; x++) {
			for (int y = bottom; y < top; y++) {
				var colorIndex = x + y * blockWidth;
				if (srcColors [colorIndex].a <= 0) {
					continue;
				}
				float xCoord = (float)x / blockWidth;
				float yCoord = (float)y / blockHeight;
				float sample = dstColors [colorIndex].a + Mathf.PerlinNoise (xCoord, yCoord);
				dstColors [colorIndex] = Color.Lerp (dstColors [colorIndex], color , srcColors [colorIndex].a);
				dstColors [colorIndex].a = sample ;
			}
		}
	}

}
