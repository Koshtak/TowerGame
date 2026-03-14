using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Weapon : Item
    {
        string Name { get; set; }
        public List<CombatMove> Moves { get; set; }
        public Weapon(string id, string name, int weight = 3) : base(id, name, weight)
        {
            Name = name;
            Moves = new List<CombatMove>();
        }
        public void AddMove(CombatMove move)
        {
            Moves.Add(move);
        }
    }
}
