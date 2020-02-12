using System;
using UnityEngine;

namespace flexington.Automata
{
    public static class Automata
    {
        public static int[,] Generate(Vector2Int mapSize, int threshold = 47, string seed = null)
        {
            // Set seed if default
            if (seed == null || seed == string.Empty) seed = DateTime.Now.Millisecond.ToString();

            // Instantiate Random Number Generator
            System.Random rng = new System.Random(seed.GetHashCode());

            // Init Map
            int[,] map = new int[mapSize.x, mapSize.y];

            // Fill map
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = (rng.Next(0, 100) <= threshold) ? 1 : 0;
                }
            }

            return map;
        }

        public static int[,] Simulate(int[,] current, int iterations)
        {
            int[,] next = new int[current.GetUpperBound(0) + 1, current.GetUpperBound(1) + 1];
            for (int i = 0; i < iterations; i++)
            {
                for (int x = 0; x < current.GetLength(0); x++)
                {
                    for (int y = 0; y < current.GetLength(1); y++)
                    {
                        int neighbours = CountNeighbour(current, x, y);

                        if (neighbours > 4) next[x, y] = 1;
                        else if (neighbours < 4) next[x, y] = 0;
                    }
                }
            }
            return next;
        }

        private static int CountNeighbour(int[,] map, int posX, int posY)
        {
            int neighbours = 0;
            for (int x = posX - 1; x <= posX + 1; x++)
            {
                for (int y = posY - 1; y <= posY + 1; y++)
                {
                    if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
                    {
                        if (x != posX || y != posY) neighbours += map[x, y];
                    }
                    else
                    {
                        neighbours++;
                    }
                }
            }
            return neighbours;
        }
    }
}