using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation
{
    public class CritterTypeSetting
    {
        public string DefinitionName { get; set; }
        public uint InitialCount { get; set; }
        public uint MinimumCount { get; set; }

        public override string ToString()
        {
            return string.Format("Definition Name={0}, Initial Count={1}, Minimum Count={2}", DefinitionName, InitialCount, MinimumCount);
        }

    }
}
