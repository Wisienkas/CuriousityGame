using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CuriousityGame
{
    public class TileMap
    {
        public static readonly Vector2 MapDimensions = new Vector2(10, 10);
        public List<Tile> TileList = new List<Tile>();
        private readonly int[,] _map = new int[,]
        {
            //An example 2d-array of a map
            {2,2,1,2,2,2,1,1,1,1},
            {2,2,1,1,2,2,1,1,1,1},
            {2,1,1,1,1,2,1,1,1,1},
            {2,2,2,1,1,2,2,1,1,1},
            {2,1,2,1,1,1,2,2,2,2},
            {3,1,2,1,1,1,1,1,2,2},
            {2,1,2,2,1,1,2,1,1,2},
            {2,1,1,2,2,1,2,1,1,1},
            {2,2,1,1,2,2,2,1,1,1},
            {2,2,1,1,2,1,2,1,1,1},
        };

        public TileMap(int[,] map, Vector2 mapDimensions)
        {
            AddTiles(map, mapDimensions);
        }

        public TileMap()
        {
            AddTiles(_map, MapDimensions);
        }

        private void AddTiles(int[,] map, Vector2 mapDimensions)
        {
            for (int i = 0; i < mapDimensions.X; i++)
            {
                for (int j = 0; j < mapDimensions.Y; j++)
                {
                    TileList.Add(new Tile(map[j, i], j * 50, i * 50));
                }
            }
        }

        public List<Tile> GetTileList()
        {
            return TileList;
        }

        public Vector2 GetMapDimensions()
        {
            return MapDimensions;
        }

        /// <summary>
        /// Return True if the point is valid to move unto
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool CheckMove(Point point)
        {
            // Game is inverted on Y
            int y = invert(point.Y, 1);
            Point pointWrapped = wrapMap(point.X, y);
            int value = _map[pointWrapped.X, pointWrapped.Y];
            return value != 2;
        }

        private int invert(int value, int dimension)
        {
            return Math.Abs(value - (_map.GetLength(dimension) - 1));
        }

        private int wrapDimesion(int value, int dimension)
        {
            int d = _map.GetLength(dimension);
            if (value < 0) return value + d;
            if (value >= d) return value - d;
            return value;
        }

        private Point wrapMap(int x, int y)
        {
            return new Point()
            {
                X = wrapDimesion(x, 0),
                Y = wrapDimesion(y, 1)
            };
        }

    }
}
