using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events.DeathCause
{
    public class HealthDeathCause : AbstractDeathCause
    {
        public string Cause { get; } = "Health";
        public int LastDamagedByEntityId { get; set; }
    }
}
