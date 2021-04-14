using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Physics
{
    public class RigidBodyCollisionSystem
    {
        private List<Collision> _collisions;
        public RigidBodyCollisionSystem(List<Collision> collisionList)
        {
            _collisions = collisionList;
        }
        public void Update()
        {
            Parallel.ForEach(_collisions, (c) => ResolveCollision(c.E1, c.E2, c.Normal));
        }

        private void ResolveCollision(Entity e1, Entity e2, Vector2 collisionNormal)
        {
            var vel1 = e1.GetComponent<VelocityComponent>();
            var vel2 = e2.GetComponent<VelocityComponent>();
            var rb1 = e1.GetComponent<RigidBodyComponent>();
            var rb2 = e2.GetComponent<RigidBodyComponent>();

            var relativeVelocity = vel2.Velocity - vel1.Velocity;

            // Project the relative velocity of both of the objects onto the collision normal

            float velAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

            // If the vectors are moving away from each other in term of the normal of the collission, do not collide
            // This prevents multiple detected collisions sending the object rocketing away
            if (velAlongNormal > 0)
            {
                return;
            }

            // Calculate impluse needed to seperate

            float restitution = Math.Min(rb1.Restitution, rb2.Restitution);
            float impulseScalar = -(1 + restitution) * velAlongNormal;
            impulseScalar /= rb1.InvMass + rb2.InvMass;
            var impluse = collisionNormal * impulseScalar;
            rb1.ApplyImpulse(-impluse);
            rb2.ApplyImpulse(impluse);
        }
    }
}
