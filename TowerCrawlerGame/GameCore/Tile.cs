using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Tile
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public bool IsWall { get; set; }
        public Entity Occupant { get; set; }
        public Tile(int x, int y, bool isWall)
        {
            X = x;
            Y = y;
            IsWall = isWall;
            Occupant = null;
            IsStairs = false;
        }
        public bool IsWalkable => !IsWall && Occupant == null;
        public bool IsStairs { get; set; }
        public bool HasObstacle => IsWall || Occupant != null;

    }
}
