using EntityComponentSystem;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class HealthDeathSystem : EntitySystem
    {
        private string _foodDefinition;
        public HealthDeathSystem(EntityPool pool, string foodDefinition) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(HealthComponent)))
        {
            _foodDefinition = foodDefinition;
        }

        public void Update()
        {
            List<Entity> toDestroy = new List<Entity>();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var health = entity.GetComponent<HealthComponent>();

                if (health.Health <= 0)
                {
                    toDestroy.Add(entity);
                    if (entity.TryGetComponent<EnergyComponent>(out EnergyComponent deadEnergy))
                    {
                        var food = Pool.AddEntityFromDefinition(_foodDefinition);
                        food.GetComponent<EnergyComponent>().Energy = deadEnergy.Energy;
                        var deadTransform = entity.GetComponent<TransformComponent>();
                        food.GetComponent<TransformComponent>().LocalPosition = deadTransform.WorldPosition;

                    }
                }
            }
            foreach(var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
