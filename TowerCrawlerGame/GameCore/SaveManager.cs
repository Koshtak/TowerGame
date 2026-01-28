using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameCore
{
    public class SaveManager
    {
        private static string _filePath = "Global_Save.json";
        public static GlobalGameState CurrentState { get; private set; }
        public static void LoadGame()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                CurrentState = JsonSerializer.Deserialize<GlobalGameState>(json);
            }
            else
            {
                //save yoksa
                CurrentState = new GlobalGameState();
            }
        }
        public static void SaveGame()
        {
            string json = JsonSerializer.Serialize(CurrentState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        public static void WipeReality()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            CurrentState = new GlobalGameState(); // Sıfırla
        }
    }
}
