using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Player : Entity
    {
        //stats
        public int StealthSkill {  get; set; }
        public int SpeedSkill { get; set; }
        public int Hunger { get; set; }
        public int MaxHunger { get; set; } = 100;
        public Player(string name, int maxHP, int startStealth, int startSpeed): base(name, maxHP) //hp 0-100, stealth 0-100, speed 0-100
        {
            StealthSkill = startStealth;
            SpeedSkill = startSpeed;
            Hunger = 0;
        }

        public bool TryFlee(Enemy targetEnemy)
         {
            // int StealthBeforeFlee=StealthSkill; //düşmandan kaçtıktan sonra stealth seviyesi normale dönmeli.
            if(this.SpeedSkill> targetEnemy.SpeedSkill) return true;
            else 
            {
                this.StealthSkill--;
                return false;
            }
        }
        
         
    }
}
