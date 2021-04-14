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

        public AbstractEventInfo(uint tick, Vector2 location)
        {
            Location = location;
            TimeStamp = tick;
        }

        public abstract override string ToString();

        public virtual string[] ToInfo()
        {
            return new[] {
                TimeStamp.ToString(),
                this.GetType().Name,
                Location.ToString(),
                this.ToString()
            };
        }
    }
}
