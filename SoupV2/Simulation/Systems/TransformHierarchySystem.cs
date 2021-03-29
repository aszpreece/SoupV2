using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    class TransformHierarchySystem : EntitySystem
    {
        private List<TransformComponent> _modifiedTransforms;
        public TransformHierarchySystem(EntityPool pool) : base(pool, typeof(TransformComponent))
        {
            _modifiedTransforms = new List<TransformComponent>();
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
            var transform = entity.GetComponent<TransformComponent>();

            if (foundDirty || transform.Dirty)
            {
                transform.WorldPosition = transform.LocalPosition + parentWorldPos;
                transform.WorldRotation = transform.LocalRotation + parentWorldRot;
                transform.WorldDepth = transform.LocalDepth + parentDepth;
                transform.Dirty = false;
                transform.NotifyChanged();
            }

            foreach (Entity child in entity.Children)
            {
                EntityTreeRecalculateTransform(child, transform.WorldPosition, transform.WorldRotation.Theta, transform.WorldDepth, foundDirty || transform.Dirty);
            }

        }

        protected override List<Entity> GetCompatibleInPool()
        {
            return Pool.Entities.Where(ent => ent.HasComponents(CompatibleTypes) && ent.Parent is null).ToList();
        }
    }
}
