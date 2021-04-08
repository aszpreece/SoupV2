using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public abstract class AbstractEventInfo
    {
        public float TimeStamp { get; set; }
        
        public Vector2 Location { get; set; }
    }
}
