using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Exceptions
{
    class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(EntityManager pool)
            : base($"Two entities in pool {pool.Id} shared the same tag.")
        {

        }
    }
}
