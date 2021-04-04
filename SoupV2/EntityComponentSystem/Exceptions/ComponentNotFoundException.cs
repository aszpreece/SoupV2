using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Exceptions
{
    class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(Entity occuredIn, Type t)
            : base($"Component {t.Name} not found in Entity \"{occuredIn.Id}\".")
        {

        }

        public ComponentNotFoundException(Entity occuredIn, string t)
    : base($"Component {t} not found in Entity \"{occuredIn.Id}\".")
        {

        }
    }
}
