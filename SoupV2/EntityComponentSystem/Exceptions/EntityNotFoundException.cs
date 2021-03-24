﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Exceptions
{
    class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(EntityPool pool)
            : base($"Entity not found in pool \"{pool.Id}\".")
        {
            
        }
    }
}
