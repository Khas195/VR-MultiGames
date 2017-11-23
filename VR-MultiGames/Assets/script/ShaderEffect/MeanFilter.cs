using System;
using Assets.script;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class MeanFilter : ISmoothPaint
{
	#region ISmoothPaint implementation
	public void Smooth (int blockWidth, int blockHeight, Color[] dstColors, Color[] srcColors, int left, int bottom, int top, int right)
	{
		for (int x = left + 1; x < right - 1; x++) {
			for (int y = bottom + 1; y < top - 1; y++) {
				var colorIndex = x + y * blockWidth;
				Vector4 average = new Vector4 ();

				for (int i = x - 1; i <= x + 1; ++i) {
					for (int j = y - 1; j <= y + 1; ++j) {
						if (i == j) {
							continue;
						}
						var index = i + j * blockWidth;
						average.x += dstColors [index].r;
						average.y += dstColors [index].g;
						average.z += dstColors [index].b;
						average.w += dstColors [index].a;
					}
				}
				dstColors [colorIndex] = average / 8.0f;
			}
		}
	}
	#endregion
}

