using SoupV2.Simulation.Events.DeathCause;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public class DeathEventInfo : AbstractEventInfo
    {
        public DeathEventInfo(int deadEntityId, AbstractDeathCause cause)
        {
            DeadEntity = deadEntityId;
            Cause = cause;
        }
        public int DeadEntity { get; set; }

        public AbstractDeathCause Cause { get; set; }
    }
}
