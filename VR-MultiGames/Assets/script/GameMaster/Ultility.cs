using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ultil  {

	/// <summary>
	/// Get the object the camera is looking at
	/// </summary>
	public static bool GetObjectCameraIsLookingAt (Camera lookatCamera, out RaycastHit hit)
	{
		return Physics.Raycast (lookatCamera.transform.position, lookatCamera.transform.forward, out hit);
	}

    /// <summary>
    /// Returns a if a == b
    /// </summary>
    public static int GetBigger(int a, int b)
    {
        return a >= b ? a : b;
    }
    /// <summary>
    /// Go to the arrays row by row
    /// </summary>
    public static int ToOneDimension(int posX, int poY, int width)
    {
        return posX + poY * width;
    }

	public static Material GetMaterialWithShader (Material[] materials, string shaderName, string callerName)
	{       
		for (int i = 0; i < materials.Length; i++)
		{
			if (materials[i].shader.name.Equals(shaderName))
			{
				return materials[i];
			}
		}
		UnityEngine.Debug.LogError("Cannot find Material with Shader " + shaderName + " for " + callerName);
		return GameDefinition.DefaultMaterial;
	}
}
