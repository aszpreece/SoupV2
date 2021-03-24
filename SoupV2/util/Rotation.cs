using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util
{
    public class Rotation
    {
        private double _theta;
        private bool _dirty = false;

        public static readonly double FullRot = 2 * Math.PI;

        public double Theta
        {
            get
            {
                if (_dirty)
                {
                    _theta = _theta % FullRot;
                }
                return _theta;
            }
            set
            {
                _dirty = true;
                _theta = value;
            }
        }


        public Rotation()
        {
            _theta = 0;
            _dirty = false;
        }

        public Rotation(double theta)
        {
            _theta = theta;
            _dirty = true;
        }

        public void Rotate(double delta)
        {
            _dirty = true;
            _theta += delta;
        }
    }
}
