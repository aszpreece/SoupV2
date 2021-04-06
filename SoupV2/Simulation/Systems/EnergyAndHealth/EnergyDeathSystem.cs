using EntityComponentSystem;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class EnergyDeathSystem : EntitySystem
    {
        public EnergyDeathSystem(EntityPool pool) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(EnergyComponent)))
        {

        }

        public void Update()
        {
            List<Entity> toDestroy = new List<Entity>();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var energy = entity.GetComponent<EnergyComponent>();

                if (energy.Energy <= 0)
                {
                    toDestroy.Add(entity);
                }
            }
            foreach(var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
