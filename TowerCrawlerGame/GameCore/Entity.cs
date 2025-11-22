using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    internal class Entity
    {
        public string Name { get; set; }
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set}

        public Entity(string Name, int MaxHp )
        {
            Name = name;
            MaxHp = maxHp;
            CurrentHP = maxHp;
        }

        //functions
        public bool IsDead => CurrentHP <= 0

        public virtual void TakeDamage(int amount)
        {
            CurrentHP -=amount;
            if(CurrentHP < 0) CurrentHP = 0;
        }
        public virtual void Heal(int amount)
        {
            if (isdead) return;
            CurrentHP += amount;
            if(CurrentHP>MaxHP) CurrentHP = MaxHP;
        }
    
    
    
    }
}
