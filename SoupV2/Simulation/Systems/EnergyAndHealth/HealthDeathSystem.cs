using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using System;
using System.Collections.Generic;
using System.Text;
using SoupV2.Simulation.Events.DeathCause;
using Newtonsoft.Json;

namespace SoupV2.Simulation.Systems
{
    public class HealthDeathSystem : EntitySystem
    {
        public delegate void DeathEvent(DeathEventInfo info);

        public event DeathEvent OnDeath;

        private Simulation _simulation;

        private EnergyManager _energyManager;
        public HealthDeathSystem(EntityManager pool, Simulation simulation, EnergyManager energyManager) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(HealthComponent)))
        {
            _simulation = simulation;
            _energyManager = energyManager;
        }

        public void Update(uint tick, float gameSpeed)
        {
            List<Entity> toDestroy = new List<Entity>();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var health = entity.GetComponent<HealthComponent>();

                // If heath is below 0 then kill it
                if (health.Health <= 0)
                {
                    toDestroy.Add(entity);
                    // Check if we would be killing an entity with energy
                    if (entity.TryGetComponent<EnergyComponent>(out EnergyComponent deadEnergy))
                    {
                        deadEnergy.HandleDeath(_energyManager, Pool, _simulation.JsonSettings, entity.GetComponent<TransformComponent>().WorldPosition);
                    }
                    var loc = entity.GetComponent<TransformComponent>().WorldPosition;
                    OnDeath?.Invoke(new DeathEventInfo(loc, tick * gameSpeed, entity.Id, new HealthDeathCause()));
                }
            }
            foreach(var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
