using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem.Exceptions
{
    public class EntityCachedException : Exception
    {
        public EntityCachedException(int entityId, string message) : 
            base($"Tried to access components of entity ${entityId} but is was cached. Extra info: ${message}")
        {

        }
    }
}
