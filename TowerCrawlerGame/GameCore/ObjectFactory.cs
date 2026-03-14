using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace GameCore
{
    public static class ObjectFactory
    {
        private static Dictionary<string, ItemData> _itemDatabase;
        private static Dictionary<string, EnemyData> _enemyDatabase;
        private static bool _isLoaded = false;

        public static void LoadData(string itemsPath, string enemiesPath)
        {
            try
            {
                string jsonItems = File.ReadAllText(itemsPath);
                var itemList = JsonSerializer.Deserialize<List<ItemData>>(jsonItems, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _itemDatabase = itemList.ToDictionary(x => x.Id);

                string jsonEnemies = File.ReadAllText(enemiesPath);
                var enemyList = JsonSerializer.Deserialize<List<EnemyData>>(jsonEnemies, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _enemyDatabase = enemyList.ToDictionary(x => x.Id);

                _isLoaded = true;
                Console.WriteLine("Veritabanı başarıyla yüklendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: Veri yüklenemedi! {ex.Message}");
                
                Console.WriteLine(ex.StackTrace);
                throw; // Hatayı Main bloğuna fırlat ki programın durduğunu anlayalım
            }
        }
        public static Item CreateItem(string id)
        {
            if (!_isLoaded || !_itemDatabase.ContainsKey(id)) return null;

            ItemData data = _itemDatabase[id];

            if (data.Type == "Food")
            {
                return new Food(data.Id, data.Name, data.Weight, data.Value);
            }

            else if (data.Type == "Weapon")
            {
                Weapon w = new Weapon(data.Id, data.Name);
                // Silahın moveseti
                w.AddMove(new CombatMove("Normal Vuruş", data.Value));
                return (Item)w;
            }

            return new Item(data.Id, data.Name, data.Weight);
        }
        public static Enemy CreateEnemy(string id)
        {
            if (!_isLoaded || !_enemyDatabase.ContainsKey(id)) return null;

            EnemyData data = _enemyDatabase[id];

            Enemy enemy = new Enemy(data.Name, data.Hp, data.Speed, data.Detection);

            // Loot varsa ekle
            if (!string.IsNullOrEmpty(data.LootId))
            {
                Item loot = CreateItem(data.LootId);
                enemy.SetLoot(loot);
            }

            return enemy;
        }
    }
}
