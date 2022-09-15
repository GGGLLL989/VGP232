using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_MapEditor
{
    
    class LevelMap
    {
        public enum Tile
        {
            Player, Enemy, Wall, None
        }

        /// <summary>
        /// Tiles of the level map
        /// </summary>
        private Tile[,] tiles;

        /// <summary>
        /// Create a level map
        /// </summary>
        /// <param name="size">It equals the width and height of this level map</param>
        public LevelMap(int size)
        {
            tiles = new Tile[size, size];
            // Set all tiles to None
            for(int i = 0; i < tiles.GetLength(0); i++)
            {
                for(int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = Tile.None;
                }
            }
        }

        /// <summary>
        /// Check if tiles[row,col] is empty
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <returns></returns>
        public bool checkEmptyTileAt(int row, int col)
        {
            if(row >= 0 && row < tiles.GetLength(0) 
                 && col >= 0 && col < tiles.GetLength(1))
            {
                return tiles[row, col] != Tile.None;
            }
            return false;
        }

        /// <summary>
        /// Place one thing at the given location
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <param name="tile">the tile</param>
        public void placeThingAt(int row, int col, Tile tile)
        {
            tiles[row, col] = tile;
        }

        /// <summary>
        /// Remove the placed thing at the given location
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        public void removeThingAt(int row, int col)
        {
            tiles[row, col] = Tile.None;
        }

        /// <summary>
        /// Get the size of the level map
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            return tiles.GetLength(0);
        }

        /// <summary>
        /// Check if the level map contains nothing
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return true;
        }

        /// <summary>
        /// Serialize the level map
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool SaveToJsonFile(string filename)
        {
            return true;
        }

        /// <summary>
        /// Deserialize the level map
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static LevelMap LoadFromJsonFile(string filename)
        {
            return null;
        }
        /// <summary>
        /// Generate a level map with random maze
        /// </summary>
        /// <param name="size">the size of the level map</param>
        /// <returns></returns>
        public static LevelMap randomLevelMap(int size)
        {
            return null;
        }
    }
}
