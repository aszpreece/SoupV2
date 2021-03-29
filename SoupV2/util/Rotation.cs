using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util
{
    public struct Rotation
    {
        public static readonly float FullRot = (float)(2.0 * Math.PI);

        private float _theta;

        public float Theta
        {
            get => _theta %= FullRot;
            set
            {
                _theta = value;
            }
        }

        public Rotation(float theta)
        {
            _theta = theta;
        }

        public void Rotate(float delta)
        {
            _theta += delta;
        }

        public static Rotation operator +(Rotation a, Rotation b)
        {
            return new Rotation(a.Theta + b.Theta);
        }


        public static Rotation operator +(Rotation a, float b)
        {
            return new Rotation(a.Theta + b);
        }

    }
}
