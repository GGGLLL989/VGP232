using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

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
        /// Whether the level map has one player
        /// </summary>
        private bool hasPlayer;

        /// <summary>
        /// Create a level map
        /// </summary>
        /// <param name="size">It equals the width and height of this level map</param>
        public LevelMap(int size)
        {
            hasPlayer = false;
            tiles = new Tile[size, size];
            
            // Set all tiles to None
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for(int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = Tile.None;
                }
            }
        }


        /// <summary>
        /// Return the tile at the given location
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <returns></returns>
        public Tile GetTile(int row, int col)
        {
            return tiles[row, col];
        }

        /// <summary>
        /// Check if tiles[row,col] is empty
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <returns></returns>
        public bool CheckEmptyTileAt(int row, int col)
        {
            if(row >= 0 && row < tiles.GetLength(0) 
                 && col >= 0 && col < tiles.GetLength(1))
            {
                return tiles[row, col] == Tile.None;
            }
            return false;
        }

        /// <summary>
        /// Place one thing at the given location
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <param name="tile">the tile</param>
        public void PlaceThingAt(int row, int col, Tile tile)
        {
            if(tile == Tile.Player)
            {
                hasPlayer = true;
            }
            tiles[row, col] = tile;
        }

        /// <summary>
        /// Remove the placed thing at the given location
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        public void RemoveThingAt(int row, int col)
        {
            if(tiles[row, col] == Tile.Player)
            {
                hasPlayer = false;
            }
            tiles[row, col] = Tile.None;
        }



        /// <summary>
        /// Check if the level map has one player
        /// </summary>
        /// <returns></returns>
        public bool HasPlayer()
        {
            return hasPlayer;
        }

        /// <summary>
        /// Get the size of the level map
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return tiles.GetLength(0);
        }

        /// <summary>
        /// Check if the level map contains nothing
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j] != Tile.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Serialize the level map
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool SaveToJsonFile(string filename)
        {
            // Convert the array to the list
            Map map = new Map();
            map.lst = new List<MTile>();
            map.size = tiles.GetLength(0);
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for(int j = 0; j <tiles.GetLength(1); j++)
                {
                    if(tiles[i, j] != Tile.None)
                    {
                        map.lst.Add(new MTile { TileName = tiles[i, j].ToString(), x = j, y = i });
                    }
                }
            }

            // Serialization
            string json = JsonSerializer.Serialize<Map>(map);
            File.WriteAllText(filename, json);
            return true;
        }

        /// <summary>
        /// Deserialize the level map
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static LevelMap LoadFromJsonFile(string filename)
        {
            try
            {
                // Deserialization
                string jsonText = File.ReadAllText(filename);
                Map map = JsonSerializer.Deserialize<Map>(jsonText);

                // Create a level map
                LevelMap levelMap = new LevelMap(map.size);
                foreach(MTile mTile in map.lst)
                {
                    if(mTile.TileName == "Player")
                    {
                        levelMap.PlaceThingAt(mTile.y, mTile.x, Tile.Player);
                    }
                    else if (mTile.TileName == "Enemy")
                    {
                        levelMap.PlaceThingAt(mTile.y, mTile.x, Tile.Enemy);
                    }
                    else if (mTile.TileName == "Wall")
                    {
                        levelMap.PlaceThingAt(mTile.y, mTile.x, Tile.Wall);
                    }
                }
                return levelMap;
            }catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Generate a level map with random maze
        /// </summary>
        /// <param name="size">the size of the level map</param>
        /// <returns></returns>
        public static LevelMap RandomLevelMap(int size)
        {
            LevelMap levelMap = new LevelMap(size);

            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (rd.Next(10) < 2)
                    {
                        levelMap.PlaceThingAt(i, j, Tile.Wall);
                    }
                }
            }
            return levelMap;
        }
    }
}
