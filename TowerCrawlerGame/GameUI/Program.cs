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
            gameMap.PlaceEntity(hero, 5, 5);

            Enemy goblin = new Enemy("Disheveled Goblin", 50, 5, 12);
            gameMap.PlaceEntity(goblin, 8, 5);

            string logMessage = "Kuleye hoş geldin...";

            bool isGameRunning = true;
            while (isGameRunning)
            {
                Console.Clear();
                DrawMap(gameMap, hero);

                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Durum: {logMessage}");
                Console.WriteLine($"{hero.Name} Statlar -> Can: {hero.CurrentHP} | Gizlilik: {hero.StealthSkill}");
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"konum:{hero.X}{hero.Y}");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int dx = 0;
                int dy = 0;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: dy = -1; break;
                    case ConsoleKey.DownArrow: dy = 1; break;
                    case ConsoleKey.LeftArrow: dx = -1; break;
                    case ConsoleKey.RightArrow: dx = 1; break;
                    case ConsoleKey.Escape: isGameRunning = false; break;
                }

                if (dx != 0 || dy != 0)
                {
                    int targetX = hero.X + dx;
                    int targetY = hero.Y + dy;

                    if (targetX >= 0 && targetY >= 0 && targetX < gameMap.Width && targetY < gameMap.Height)
                    {
                        Tile targetTile = gameMap.Grid[targetX, targetY];
                        if (targetTile.Occupant is Enemy enemy)
                        {
                            if (hero.StealthSkill < enemy.DetectionSkill)
                            {
                                logMessage = $"{enemy.Name} seni tespit etti! (düşman tespit seviyesi: {enemy.DetectionSkill} > gizlilik seviyen: {hero.StealthSkill})";
                            }
                            else
                            {
                                logMessage = $"{enemy.Name} seni görmedi.";
                            }
                        }
                        else if (targetTile.IsWall)
                        {
                            logMessage = "Duvar.";
                        }
                        else
                        {
                            gameMap.MoveEntity(hero, dx, dy);
                            logMessage = "İlerliyorsun...";
                        }


                    }
                }
            }
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
                    else if (t.Occupant != null)
                    {
                        Console.ForegroundColor= ConsoleColor.DarkRed;
                        Console.Write("X");
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
