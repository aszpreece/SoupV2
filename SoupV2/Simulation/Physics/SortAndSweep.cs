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
        private List<Entity> _toRemove = new List<Entity>();
        private Func<Entity, Entity, bool> _shouldDetectCollision;

        public List<Collision> Collisions { get; set; } = new List<Collision>();

        public SortAndSweep(EntityPool pool, Func<Entity, bool> compatible, Func<Entity, Entity, bool> shouldResolveCollision) : base(pool, compatible)
        {
            _shouldDetectCollision = shouldResolveCollision;
        }

        /// <summary>
        /// Determines which entities need to be checked for collision
        /// </summary>
        public void Sweep()
        {

        }

        private void RemoveDeletedEntities()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                bool removed = false;
                do
                {
                    for (int j = 0; j < _toRemove.Count; j++)
                    {
                        if (Compatible[i].Id == _toRemove[j].Id)
                        {
                            // Swap removed 
                            SwapRemove.SwapRemoveList(Compatible, i);
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
            RemoveDeletedEntities();

            // Sort all entities by their minimum X extent
            Compatible.Sort((e1, e2) => 
                e1.GetComponent<CircleColliderComponent>().MinX()
                .CompareTo(
                    e2.GetComponent<CircleColliderComponent>().MinX()
                )
            );

            // Reset collision info
            for (int i = 0; i < Compatible.Count; i++)
            {
                CircleColliderComponent collider = Compatible[i].GetComponent<CircleColliderComponent>();
                TransformComponent transform = Compatible[i].GetComponent<TransformComponent>();

                collider.UpdatePosition(transform);
                collider.ClearCollisions();
            }

            Collisions.Clear();

            for (int i = 0; i < Compatible.Count; i++)
            {
                CircleColliderComponent collider = Compatible[i].GetComponent<CircleColliderComponent>();
                float maxX = collider.MaxX();
                for (int j = i + 1; j < Compatible.Count; j++)
                {
                    CircleColliderComponent otherCollider = Compatible[j].GetComponent<CircleColliderComponent>();
   
                    // If the other collider has a minimum x span that is greater than our max x,
                    // Then we no longer need to keep checking. This collider and all other colliders after it cannot be intersecting this collider.

                    if (otherCollider.MinX() > maxX)
                    {
                        break;
                    }



                    if (collider.Intersects(otherCollider)) {

                        if (!_shouldDetectCollision(Compatible[i], Compatible[j]))
                        {
                            continue;
                        }
                        //Calculatye the collison normal

                        var collNormal = otherCollider.Position - collider.Position;

                        if (collNormal != Vector2.Zero)
                        {
                            collNormal.Normalize();
                        }
                        else
                        {
                            collNormal = Vector2.One;
                        }

                        Collisions.Add(new Collision() {
                            Normal = collNormal,
                            E1 = Compatible[i],
                            E2 =Compatible[j]
                        });
                    }

                }
            }
        }
    }
}
