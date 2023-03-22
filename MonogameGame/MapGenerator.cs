using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

    public class MapGenerator
    {
        private Random random;
        private int[,] map;

        public MapGenerator(int width, int height, int seed)
        {
            random = new Random(seed);
            map = new int[width, height];
        }

        public int[,] GenerateMap()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                int randomNumber = random.Next(0, 10);
                if (randomNumber < 8)
                    map[x, y] = 0; // Grass
                else
                    map[x, y] = 1; // Tree
            }
            }

            return map;
        }
    }

    
    
