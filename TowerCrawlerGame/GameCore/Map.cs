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
            if (x < 0 || x >= Width || y < 0 || y >= Height) return false;
            Tile targetTile = Grid[x, y];
            if (targetTile.IsWalkable)
            {
                targetTile.Occupant = entity;
                entity.X = x;
                entity.Y = y;
                return true;
            }
            return false;
        }
        

        public bool MoveEntity(Entity entity, int dx, int dy)
        {
            int targetX = entity.X + dx;
            int targetY=entity.Y + dy;

            if(targetX < 0 || targetY < 0||targetX>=Width||targetY>=Height) return false;
            
            Tile targetTile = Grid[targetX,targetY];
            if(targetTile.IsWalkable)
            {
                Grid[entity.X, entity.Y].Occupant = null;
                entity.X=targetX;
                entity.Y=targetY;
                targetTile.Occupant = entity;
                return true;
            }
            return false;
        }
        /*
         
         //gelişmiş kod

        public bool PlaceEntity(Entity entity, int x, int y, bool removeOld = false)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

            Tile targetTile = Grid[x, y];
            if (!targetTile.IsWalkable) return false;

            if (removeOld)
                Grid[entity.X, entity.Y].Occupant = null;

            targetTile.Occupant = entity;
            entity.X = x;
            entity.Y = y;
            return true;
        }
        public bool MoveEntity(Entity entity, int dx, int dy)
        {
            return PlaceEntity(entity, entity.X + dx, entity.Y + dy, removeOld: true);
        }

        */
    }
}
