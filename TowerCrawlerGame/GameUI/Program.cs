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
            int currentFloor = 1;
            
            Player hero = new Player("Marlo", 100, 10, 10);

            Map gameMap = SetupLevel(currentFloor, hero);            
            Weapon ironSword = new Weapon("Demir Kılıç");

            CombatMove quickSlash = new CombatMove("quick thinker", 15, (attacker, target) => {/* effects*/});
            CombatMove heavyHit = new CombatMove("Heavy and clear", 35, (attacker, target) => { attacker.SpeedPenalty += 10; });

            ironSword.AddMove(quickSlash);
            ironSword.AddMove(heavyHit);

            hero.EquippedWeapon = ironSword;

            Enemy goblin = new Enemy("Disheveled Goblin", 50, 15, 12);
            
            Food goblinMeat = new Food("Goblin meat", 2, 2, (p) => { p.StealthSkill -= 2; });

            goblin.SetLoot(goblinMeat);
           // gameMap.PlaceEntity(goblin, 8, 5);

            string logMessage = "Kuleye Hoş Geldin!";
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
                Console.WriteLine("Hareket: Ok Tuşları | Envanter: I | Çıkış: ESC");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int dx = 0;
                int dy = 0;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.W: dy = -1; break;
                    case ConsoleKey.S: dy = 1; break;
                    case ConsoleKey.A: dx = -1; break;
                    case ConsoleKey.D: dx = 1; break;
                    case ConsoleKey.I: OpenInventory(hero); break;
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
                            Console.Clear();
                            Console.WriteLine($"Düşman Canı: {enemy.CurrentHP} | Senin Gizlilik: {hero.StealthSkill}");
                            Console.WriteLine("Saldırı Seç:");

                            for (int i = 0;i<hero.EquippedWeapon.Moves.Count ;i++)
                            {
                                var move = hero.EquippedWeapon.Moves[i];
                                Console.WriteLine($"{i + 1}.{move.Name} (hasar{move.Damage})");
                            }

                            Console.Write("Seçimini yap (Numara gir): ");
                            char secim = Console.ReadKey().KeyChar;
                            int index = (int)char.GetNumericValue(secim) - 1;

                            if (index >= 0 && index < hero.EquippedWeapon.Moves.Count)
                            {
                                CombatMove selectedMove = hero.EquippedWeapon.Moves[index];
                                enemy.TakeDamage(selectedMove.Damage);
                                selectedMove.OnExecute?.Invoke(hero, enemy);
                                logMessage = $"{selectedMove.Name} kullandın! Düşmana {selectedMove.Damage} hasar verdin.";
                                if (enemy.IsDead)
                                {
                                    logMessage += $"{enemy.Name} öldü!";
                                    gameMap.Grid[targetX, targetY].Occupant = null;
                                    if(enemy.SetLoot!=null)
                                    {
                                        Console.WriteLine($"\n {enemy.Name} yere eşya düşürdü:{enemy.Loot.Name}");
                                        Console.WriteLine("Almak istiyor musun? E'ye bas! ");
                                        var lootChoice = Console.ReadKey().Key;
                                        if (lootChoice==ConsoleKey.E)
                                        {
                                            if (hero.AddToInventory(enemy.Loot)) logMessage += $" | {enemy.Loot.Name} çantaya eklendi.";
                                            else logMessage += " | Çanta dolu! Eşya yerde kaldı.";

                                        }
                                        else logMessage += "| eşyayı almadın.";
                                    }
                                }
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
                Tile currentTile = gameMap.Grid[hero.X, hero.Y];
                
                if(currentTile.IsStairs)
                {
                    Console.Clear();
                    Console.WriteLine("üst kat");
                    Console.WriteLine("1. Yukarı Çık (Devam)");
                    Console.WriteLine("2. Alt Kata Dön");
                    Console.WriteLine("3. Burada Kal");
                    Console.WriteLine("4. Oyunu Kaydet");

                    var choice = Console.ReadKey().Key;

                    if (choice == ConsoleKey.D1)
                    {
                        currentFloor++;
                        gameMap = SetupLevel(currentFloor, hero);
                        logMessage = $"{currentFloor}. Kat";
                    }
                    else if (choice == ConsoleKey.D2)
                    {
                        currentFloor--;
                        //eski katı yükle.
                    }
                    else if (choice == ConsoleKey.D3)
                    {
                        Console.WriteLine("\nkat değişmedi");
                    }
                    else if (choice == ConsoleKey.D2)
                    {
                        Console.WriteLine("\nBase'e döndün'");
                    }
                }
            }
            Console.ReadLine();
        }
        static Map SetupLevel(int floorLevel, Player hero)
        {
            Map newMap = new Map(20, 10);
            newMap.PlaceEntity(hero, 1, 1);

            //enemy statları oyuncunun statlarına göre ayarlanacak daha sonra.
            int enemyHP = 40 + (floorLevel * 10);
            int enemyDmg = 5 + (floorLevel * 2);

            Enemy floorEnemy = new Enemy($"{floorLevel} düşmanı", enemyHP, 5, 10);
            Food goblinMeat = new Food("Goblin meat", 2, 2, (p) => { p.StealthSkill -= 2; });

            floorEnemy.SetLoot(goblinMeat);
            // Lootlar başka şekilde ayarlanacak.
            /* Food loot = new Food("Garip Et", 2, 10 * floorLevel);
             floorEnemy.SetLoot(loot);*/
            newMap.PlaceEntity(floorEnemy, 10, 5);
            return newMap;
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
                    else if(t.IsStairs)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("H");
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
        static void OpenInventory(Player hero)
        {
            Console.Clear();
            Console.WriteLine("---ENVANTER---");
            Console.WriteLine($"Kapasite:{hero.Inventory.Count}/{hero.MaxInventorySize}");
            Console.WriteLine($"Güncel Hızın:{hero.SpeedSkill}");
            Console.WriteLine("--------------");

            if (hero.Inventory.Count == 0)
            {
                Console.WriteLine("Çantan Boş");
            }
            else
            {
                for (int x = 0; x < hero.Inventory.Count; x++)
                {
                    Item item =hero.Inventory[x];
                    Console.Write($"{x+1}.{item.Name}(Ağırlık:{item.Weight})");

                    if(item is Food food)
                        Console.Write($"[Yenebilir-Can: +{ food.RestoreAmount}]");

                    Console.WriteLine();
                }
            }
            Console.WriteLine("\nyemek için sayı gir. çıkmak için ESC");
            var key= Console.ReadKey(true);


            if (char.IsDigit(key.KeyChar)) 
            {
                int index = (int)char.GetNumericValue(key.KeyChar)-1;
                if(index>=0&&index<hero.Inventory.Count)
                {
                    Item selected=hero.Inventory[index];
                    if(selected is Food foodToEat)
                    {
                        hero.EatItem(foodToEat);
                        Console.WriteLine($"{foodToEat.Name} yendi.+{foodToEat.RestoreAmount}");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("yenmez");
                        Console.ReadKey();
                    }
                }

            }
        }
        
    }
}
