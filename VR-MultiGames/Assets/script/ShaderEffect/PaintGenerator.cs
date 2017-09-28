using UnityEngine;

public class PaintGenerator
{
    private Color[] colorMap;
    private int width;
    private int height;

    public PaintGenerator(Color[] colorMap, int width, int height)
    {
        this.colorMap = colorMap;
        this.width = width;
        this.height = height;
    }

    public Color[] GenerateRandomShape(int startX, int endX, int startY, int endY)
    {
        string seed = Time.time.ToString();
        System.Random pesudoRandom = new System.Random(seed.GetHashCode());
        RandomGenMap(startX, endX, startY, endY, pesudoRandom);
        SmoothMap(startX, endX, startY, endY);
        ApplyNoiseOnExistingShape(startX, endX, startY, endY);

        return colorMap;
    }

    private void ApplyNoiseOnExistingShape(int startX, int endX, int startY, int endY)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                var colorIndex = Ultil.ToOneDimension(x, y, width);
                if (IsPositionValid(x, y, colorIndex))
                {
                    if (colorMap[colorIndex].a > 0)
                    {
                        float xCoord = (float)x / width * 20f;
                        float yCoord = (float)x / height * 20f;
                        float noise = Mathf.PerlinNoise(xCoord, yCoord);
                        colorMap[colorIndex].a = noise < 0.5f ? 0.5f : noise;

                    }
                }
            }
        }
    }

    private void SmoothMap(int startX, int endX, int startY, int endY)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                var colorIndex = Ultil.ToOneDimension(x, y, width);
                if (IsPositionValid(x, y, colorIndex))
                {
                    int wallCount = GetSurroundingWall(x, y);
                    if (wallCount > 4)
                    {
                        colorMap[colorIndex].a = 1;
                    }
                    else if (wallCount < 4)
                    {
                        colorMap[colorIndex].a = 0;
                    }
                }
            }
        }
    }

    private bool IsPositionValid(int x, int y, int colorIndex)
    {
        return (x >= 0 && x < width) && (y >= 0 && y < height)
                            && colorIndex < colorMap.Length;
    }

    private int GetSurroundingWall(int posX, int posY)
    {
        int result = 0;
        for (int neighborX = posX - 1; neighborX <= posX + 1; neighborX++)
        {
            for (int neighborY = posY - 1; neighborY <= posY + 1; neighborY++)
            {

                var colorIndex = Ultil.ToOneDimension(neighborX, neighborY, width);
                if (IsPositionValid(neighborX, neighborY, colorIndex))
                {
                    if (neighborX != posX || neighborY != posY)
                    {
                        result += (int) Mathf.Round(colorMap[colorIndex].a);
                    }
                }
                else
                {
                    result--;
                }
            }
        }
        return result;
    }

    private void RandomGenMap(int startX, int endX, int startY, int endY, System.Random pesudoRandom)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                var colorIndex = Ultil.ToOneDimension(x, y, width);
                if (IsPositionValid(x,y,colorIndex))
                {
                    if (colorMap[colorIndex].a > 0) continue;
                    if (IsBorder(x, y))
                    {
                        colorMap[colorIndex].a = 0;
                    }
                    else colorMap[colorIndex].a = pesudoRandom.Next(0, 100) < GameSettings.GetInstance().PaintFilPercent ? 1 : 0;
                }
            }
        }
    }

    private bool IsBorder(int x, int y)
    {
        return x == 0 || y == 0 || x == width || y == height;
    }
}