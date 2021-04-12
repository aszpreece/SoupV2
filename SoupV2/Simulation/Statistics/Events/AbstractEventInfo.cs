using Microsoft.Xna.Framework;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public abstract class AbstractEventInfo : AbstractSimulationInfo
    {        
        public Vector2 Location { get; set; }
    }
}
