using System;
using System.Collections.Generic;
using System.Text;

using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.NEAT;
using SoupV2.NEAT.Genes;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using SoupV2.Simulation.Physics;

namespace SoupV2.Simulation.Systems.Abilities
{


    class WeaponSystem : EntitySystem
    {

        private List<Collision> _collisions;
        private EnergyManager _energyManager;

        public delegate void AttackEvent(AttackEventInfo e);
        public event AttackEvent OnAttack;

        public WeaponSystem(EntityPool pool, List<Collision> collisionList, EnergyManager energyManager) 
            : base(pool, (e) => e.HasComponent<WeaponComponent>() && e.RootEntity.HasComponent<EnergyComponent>())
        {
            _collisions = collisionList;
            _energyManager = energyManager;
        }

        public void Update(GameTime gameTime, float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {

                float timeElapsed = (float)(gameTime.ElapsedGameTime.TotalSeconds * gameSpeed);
                var entity = Compatible[i];
                var weapon = entity.GetComponent<WeaponComponent>();

                //Reduce cooldown timers if necessary
                if (weapon.CooldownLeftSeconds > 0)
                {
                    weapon.CooldownLeftSeconds -= timeElapsed;
                }
                if (weapon.AttackTimeLeft > 0)
                {
                    if (entity.TryGetComponent<GraphicsComponent>(out GraphicsComponent graphics))
                    {
                        graphics.Color = Color.Red;
                    }
                    
                    weapon.AttackTimeLeft -= timeElapsed;
                } else
                {
                    if (entity.TryGetComponent<GraphicsComponent>(out GraphicsComponent graphics))
                    {
                        graphics.Color = Color.White;
                    }

                    // Remember to set the activation bool to false
                    weapon.Active = 0f;
                }

                // Only attack if activation is over threshold and the cooldowns have expired.
                if (weapon.Activation > weapon.ActivateThreshold && weapon.CooldownLeftSeconds <= 0 && weapon.AttackTimeLeft <= 0)
                {
                   
                    var energy = entity.RootEntity.GetComponent<EnergyComponent>();

                    // check if we have the energy
                    if (!energy.CanAfford(weapon.AttackCost))
                    {
                        continue;
                    }

                    // charge the entity energy for the attack
                    float charged = energy.ChargeEnergy(weapon.AttackCost);
                    _energyManager.DepositEnergy(charged);

                    weapon.Active = 1.0f;

                    // set up all the timers
                    weapon.CooldownLeftSeconds = weapon.CooldownTimeSeconds;
                    weapon.AttackTimeLeft = weapon.AttackTimeSeconds;
                
                }
            }
            foreach (var c in _collisions)
            {
                // Damage the health entity
                // Figure out which was the weapon and which was the health entity
                Entity health;
                Entity weapon;
                if (c.E1.HasComponent<HealthComponent>())
                {
                    health = c.E1;
                    weapon = c.E2;
                }
                else
                {
                    health = c.E2;
                    weapon = c.E1;
                }

                

                var weaponComp = weapon.GetComponent<WeaponComponent>();
                if (weaponComp.Active <= 0)
                {
                    continue;
                }

                var healthComp = health.GetComponent<HealthComponent>();

                float damage = (float)(weaponComp.Damage);
                weaponComp.Active = 0;

                healthComp.Health -= damage;
            }
        }

    }
}


