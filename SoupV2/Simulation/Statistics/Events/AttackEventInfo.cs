using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
 
namespace SoupV2.Simulation.Events
{
    public class AttackEventInfo : AbstractEventInfo
    {
        public int AttackerId { get; set; }
        public int DefenderId { get; set; }
        public float Damage { get; set; }


        public AttackEventInfo(Vector2 location, uint tick, int attackerId, int defenderId, float damage): base(tick, location)
        {
            AttackerId = attackerId;
            DefenderId = defenderId;
            Damage = damage;
            TimeStamp = tick;
        }

        public override string ToString()
        {
            return $"Attacker: {AttackerId}, Defender: {DefenderId}, Damage: {Damage}";
        }
    }
}
