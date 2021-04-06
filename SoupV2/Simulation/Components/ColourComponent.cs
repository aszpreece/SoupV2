using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class ColourComponent: AbstractComponent
    {

        public Color colour { get => new Color(R, G, B); }
        // Controls the colour filter of the entity

        private float _r = 0, _g= 0, _b = 0;
        private float _lerpAmount = 0.1f;

        [Control]
        public float R { 
            get => _r;
            set{
                _r = MathHelper.Lerp(_r, value, _lerpAmount);
            } 
        }
        [Control]
        public float G {
            get => _g;
            set
            {
                _g = MathHelper.Lerp(_g, value, _lerpAmount);
            }
        }
        [Control]
        public float B {
            get => _b;
            set
            {
                _b = MathHelper.Lerp(_b, value, _lerpAmount);
            }
        }

        public float RealR { get => _r; set { _r = value; } }
        public float RealG { get => _g; set { _g = value; } }
        public float RealB { get => _b; set { _b = value; } }


        public ColourComponent(Entity owner): base(owner)
        {

        }

    }
}
