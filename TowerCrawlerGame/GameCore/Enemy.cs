using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class Enemy : Entity
    {
        //enemy stats
        public int SpeedSkill { get; set; }
        public int DetectionSkill { get; set; }
        public bool IsEdible { get; set; }

        public Enemy(String name, int maxHP, int speed, int detection ) : base( name, maxHP)
        {
            SpeedSkill = speed;
            DetectionSkill = detection;
            IsEdible = false;

        }
    }
}
