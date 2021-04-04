using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentSystem
{
    public abstract class EntitySystem
    {
        public EntityPool Pool { get; set; }
        public List<Entity> Compatible { get; set; }

        public Func<Entity, bool> CompatiblePredicate { get; }

        public EntitySystem(EntityPool pool, Func<Entity, bool> compatiblePredicate)
        {

            CompatiblePredicate = compatiblePredicate;

            Pool = pool;

            Compatible = GetCompatibleInPool();

            Pool.EntityComponentAdded += OnPoolEntityChanged;
            Pool.EntityComponentRemoved += OnPoolEntityChanged;

            Pool.EntityAdded += OnPoolEntityChanged;
            Pool.EntityRemoved += OnPoolEntityChanged;
        }

        protected virtual void OnPoolEntityChanged(EntityPool pool, Entity entity)
        {
            Pool = pool;

            Compatible = GetCompatibleInPool();
        }

  

        protected virtual List<Entity> GetCompatibleInPool()
        {
            return Pool.Entities.Where(CompatiblePredicate).ToList();
        }
    }
}
