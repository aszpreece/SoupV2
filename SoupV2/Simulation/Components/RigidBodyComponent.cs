﻿using EntityComponentSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class RigidBodyComponent : IComponent
    {
        private float _inv_mass = 1f;
        private float _mass = 1f;

        public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _inv_mass = 1.0f / value;
                _mass = value;
            }
        }

        public float InvMass
        {
            get
            {
                return _inv_mass;
            }

        }

        public float Restitution { get; internal set; } = 0.9f;


        public void Reset()
        {
            _impulse = Vector2.Zero;
            _force = Vector2.Zero;
        }



        private Vector2 _force = new Vector2();
        private Vector2 _impulse = new Vector2();
        public Vector2 Impulse { get => _impulse; }
        public Vector2 Force { get => _force; }

        public void ApplyForce(Vector2 newtonVector)
        {
            _force += newtonVector;
        }

        public void ApplyImpulse(Vector2 impulseVector)
        {
            _impulse += impulseVector;
        }

    }
}
