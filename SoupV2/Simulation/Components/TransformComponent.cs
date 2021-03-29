using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Exceptions;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class TransformComponent
         : IComponent
    {
        public bool Dirty { get; set; }
        public Entity Owner { get; set; }

        private Vector2 _localPosition;
        public Vector2 LocalPosition { get
            {
                return _localPosition;
            }
            set
            {
                Dirty = true;
                _localPosition = value;
            }
        }
        public Vector2 WorldPosition { get; internal set; }

        private Rotation _localRotation;
        public Rotation LocalRotation
        {
            get
            {
                return _localRotation;
            }
            set
            {
                Dirty = true;
                _localRotation = value;
            }
        }
        public Rotation WorldRotation { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        private float _localDepth;
        public float LocalDepth
        {
            get
            {
                return _localDepth;
            }
            set
            {
                Dirty = true;
                _localDepth = value;
            }
        }
        public float WorldDepth { get; set; }

        public delegate void TransformChange(TransformComponent transform);

        public event TransformChange OnWorldChange;

        public TransformComponent(Entity owner)
        {
            Owner = owner;
            WorldPosition = new Vector2(0, 0);
            WorldRotation = new Rotation(0);
            LocalRotation = new Rotation(0);
            LocalPosition = new Vector2(0, 0);
            LocalDepth = 0;
            WorldDepth = 0;
        }

        internal void NotifyChanged()
        {
            OnWorldChange?.Invoke(this);
        }

        public Vector2 WorldForward {
            get {
                return new Vector2((float)Math.Cos(WorldRotation.Theta), (float)-Math.Sin(WorldRotation.Theta));
            }
        }

        public Vector2 WorldLeft
        {
            get
            {
                return new Vector2((float)Math.Cos(WorldRotation.Theta + Math.PI / 2), (float)-Math.Sin(WorldRotation.Theta + Math.PI / 2));
            }
        }
    }
}
