using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Map
    {
        public Tile[,] Grid {  get; private set; }
        public int Width { get; }
        public int Height { get; }
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new Tile[width, height];
            GenerateEmptyMap();

        }
        public void GenerateEmptyMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    bool isBorder = (x == 0 || x == Width - 1 || y == 0 || y == Height - 1);
                    Grid[x, y] = new Tile(x, y, isBorder);
                }
            }
        }

        public bool PlaceEntity(Entity entity, int x, int y)
        {
            if(x<0||x>=Width||y<0||y>=Height) return false;
            Tile targetTile = Grid[x,y];
            if (targetTile.IsWalkable)
            {
                targetTile.Occupant = entity;
                return true;
            }
            return false;
        }
    }
}
