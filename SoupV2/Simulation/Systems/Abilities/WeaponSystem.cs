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


    public class WeaponSystem : EntitySystem
    {

        private List<Collision> _collisions;
        private EnergyManager _energyManager;

        public delegate void AttackEvent(AttackEventInfo e);
        public event AttackEvent OnAttack;

        public WeaponSystem(EntityManager pool, List<Collision> collisionList, EnergyManager energyManager) 
            : base(pool, (e) => e.HasComponent<WeaponComponent>())
        {
            _collisions = collisionList;
            _energyManager = energyManager;
        }

        public void Update(uint tick, float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {

                float timeElapsed = gameSpeed;

                var entity = Compatible[i];
                var weapon = entity.GetComponent<WeaponComponent>();

                // reset hit marker
                weapon.Hit = 0.0f;
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
                    if (entity.RootEntity.HasComponent<EnergyComponent>())
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
                    }



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

                // Check not attached to same entity
                if (health.RootEntity == weapon.RootEntity)
                {
                    continue;
                }
                

                var weaponComp = weapon.GetComponent<WeaponComponent>();
                if (weaponComp.Active <= 0)
                {
                    continue;
                }

                var healthComp = health.GetComponent<HealthComponent>();

                float damage = (float)(weaponComp.Damage);
                weaponComp.Active = 0;

                var trans = weapon.GetComponent<TransformComponent>();

                healthComp.Health -= damage;
                weaponComp.Hit = 1.0f;

                if (weaponComp.SiphonEnergy && health.HasComponent<EnergyComponent>() && weapon.RootEntity.HasComponent<EnergyComponent>())
                {
                    var attackerEnergy = weapon.RootEntity.GetComponent<EnergyComponent>();
                    var victimEnergy = health.GetComponent<EnergyComponent>();
                    float taken = victimEnergy.ChargeEnergy(weaponComp.SiphonAmount);
                    attackerEnergy.DepositEnergy(taken);
                }

                OnAttack?.Invoke(new AttackEventInfo(trans.WorldPosition, tick * gameSpeed, weapon.Id, weapon.Tag, health.Id, health.Tag, damage));
            }
        }

    }
}


