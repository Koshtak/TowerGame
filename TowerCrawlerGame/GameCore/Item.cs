using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Item
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public Item(string id, string name, int weight) 
        {
            Id = id;
            Name = name;
            Weight = weight;
        }
    }
}
