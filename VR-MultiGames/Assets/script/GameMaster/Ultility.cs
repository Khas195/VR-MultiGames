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

	///////////// QUADRATIC EASING: t^2 ///////////////////

	/// <summary>
	// quadratic easing in - accelerating from zero velocity
	// t: current time, b: beginning value, c: change in value, d: duration
	// t and d can be in frames or seconds/milliseconds
	/// </summary>
	public static float EaseInQuad (float t,float b,float c,float d) {
		return c*(t/=d)*t + b;
	}

	// quadratic easing out - decelerating to zero velocity
	public static float EaseOutQuad (float t,float b,float c,float d) {
		return -c *(t/=d)*(t-2) + b;
	}

	// quadratic easing in/out - acceleration until halfway, then deceleration
	public static float EaseInOutQuad (float t,float b,float c,float d) {
		if ((t/=d/2) < 1) return c/2*t*t + b;
		return -c/2 * ((--t)*(t-2) - 1) + b;
	}


}
