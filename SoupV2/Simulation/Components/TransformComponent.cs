using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.Simulation.Exceptions;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class TransformComponent
         : AbstractComponent
    {
        public bool Dirty { get; set; }


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


        [JsonIgnore]
        public Vector2 WorldPosition { get; internal set; }= new Vector2(0, 0);

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

        [JsonIgnore]
        public Rotation WorldRotation { get; set; } = new Rotation(0);


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

        [JsonIgnore]
        public float WorldDepth { get; set; } = 0;

        public delegate void TransformChange(TransformComponent transform);

        public TransformComponent(Entity owner) : base (owner)
        {
            LocalRotation = new Rotation(0);
            LocalPosition = new Vector2(0, 0);
            LocalDepth = 0;
        }

        [JsonIgnore]
        public Vector2 WorldForward {
            get {
                return new Vector2((float)Math.Cos(WorldRotation.Theta), (float)Math.Sin(WorldRotation.Theta));
            }
        }

        [JsonIgnore]
        public Vector2 WorldLeft
        {
            get
            {
                return new Vector2((float)Math.Cos(WorldRotation.Theta + Math.PI / 2), (float)Math.Sin(WorldRotation.Theta + Math.PI / 2));
            }
        }
    }
}
