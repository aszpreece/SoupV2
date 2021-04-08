using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    class BirthEventInfo : AbstractEventInfo
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public AbstractGenotype ChildGenotype { get; set; }
    }
}
