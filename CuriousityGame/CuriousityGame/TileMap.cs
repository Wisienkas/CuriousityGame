using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CuriousityGame
{
    public class TileMap
    {
        public static readonly Vector2 MapDimensions = new Vector2(10,10);
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

        public static bool CheckMove(Move m)
        {
            //TODO: Check if map is blocked
            throw new NotImplementedException();
        }

    }
}
