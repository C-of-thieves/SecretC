using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexNoise;

public static class PerlinNoiseGenerator
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0)
        {
            scale = 0.001f;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;
                float perlinValue = Noise.CalcPixel2D((int)sampleX, (int)sampleY, 0.1f);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}