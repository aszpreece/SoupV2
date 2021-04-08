using SoupV2.Simulation.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2
{
    interface DataOutput
    {
        public void Initialize();
        public void WriteEvent(AbstractEventInfo info);
    }
}
