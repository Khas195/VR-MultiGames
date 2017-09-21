using System;
using Assets.script;
using UnityEngine;

class Paintable : MonoBehaviour
{
    private Material mat;
    private Texture2D drawTexture;
    private int radius = 5;
    void Start()
    {
        mat = GetPaintableMaterial(GetComponent<Renderer>().materials);
        var mainTex = mat.GetTexture(PaintableDefinition.MainTexture);
        drawTexture = new Texture2D(mainTex.width, mainTex.height);
        ResetTexture(drawTexture);
        mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
    }

    private void ResetTexture(Texture2D texture)
    {
        Color[] colors = texture.GetPixels();
        Color empty = new Color(1, 1, 1, 1);
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = empty;
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
    
    public void Paint(Vector2 textureCoord)
    {
        int x = (int)(textureCoord.x * drawTexture.width) ;
        int y = (int)(textureCoord.y * drawTexture.height) ;
        for (int j = y - radius; j <= y + radius; j++)
        {
            for (int i = x - radius; i <= x + radius; i++)
            {
                if (i > drawTexture.width || j > drawTexture.height || i < 0 || j < 0) continue;
                drawTexture.SetPixel(i, j, Color.red);
            }
        }

        drawTexture.Apply();
    }
}