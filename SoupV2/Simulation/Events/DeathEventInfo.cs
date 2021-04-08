using SoupV2.Simulation.Events.DeathCause;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    class DeathEventInfo : AbstractEventInfo
    {
        public int DeadEntity { get; set; }

        public AbstractCause Cause { get; set; }
    }
}
