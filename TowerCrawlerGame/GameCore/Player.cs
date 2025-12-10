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
        private int _baseSpeed;

        
        public int SpeedSkill 
        {
            get
            {
                int totalWeight = Inventory.Sum(i => i.Weight);
                int finalSpeed = _baseSpeed - totalWeight - SpeedPenalty;
                return finalSpeed < 0 ? 0 : finalSpeed;
            }
            
        }
        public int SpeedPenalty { get; set; }
        public int StealthSkill { get; set; }
        public int Hunger { get; set; }
        public int MaxHunger { get; set; } = 100;
        public List<Item> Inventory { get; set; }
        public int MaxInventorySize { get; set; } = 5;
        public Weapon EquippedWeapon { get; set; }

        public Player(string name, int maxHP, int startStealth, int startSpeed): base(name, maxHP) //hp 0-100, stealth 0-100, speed 0-100
        {
            StealthSkill = startStealth;
            _baseSpeed = startSpeed;
            Inventory= new List<Item>();
            Hunger = 0;
            SpeedPenalty = 0;
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
        
        public bool AddToInventory(Item item)
        {
            if (Inventory.Count >= MaxInventorySize)return false;
            Inventory.Add(item);
            return true;
        }

        public void EatItem(Food food)
        {
            if (Inventory.Contains(food))
            {
                Heal(food.RestoreAmount);
                food.OnConsume?.Invoke(this);
                Inventory.Remove(food);
                 
            }
        }
        public void ResetDebuffs()
        {
            SpeedPenalty = 0;
            //diğer debufflar
        }

    }
}
