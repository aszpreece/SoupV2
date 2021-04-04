using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class MouthFoodCollisionSystem
    {
        private List<Collision> _collisions;
        public MouthFoodCollisionSystem(List<Collision> collisionList)
        {
            _collisions = collisionList;
        }

        public void Update(GameTime gameTime, float gameSpeed)
        {
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

                var foodEnergy = food.GetComponent<EnergyComponent>();
                var mouthComp = mouth.GetComponent<MouthComponent>();
                //Find energy in mouth family tree
                var mouthEnergy = mouth.RootEntity.GetComponent<EnergyComponent>();

                float taken = foodEnergy.ChargeEnergy((float)(mouthComp.EnergyPerSecond * gameTime.ElapsedGameTime.TotalSeconds * gameSpeed));
                mouthEnergy.DepositEnergy(taken);

            }
        }

    }
}

