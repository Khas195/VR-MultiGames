using UnityEngine;

public class Paintable : MonoBehaviour
{
    [SerializeField]
    private string paintableShaderName;

    private Material mat;

    void Start()
    {
        var ren = this.GetComponent<Renderer>();

        mat = GetPaintableMaterial(ren);
        if (mat == null)
        {
            enabled = false;
        }

        drawTexture = mat.GetTexture(PaintableDefinition.DrawOnTextureName);

    }



    private Material GetPaintableMaterial(Renderer renderer)
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            if (renderer.materials[i].name.Equals(paintableShaderName))
            {
                return renderer.materials[i];
            }
        }
        return null;
    }

    public void Paint(Vector2 texCoord)
    {
    }
}