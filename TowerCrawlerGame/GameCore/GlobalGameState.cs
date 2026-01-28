using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class GlobalGameState
    {
        public bool IsRealityEstablished { get; set; } = false;
        public int MemoryFragments { get; set; } = 0;
        public int CrystalCapacity { get; set; } = 100;
        public List<string> UnlockedCharacters { get; set; } = new List<string>();
        public List<EchoData> Echoes { get; set; } = new List<EchoData>();
        public GlobalGameState()
        {
            UnlockedCharacters.Add("toyo");
        }
    }

    public class EchoData
    {
        public int FloorLevel { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int EchoCharName { get; set; }
        public List<ItemData> DroppedItems { get; set; }    

    }
}
