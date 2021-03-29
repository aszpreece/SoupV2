﻿using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Physics
{
    class CircleColliderComponent : IComponent
    {

        public TransformComponent Transform { get; set; }

        public float Radius { get; set; } = 1.0f;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public bool Colliding
        {
            get
            {
                return Collisions.Count > 0;
            }
        }

        public List<CircleColliderComponent> Collisions { get; internal set; } = new List<CircleColliderComponent>();

        public CircleColliderComponent(TransformComponent transform)
        {
            Transform = transform;
            transform.OnWorldChange += UpdatePosition;
        }

        private void UpdatePosition(TransformComponent transform)
        {
            Position = transform.WorldPosition;
        }

        public float MinX()
        {
            return Position.X - Radius;
        }

        public float MaxX()
        {
            return Position.X + Radius;
        }

        /// <summary>
        /// Checks if this collider is colliding with another collider.
        /// TODO make it so it returns more info about the collision.
        /// </summary>
        public bool Intersects(CircleColliderComponent other)
        {
            float dist = (this.Position - other.Position).LengthSquared();
            if (dist < Math.Pow(other.Radius + Radius, 2))
            {
                return true;
            }
            return false;
        }

        public void AddCollision(CircleColliderComponent other)
        {
            Collisions.Add(other);
        }

        public void ClearCollisions()
        {
            Collisions.Clear();
        }
    }
}