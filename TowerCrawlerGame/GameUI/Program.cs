using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore;

namespace GameUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map gameMap = new Map(10, 10);
            Player hero = new Player("Marlo", 100, 20,50 );

            if (gameMap.PlaceEntity(hero, 5, 5)) Console.WriteLine("oyuncu kondu");
            else Console.WriteLine("konamadı");

            Console.WriteLine($"\n---HARİTA---");
            for (int y = 0; y < gameMap.Height; y++)
            {
                for (int x = 0; x < gameMap.Width; x++)
                {
                    Tile t = gameMap.Grid[x, y];
                    if (t.Occupant == hero) Console.Write("@");
                    else if (t.IsWall) Console.Write("#");
                    else Console.Write(".");
                }
                Console.WriteLine();
            }

            /*
                Enemy Goblin = new Enemy("japanese goblin", 100, 10, 20);
            Console.WriteLine($"\nDüşman Gördün: {Goblin.Name}");


            bool kactikMi = hero.TryFlee(Goblin);
            if (kactikMi)
                Console.WriteLine("Kaçtın!");
            else
                Console.WriteLine("Yakalandın!");

            */
            Console.ReadLine();
        }
    }
}
