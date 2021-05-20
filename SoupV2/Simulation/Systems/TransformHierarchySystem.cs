using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class TransformHierarchySystem : EntitySystem
    {
        private readonly AdjacencyGrid _grid;

        public TransformHierarchySystem(EntityManager pool, AdjacencyGrid grid) : base(pool, 
            ent => ent.HasComponents(typeof(TransformComponent)) 
            && ent.Parent is null
        )
        {
            _grid = grid;
            
        }

        public void Update()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var root = Compatible[i];
                EntityTreeRecalculateTransform(root, Vector2.Zero, 0.0f, 0.0f, false);
            }

        }

        /// <summary>
        /// Recursivley update the transforms of each entity in the tree.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parentWorldPos"></param>
        /// <param name="parentWorldRot"></param>
        public void EntityTreeRecalculateTransform(Entity entity, Vector2 parentWorldPos, float parentWorldRot, float parentDepth, bool foundDirty)
        {
            if (!entity.IsActive())
            {
                return;
            }
            var transform = entity.GetComponent<TransformComponent>();
            if (foundDirty || transform.Dirty)
            {
                Matrix rot;
                if (!entity.IsRoot)
                {
                    rot = Matrix.CreateRotationZ(parentWorldRot + transform.LocalRotation.Theta);
                } else
                {
                    rot = Matrix.CreateRotationZ(parentWorldRot);
                }

                transform.WorldPosition = Vector2.Transform(transform.LocalPosition, rot) + parentWorldPos;
                transform.WorldRotation = transform.LocalRotation + parentWorldRot;
                transform.WorldDepth = transform.LocalDepth + parentDepth;
                transform.Dirty = false;
                foundDirty = true;

                _grid.PlaceIntoGrid(entity);
            }

            foreach (Entity child in entity.Children.Values)
            {
                EntityTreeRecalculateTransform(child, transform.WorldPosition, transform.WorldRotation.Theta, transform.WorldDepth, foundDirty || transform.Dirty);
            }

        }

        protected override void OnPoolEntityChanged(EntityManager pool, Entity entity)
        {
            var index = Compatible.IndexOf(entity);
            // If we already have the entity, check if we are still compatible
            if (index >= 0)
            {
                if (!CompatiblePredicate(entity))
                {
                    SwapRemove.SwapRemoveList(Compatible, index);
                    // Clean up grid
                    _grid.RemoveFromGrid(entity);
                }
            }
            else
            // If we do not have it but we are compatible, add it
            {
                if (CompatiblePredicate(entity))
                {
                    Compatible.Add(entity);
                }
            }
        }

    }
}
