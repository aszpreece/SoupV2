using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentSystem
{
    public abstract class EntitySystem
    {
        public EntityManager Pool { get; set; }
        public List<Entity> Compatible { get; set; }

        public Func<Entity, bool> CompatiblePredicate { get; }

        public EntitySystem(EntityManager pool, Func<Entity, bool> compatiblePredicate)
        {

            CompatiblePredicate = compatiblePredicate;

            Pool = pool;


            Pool.EntityComponentAdded += OnPoolEntityChanged;
            Pool.EntityComponentRemoved += OnPoolEntityChanged;

            Pool.EntityAdded += OnPoolEntityChanged;
            Pool.EntityRemoved += OnPoolEntityChanged;

            Compatible = GetCompatibleInPool();
        }

        protected virtual void OnPoolEntityChanged(EntityManager pool, Entity entity)
        {
            var index = Compatible.IndexOf(entity);
            // If we already have the entity, check if we are still compatible
            if (index >= 0)
            {
                if (!CompatiblePredicate(entity))
                {
                    SwapRemove.SwapRemoveList(Compatible, index);
                } 
            } else
            // If we do not have it but we are compatible, add it
            {
                if (CompatiblePredicate(entity))
                {
                    Compatible.Add(entity);
                }
            }
        }

  

        protected virtual List<Entity> GetCompatibleInPool()
        {
            return Pool.Entities.Where(CompatiblePredicate).ToList();
        }
    }
}
