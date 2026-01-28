using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class ItemData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public List<string> Moves { get; set; }
    }

    public class EnemyData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Speed { get; set; }
        public int Detection { get; set; }
        public string LootId { get; set; }
    }
}
