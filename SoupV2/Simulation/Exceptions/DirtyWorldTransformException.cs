using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Exceptions
{
    class DirtyWorldTransformException : Exception
    {
        public DirtyWorldTransformException(Entity entity)
            : base($"Tried to fetch a dirty world transform \"{entity.Id}\".")
        {

        }
    }
}
