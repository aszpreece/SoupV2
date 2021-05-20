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

        public AbstractEventInfo(float timestamp, Vector2 location)
        {
            Location = location;
            TimeStamp = timestamp;
        }

        public abstract override string ToString();

        /// <summary>
        /// Converts the event into an array that can be displayed on the UI.
        /// </summary>
        /// <returns></returns>
        public virtual string[] ToInfo()
        {
            return new[] {
                // Time stamp
                TimeStamp.ToString(),
                //Event type name
                this.GetType().Name,
                // Locations
                Location.ToString(),
                // Any extra info
                this.ToString()
            };
        }
    }
}
