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
            Map gameMap = new Map(20, 10);
            Player hero = new Player("Marlo", 100, 20, 50);

            if (gameMap.PlaceEntity(hero, 5, 5)) Console.WriteLine("oyuncu kondu");
            else Console.WriteLine("konamadı");


            bool isGameRunning = true;
            while (isGameRunning)
            {
                Console.Clear();
                DrawMap(gameMap, hero);
                Console.WriteLine($"konum:{hero.X}{hero.Y}");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                int dx = 0;
                int dy = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: dy = -1; break; // Yukarı (Y azalır)
                    case ConsoleKey.DownArrow: dy = 1; break; // Aşağı (Y artar)
                    case ConsoleKey.LeftArrow: dx = -1; break; // Sola (X azalır)
                    case ConsoleKey.RightArrow: dx = 1; break; // Sağa (X artar)
                    case ConsoleKey.Escape: isGameRunning = false; break;
                }

                // 3. Hareket Mantığını Çağır
                if (dx != 0 || dy != 0)
                {
                    gameMap.MoveEntity(hero, dx, dy);
                }
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
        static void DrawMap(Map map, Player hero)
        {
            Console.WriteLine("\n---HARİTA---");
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Tile t = map.Grid[x, y];
                    if (t.Occupant == hero)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("@");
                    }
                    else if (t.IsWall)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("#");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
