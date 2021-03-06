using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class CircleColliderComponent : AbstractComponent
    {

        [JsonIgnore]
        public TransformComponent Transform { get; set; }

        private float _radius = 1.0f;

        [Description("The radius of the circle hitbox.")]
        public float Radius {
            get => _radius;
            set
            {
                if (value >= 0)
                {
                    _radius = value;
                }
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public Vector2 Position { get; set; } = Vector2.Zero;

        [JsonIgnore]
        [Browsable(false)]
        public bool Colliding
        {
            get
            {
                return Collisions.Count > 0;
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public List<CircleColliderComponent> Collisions { get; internal set; } = new List<CircleColliderComponent>();

        public CircleColliderComponent(Entity owner) : base(owner)
        {

        }

        public void UpdatePosition(TransformComponent transform)
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
