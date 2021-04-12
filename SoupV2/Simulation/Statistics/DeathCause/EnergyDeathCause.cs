using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events.DeathCause
{
    public class EnergyDeathCause : AbstractDeathCause
    {
        public string Cause { get; } = "Energy";
    }
}
