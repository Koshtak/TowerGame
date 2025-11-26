using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class CombatMove
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        //public string Description { get; set; }

        public Action<Player,Enemy> OnExecute { get; set; }
        public CombatMove(string name, int damage,Action<Player,Enemy> effect=null)
        {
            Name = Name;
            Damage = damage;
            OnExecute = effect;
        }
    }
}
