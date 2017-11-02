using System;
using Assets.script;
using UnityEngine;

class Paintable : MonoBehaviour
{
	[SerializeField]
	bool InitOnStart;
	[SerializeField]
	float paintSizeMultiplier = 1;
    private Material mat;
    private Texture2D drawTexture;

	bool init = false;

	float curFill;

	float totalFill;

	public float GetFillPercentage ()
	{
		return curFill / totalFill;
	}

    void Start()
	{       
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
		mat = Ultil.GetMaterialWithShader(GetComponent<Renderer>().materials, PaintableDefinition.PaintableShaderName, name);

		var mainTex = mat.GetTexture(PaintableDefinition.MainTexture);
		var scaleX = transform.localScale.x;
		var scaleY = transform.localScale.y;
		var ratio = scaleX / scaleY;

		drawTexture = new Texture2D(Mathf.RoundToInt(GameSettings.GetInstance().InkSplatSize * ratio * paintSizeMultiplier)
			, Mathf.RoundToInt(GameSettings.GetInstance().InkSplatSize * paintSizeMultiplier));
		ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));

		mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
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

    public void Update()
    {
    }
    private bool IsPositionValid(int x, int y, int colorIndex, int length, int width, int height)
    {
        return (x >= 0 && x < width) && (y >= 0 && y < height)
               && colorIndex < length;
    }
    public bool PaintMapping(Vector2 textureCoord, Texture2D ink, Color randomColor)
    {
		if (!init) return false;
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
		Color[] inkColors = ink.GetPixels (inkLeft, inkBottom, blockWidth , blockHeight);

		for (int x = 0; x < blockWidth; x++)
	    {
			for (int y = 0; y < blockHeight; y++)
	        {
				
				var colorIndex = x  + y * blockWidth;
				if (colors [colorIndex].a <= 0 && inkColors[colorIndex].a > 0) {
					curFill++;
				}
				colors[colorIndex] = Color.Lerp(colors[colorIndex], randomColor, inkColors[colorIndex].a);
				colors[colorIndex].a = inkColors[colorIndex].a +  colors[colorIndex].a;
	        }
	    }

		drawTexture.SetPixels(xOrigin, yOrigin, blockWidth, blockHeight, colors);
        drawTexture.Apply();
		return true;
    }
}
