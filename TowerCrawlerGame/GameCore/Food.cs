using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Food : Item
    {
        public int RestoreAmount {  get; set; }
        public Action<Player> OnConsume { get; set; }
        public Food(string id, string name, int weight, int restore, Action<Player> effect = null): base(id, name, weight)
        {
            RestoreAmount = restore;
            OnConsume = effect;


        }
    }
}
