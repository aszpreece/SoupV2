using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class MouthFoodCollisionSystem : EntitySystem
    {
        private List<Collision> _collisions;
        public MouthFoodCollisionSystem(EntityPool pool, List<Collision> collisionList): base(pool, (e) => e.HasComponent<MouthComponent>())
        {
            _collisions = collisionList;
        }

        public void Update(uint tick, float gameSpeed)
        {
            foreach (Entity mouth in Compatible)
            {
                mouth.GetComponent<MouthComponent>().Eating = 0.0f;
            }


            foreach (Collision c in _collisions)
            {
                // Eat the food!
                // Figure out which was the mouth
                Entity mouth;
                Entity food;
                if (c.E1.HasComponent<MouthComponent>())
                {
                    mouth = c.E1;
                    food = c.E2;
                } else
                {
                    mouth = c.E2;
                    food = c.E1;
                }

                var mouthComp = mouth.GetComponent<MouthComponent>();

                // This makes it so we can only eat one thing at a time.
                if (mouthComp.Eating > 0)
                {
                    continue;
                }

                var foodEnergy = food.GetComponent<EnergyComponent>();
                mouthComp.Eating = 1.0f;
                //Find energy in mouth family tree
                var mouthEnergy = mouth.RootEntity.GetComponent<EnergyComponent>();

                float taken = foodEnergy.ChargeEnergy((float)(mouthComp.EnergyPerSecond * gameSpeed));
                mouthEnergy.DepositEnergy(taken);

            }
        }

    }
}

