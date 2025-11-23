using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore;

namespace GameUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player hero = new Player("Marlo", 100, 20,50 );
            Console.WriteLine($"oyun başladı: {hero.Name}");
            Console.WriteLine($"Can:{hero.CurrentHP}/{hero.MaxHP}");
            Console.WriteLine($"Gizlilik:{hero.StealthSkill}");

            Enemy Goblin = new Enemy("japanese goblin", 100, 10, 20);
            Console.WriteLine($"\nDüşman Gördün: {Goblin.Name}");
            bool kactikMi = hero.TryFlee(Goblin);
            if (kactikMi)
                Console.WriteLine("Kaçtın!");
            else
                Console.WriteLine("Yakalandın!");

            Console.ReadLine();
        }
    }
}
