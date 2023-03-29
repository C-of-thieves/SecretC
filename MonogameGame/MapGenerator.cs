using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Troschuetz.Random;

namespace MonogameGame
{
    public class MapGenerator
    {

        private int _mapWidth;
        private int _mapHeight;
        private float _fillProbability;
        private int _iterations;
        private Random _random;
        public int[,] MapDataInt { get; private set; }

        public MapGenerator(int mapWidth, int mapHeight, float fillProbability, int iterations)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _fillProbability = fillProbability;
            _iterations = iterations;

            _random = new Random();
            MapDataInt = new int[_mapWidth, _mapHeight];

            GenerateIslands();
        }


        private void GenerateIslands()
        {
            // Initial random fill
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (_random.NextDouble() < _fillProbability)
                    {
                        MapDataInt[x, y] = 1; // Land
                    }
                    else
                    {
                        MapDataInt[x, y] = 0; // Water
                    }
                }
            }

            // Cellular automata iterations
            for (int i = 0; i < _iterations; i++)
            {
                MapDataInt = RunCellularAutomataStep(MapDataInt);
            }
        }

        private int[,] RunCellularAutomataStep(int[,] map)
        {
            int[,] newMap = new int[_mapWidth, _mapHeight];

            Parallel.For(0, _mapWidth, x =>
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    int adjacentLandTiles = CountAdjacentLandTiles(x, y, map);

                    // Rule 1: If an empty cell has 4 or more filled neighbors, fill it
                    // Rule 2: If a filled cell has 3 or fewer filled neighbors, empty it
                    if (map[x, y] == 0 && adjacentLandTiles >= 20)
                    {
                        newMap[x, y] = 1;
                    }
                    else if (map[x, y] == 1 && adjacentLandTiles <= 3)
                    {
                        newMap[x, y] = 0;
                    }
                    else
                    {
                        newMap[x, y] = map[x, y];
                    }
                }
            });

            return newMap;
        }

        private int CountAdjacentLandTiles(int x, int y, int[,] map)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborX = x + i;
                    int neighborY = y + j;

                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (neighborX >= 0 && neighborX < _mapWidth && neighborY >= 0 && neighborY < _mapHeight)
                    {
                        count += map[neighborX, neighborY];
                    }
                }
            }

            return count;
        }

    }
}