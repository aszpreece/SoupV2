using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Statistics.StatReporting.ReportInfoObjects
{
    class SimulationColourInfo : AbstractSimulationInfo
    {
        public  SimulationColourInfo(float r, float g, float b, int species, float timestamp)
        {
            R = r;
            G = g;
            B = b;
            Species = species;
            TimeStamp = timestamp;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        
        public int Species { get; set; }
    }
}
