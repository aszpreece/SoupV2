using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Exceptions
{
    class ComponentAlreadyExistsException : Exception
    {
        public ComponentAlreadyExistsException(Entity entity)
            : base("Component already exists on entity \"" + entity.Id + "\".")
        {

        }
    }
}
