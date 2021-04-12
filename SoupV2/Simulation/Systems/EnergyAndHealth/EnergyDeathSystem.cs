using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using SoupV2.Simulation.Events.DeathCause;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class EnergyDeathSystem : EntitySystem
    {
        public delegate void DeathEvent(DeathEventInfo info);

        public event DeathEvent OnDeath;
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
                    OnDeath?.Invoke(new DeathEventInfo(entity.Id, new EnergyDeathCause()));
                }
            }
            foreach(var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
