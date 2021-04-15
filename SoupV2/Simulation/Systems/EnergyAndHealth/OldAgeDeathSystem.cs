using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using SoupV2.Simulation.Events.DeathCause;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class OldAgeDeathSystem : EntitySystem
    {
        public delegate void DeathEvent(DeathEventInfo info);

        public event DeathEvent OnDeath;

        private bool _oldAgeEnabled;
        
        private float _oldAgeMultiplier;

        private Simulation _simulation;
        private EnergyManager _energyManager;

        public OldAgeDeathSystem(EntityManager pool, Simulation simulation, EnergyManager energyManager, bool oldAgeEnabled, float oldAgeMultiplier) : base(pool, (e) => e.HasComponents(typeof(OldAgeComponent)))
        {
            _oldAgeEnabled = oldAgeEnabled;
            _oldAgeMultiplier = oldAgeMultiplier;
            _simulation = simulation;
            _energyManager = energyManager;
        }

        public void Update(uint tick, float _gameSpeed)
        {
        
            List<Entity> toDestroy = new List<Entity>();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var ageComponent = entity.GetComponent<OldAgeComponent>();
                ageComponent.CurrentAge += _gameSpeed * _oldAgeMultiplier;

                if (ageComponent.CurrentAge >= ageComponent.MaxAge && _oldAgeEnabled)
                {
                    toDestroy.Add(entity);
                    // Check if we would be killing an entity with energy


                    if (entity.TryGetComponent<EnergyComponent>(out EnergyComponent deadEnergy))
                    {
                        deadEnergy.HandleDeath(_energyManager, Pool, _simulation.JsonSettings, entity.GetComponent<TransformComponent>().WorldPosition);
                    }
                    var transform = entity.GetComponent<TransformComponent>();

                    OnDeath?.Invoke(new DeathEventInfo(transform.WorldPosition, tick, entity.Id, new OldAgeDeathCause())
                    {
                        Location = entity.GetComponent<TransformComponent>().WorldPosition
                    });
                }
            }
            foreach (var e in toDestroy)
            {
                Pool.DestroyEntity(e);
            }
        }
    }
}
