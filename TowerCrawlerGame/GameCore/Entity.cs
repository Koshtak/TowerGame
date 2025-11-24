using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public abstract class Entity
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set; }

        public Entity(string name, int maxHP )
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
        }

        //functions
        public bool IsDead => CurrentHP <= 0;

        public virtual void TakeDamage(int amount)
        {
            CurrentHP -=amount;
            if(CurrentHP < 0) CurrentHP = 0;
        }
        public virtual void Heal(int amount)
        {
            if (IsDead) return;
            CurrentHP += amount;
            if(CurrentHP>MaxHP) CurrentHP = MaxHP;
        }
    
    
    
    }
}
