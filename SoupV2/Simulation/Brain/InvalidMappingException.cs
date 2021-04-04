using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Brain
{
    class InvalidMappingException : Exception
    {
        public InvalidMappingException(string mapping) : base(mapping) { }
    }
}
