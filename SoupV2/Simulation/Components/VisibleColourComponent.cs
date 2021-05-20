using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Brain;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class VisibleColourComponent: AbstractComponent
    {

        [Browsable(false)]
        public Color Colour { get => new Color(RealR, RealG, RealB); }
        // Controls the colour filter of the entity

        private float _r = 0, _g= 0, _b = 0;
        private float _lerpAmount = 0.1f;

        [Control]
        [Browsable(false)]
        public float R { 
            get => _r;
            set{
                _r = MathHelper.Lerp(_r, value, _lerpAmount);
            } 
        }
        [Control]
        [Browsable(false)]
        public float G {
            get => _g;
            set
            {
                _g = MathHelper.Lerp(_g, value, _lerpAmount);
            }
        }
        [Control]
        [Browsable(false)]
        public float B {
            get => _b;
            set
            {
                _b = MathHelper.Lerp(_b, value, _lerpAmount);
            }
        }

        [Input]
        public float RealR { 
            get => (_r + 1) /2; 
            set { 
                if (value >= 0 && value <= 1)
                {
                    _r = (value * 2) - 1;
                }
            } 
        }

        [Input]
        public float RealG { 
            get => (_g + 1) /2; 
            set {
                if (value >= 0 && value <= 1)
                {
                    _g = (value * 2) - 1;
                }
            } 
        }

        [Input]
        public float RealB { 
            get => (_b + 1) /2; 
            set {
                if (value >= 0 && value <= 1)
                {
                    _b = (value * 2) - 1;
                }
            } 
        }


        public VisibleColourComponent(Entity owner): base(owner)
        {

        }

    }
}
