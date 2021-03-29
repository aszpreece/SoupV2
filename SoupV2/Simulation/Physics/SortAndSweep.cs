using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Physics
{
    internal class SortAndSweep : EntitySystem
    {
        // sorted list of physics objects by their min value on x axis
        private List<Entity> _physicsEntities;
        private List<Entity> _toRemove;

        public SortAndSweep(EntityPool pool): base(pool, typeof(CircleColliderComponent), typeof(RigidBodyComponent))
        {
            _physicsEntities = new List<Entity>();
            _toRemove = new List<Entity>();
        }

        /// <summary>
        /// Determines which entities need to be checked for collision
        /// </summary>
        public void Sweep()
        {

        }

        private void RemoveDeletedEntities()
        {
            for (int i = 0; i < _physicsEntities.Count; i++)
            {
                bool removed = false;
                do
                {
                    for (int j = 0; j < _toRemove.Count; j++)
                    {
                        if (_physicsEntities[i].Id == _toRemove[j].Id)
                        {
                            // Swap removed 
                            SwapRemove.SwapRemoveList(_physicsEntities, i);
                            SwapRemove.SwapRemoveList(_toRemove, j);
                            removed = true;
                            break;
                        }
                    }
                    if (removed && _toRemove.Count == 0)
                    {
                        // Can do a simple return instead of carrying on looping through the list.
                        return;
                    }

                } while (removed);

                // If for some reason we still have entities to remove,
                // then we can just clear the list as they are not being tracked by the physics system anyway
                _toRemove.Clear();

            }
            
        }
        public void GetCollisions()
        {
            _physicsEntities = Compatible;
            RemoveDeletedEntities();

            // Sort all entities by their minimum X extent
            _physicsEntities.Sort((e1, e2) => 
                e1.GetComponent<CircleColliderComponent>().MinX()
                .CompareTo(
                    e2.GetComponent<CircleColliderComponent>().MinX()
                )
            );

            // Reset collision info
            for (int i = 0; i < _physicsEntities.Count; i++)
            {
                CircleColliderComponent collider = _physicsEntities[i].GetComponent<CircleColliderComponent>();
                collider.ClearCollisions();
            }

            for (int i = 0; i < _physicsEntities.Count; i++)
            {
                CircleColliderComponent collider = _physicsEntities[i].GetComponent<CircleColliderComponent>();
                float maxX = collider.MaxX();
                for (int j = i + 1; j < _physicsEntities.Count; j++)
                {
                    CircleColliderComponent otherCollider =_physicsEntities[j].GetComponent<CircleColliderComponent>();
   
                    // If the other collider has a minimum x span that is greater than our max x,
                    // Then we no longer need to keep checking. This collider and all other colliders after it cannot be intersecting this collider.

                    if (otherCollider.MinX() > maxX)
                    {
                        break;
                    }

                    if (collider.Intersects(otherCollider)) {
                        //collider.AddCollision(otherCollider);
                        //otherCollider.AddCollision(collider);
                        ResolveCollision(_physicsEntities[i], collider, _physicsEntities[j], otherCollider);
                    }

                }
            }
        }

        private void ResolveCollision(Entity e1, CircleColliderComponent e1Collider, Entity e2, CircleColliderComponent e2Collider)
        {
            var vel1 = e1.GetComponent<VelocityComponent>();
            var vel2 = e2.GetComponent<VelocityComponent>();
            var rb1 = e1.GetComponent<RigidBodyComponent>();
            var rb2 = e2.GetComponent<RigidBodyComponent>();

            var collNormal = e2Collider.Position - e1Collider.Position;
           
            if (collNormal != Vector2.Zero)
            {
                collNormal.Normalize();
            } else
            {
                return;
            }

            var relativeVelocity = vel2.Velocity - vel1.Velocity;

            // Project the relative velocity of both of the objects onto the collision normal
           
            float velAlongNormal = Vector2.Dot(relativeVelocity, collNormal);
            
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
            var impluse = collNormal * impulseScalar;
            rb1.ApplyImpulse(-impluse);
            rb2.ApplyImpulse(impluse);
        }
    }
}
