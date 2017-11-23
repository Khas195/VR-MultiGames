using System;
using Assets.script;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public interface ISmoothPaint
{
	void Smooth(int blockWidth,int blockHeight, Color[] dstColors, Color[] srcColors,int left,int bottom,int top,int right);
}

public class GaussianSmoothing : ISmoothPaint{
	int sigma;

	public GaussianSmoothing(int sigma){
		this.sigma = sigma;
	}

	#region ISmoothPaint implementation

	public void Smooth (int blockWidth, int blockHeight, Color[] dstColors, Color[] srcColors, int left, int bottom, int top, int right)
	{
		for (int x = left + 1; x < right - 1; x++) {
			for (int y = bottom + 1; y < top - 1; y++) {
				var colorIndex = x + y * blockWidth;
				Vector4 average = new Vector4 ();

				for (int i = x - 1; i <= x + 1; ++i) {
					for (int j = y - 1; j <= y + 1; ++j) {
						var index = i + j * blockWidth;
						float t1 = -(i * i + j * j) / 2 * sigma * sigma;
						float gaussian = 1/(2.0f*Mathf.PI*Mathf.Pow(sigma, Mathf.Pow(2.0f,Mathf.Exp(t1))));
						average.x += dstColors [index].r * gaussian;
						average.y += dstColors [index].g * gaussian;
						average.z += dstColors [index].b * gaussian;
						average.w += dstColors [index].a * gaussian;
					}
				}
				dstColors [colorIndex] = average;
			}
		}
	}

	#endregion


}