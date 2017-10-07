using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ultil  {
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
}
