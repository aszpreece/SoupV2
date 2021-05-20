using Microsoft.Xna.Framework;
using SoupV2.Simulation.Events.DeathCause;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public class DeathEventInfo : AbstractEventInfo
    {
        public DeathEventInfo(Vector2 location, float timestamp, int deadEntityId, string deadEntityTag, AbstractDeathCause cause): base(timestamp, location)
        {
            DeadEntity = deadEntityId;
            DeadEntityTag = deadEntityTag;
            Cause = cause;
        }
        public int DeadEntity { get; set; }
        public string DeadEntityTag { get; }
        public AbstractDeathCause Cause { get; set; }

        public override string ToString()
        {
            return $"Entity: {DeadEntity}, Cause: {Cause.ToString()}";
        }
    }
}
