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
            
            // Veri Yükleme
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string itemsPath = Path.Combine(baseDir, "Data", "items.json");
            string enemiesPath = Path.Combine(baseDir, "Data", "enemies.json");

            Console.WriteLine($"Veri yolu kontrol ediliyor: {baseDir}");

            ObjectFactory.LoadData(itemsPath, enemiesPath);
                
            SaveManager.LoadGame();
            bool appRunning = true;
            while (appRunning)
            {
                if (!SaveManager.CurrentState.IsRealityEstablished)
                {
                    // ---(TUTORIAL / PERMADEATH) ---
                    Console.Clear();
                    Console.WriteLine("        ");
                    Console.WriteLine("Başlamak için bir tuşa bas...");
                    Console.ReadKey();

                    // İlk karakterle başlat
                    bool success = StartRun("toyo", isGenesisRun: true);

                    if (success)
                    {
                        Console.WriteLine("Yeni yola ulaştın.");
                        SaveManager.CurrentState.IsRealityEstablished = true;
                        SaveManager.SaveGame();
                    }
                    else
                    {
                        Console.WriteLine("Yeni yolu bulamadın");
                        Console.WriteLine("TÜM İLERLEME SİLİNDİ.");
                        SaveManager.WipeReality();
                        Console.ReadKey();
                    }
                }
                else
                {
                    // ---BASE HUB (NORMAL OYUN) ---
                    Console.Clear();
                    Console.WriteLine("=== BASE: HAFIZA KRİSTALİ ===");
                    Console.WriteLine($"Hafıza Doluluğu: {SaveManager.CurrentState.MemoryFragments}/{SaveManager.CurrentState.CrystalCapacity}");
                    Console.WriteLine("1. Kuleye Gir");
                    Console.WriteLine("2. Karakter Seç");
                    Console.WriteLine("3. Kristal Yönetimi");
                    Console.WriteLine("ESC. Çıkış");

                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.D1)
                    {
                        // Normal Run
                        bool alive = StartRun("toyo", isGenesisRun: false);
                        if (!alive)
                        {
                            Console.WriteLine("Karakter öldü. Anıları kulenin içinde yankılanıyor.");
                        }
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        appRunning = false;
                    }
                }
            }
        }

        //Oyun Başlatma
        static bool StartRun(string characterId, bool isGenesisRun)
        {
            int currentFloor = 1;
            Player hero = new Player("Marlo", 100, 10, 10);

            Map gameMap = SetupLevel(currentFloor, hero);
            int targetFloor = isGenesisRun ? 3 : 99;
            // Silah Ayarları
            Weapon ironSword = new Weapon("Demir Kılıç");
            CombatMove quickSlash = new CombatMove("Hızlı Kesik", 15, (attacker, target) => { });
            CombatMove heavyHit = new CombatMove("Ağır Darbe", 35, (attacker, target) => { attacker.SpeedPenalty += 10; });

            ironSword.AddMove(quickSlash);
            ironSword.AddMove(heavyHit);
            hero.EquippedWeapon = ironSword;

            string logMessage = "Kuleye Hoş Geldin!";
            bool isGameRunning = true;


            // OYUN DÖNGÜSÜ
            while (isGameRunning)

            {
                Console.Clear();
                DrawMap(gameMap, hero);

                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Durum: {logMessage}");
                Console.WriteLine($"{hero.Name} Statlar -> Can: {hero.CurrentHP} | Gizlilik: {hero.StealthSkill}");
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Konum: {hero.X},{hero.Y}");
                Console.WriteLine("Hareket: W/A/S/D | Envanter: I | Çıkış: ESC");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int dx = 0; int dy = 0;

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

                            for (int i = 0; i < hero.EquippedWeapon.Moves.Count; i++)
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
                                    if (enemy.Loot != null)
                                    {
                                        Console.WriteLine($"\n {enemy.Name} yere eşya düşürdü:{enemy.Loot.Name}");
                                        Console.WriteLine("Almak istiyor musun? E'ye bas! ");
                                        var lootChoice = Console.ReadKey().Key;
                                        if (lootChoice == ConsoleKey.E)
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

                if (hero.IsDead)
                {
                    return false;
                }

                Tile currentTile = gameMap.Grid[hero.X, hero.Y];

                if (currentTile.IsStairs)
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
                    if (isGenesisRun && currentFloor >= targetFloor)
                    {
                        return true;
                    }
                }
            }
        return false;
        }
        static Map SetupLevel(int floorLevel, Player hero)
        {
            Map newMap = new Map(20, 10);
            newMap.PlaceEntityRandomly(hero);

            int enemyCount = floorLevel;
            if (enemyCount > 5) enemyCount = 5;

            Console.WriteLine($"--- Kat {floorLevel} Hazırlanıyor: {enemyCount} Düşman Eklenecek ---");

            for (int i = 0; i < enemyCount; i++)
            {
                string enemyId = "goblin_scout";
                if (floorLevel >= 3 && new Random().Next(0, 2) == 0)
                {
                    enemyId = "orc_warrior";
                }

                // Düşmanı yarat
                Enemy enemy = ObjectFactory.CreateEnemy(enemyId);

                // --- HATA TESPİT BLOĞU ---
                if (enemy == null)
                {
                    // EĞER BU YAZIYI GÖRÜYORSAN: JSON dosyasındaki "id" ile buradaki "enemyId" tutmuyor demektir.
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"!!! HATA: '{enemyId}' ID'li düşman üretilemedi! JSON dosyasını kontrol et.");
                    Console.ResetColor();
                }
                else
                {
                    enemy.BoostHealth(floorLevel * 5);
                    enemy.Name += $" {i + 1}";

                    bool basari = newMap.PlaceEntityRandomly(enemy);

                    if (basari)
                    {
                        Console.WriteLine($"-> {enemy.Name} haritaya yerleşti ({enemy.X},{enemy.Y}).");
                    }
                    else
                    {
                        Console.WriteLine($"-> {enemy.Name} için BOŞ YER BULUNAMADI!");
                    }
                }
            }
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
                        Console.ForegroundColor= ConsoleColor.Red;
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
