using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class TransformComponent
         : IComponent
    {
        public Entity Owner { get; set; }

        private bool _dirty = true;
        public Vector3 LocalPosition { get; set; }
        public Vector3 WorldPosition { get; set; }
        public Vector3 Scale { get; set; }
        public Rotation Rotation { get; set; }

        public TransformComponent()
        {
            
        }

        public float GetWorldDepth()
        {
            return WorldPosition.Z;
        }
        
    }
}
