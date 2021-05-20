using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
 
namespace SoupV2.Simulation.Events
{
    public class AttackEventInfo : AbstractEventInfo
    {
        public int AttackerId { get; set; }
        public string AttackerTag { get; }
        public int DefenderId { get; set; }
        public string DefenderTag { get; }
        public float Damage { get; set; }


        public AttackEventInfo(Vector2 location, float tick, int attackerId, string attackerTag, int defenderId, string defenderTag, float damage): base(tick, location)
        {
            AttackerId = attackerId;
            AttackerTag = attackerTag;
            DefenderId = defenderId;
            DefenderTag = defenderTag;
            Damage = damage;
            TimeStamp = tick;
        }

        public override string ToString()
        {
            return $"Attacker: {AttackerId}, Defender: {DefenderId}, Damage: {Damage}";
        }
    }
}
