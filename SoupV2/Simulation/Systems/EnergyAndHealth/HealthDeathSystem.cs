using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using System;
using System.Collections.Generic;
using System.Text;
using SoupV2.Simulation.Events.DeathCause;

namespace SoupV2.Simulation.Systems
{
    public class HealthDeathSystem : EntitySystem
    {
        private string _foodDefinition;

        public delegate void DeathEvent(DeathEventInfo info);

        public event DeathEvent OnDeath;
        public HealthDeathSystem(EntityPool pool, string foodDefinition) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(HealthComponent)))
        {
            _foodDefinition = foodDefinition;
        }

        public void Update(uint tick)
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
                        var food = Pool.AddEntityFromDefinition(_foodDefinition);
                        food.GetComponent<EnergyComponent>().Energy = deadEnergy.Energy;
                        var deadTransform = entity.GetComponent<TransformComponent>();
                        food.GetComponent<TransformComponent>().LocalPosition = deadTransform.WorldPosition;
                    }
                    var loc = entity.GetComponent<TransformComponent>().WorldPosition;
                    OnDeath?.Invoke(new DeathEventInfo(loc, tick, entity.Id, new HealthDeathCause()));
                }
            }
            foreach(var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
