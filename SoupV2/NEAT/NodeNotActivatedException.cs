using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT
{
    class NodeNotActivatedException : Exception
    {
        public NodeNotActivatedException(int id) : base($"The given node was not activated: {id}")
        {

        }
    }
}
