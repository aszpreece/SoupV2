using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class EyeComponent : AbstractComponent
    {
        /// <summary>
        /// Activation of this eye's red channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        [Input]
        public float ActivationR { get; set; }
        /// <summary>
        /// Activation of this eye's green channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        [Input]
        public float ActivationG { get; set; }
        /// <summary>
        /// Activation of this eye's blue channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        [Input]
        public float ActivationB { get; set; }


        private float _fov;
        /// <summary>
        /// Field of view of this eye. Between 0 and Pi
        /// </summary>
        [Browsable(false)]
        public float Fov { 
            get => _fov;
            set { 
                if (value > 0)
                {
                    _fov = value;
                }
            }  
        }

        public float FovDegrees
        {
            get => MathHelper.ToDegrees(_fov);
            set
            {
                Fov = MathHelper.ToRadians(value);
            }
        }


        /// <summary>
        /// Range of the eye in world units
        /// </summary>
        /// 
        private float _eyeRange;
        private float _eyeRangeSquared;
        public float EyeRange { 
            get => _eyeRange;
            set {
                if (value > 0)
                {
                    _eyeRange = value;
                    _eyeRangeSquared = value * value;
                }
            } 
        }

        [Browsable(false)]
        public float EyeRangeSquared { get => _eyeRangeSquared; }


        /// <summary>
        /// If true just return 1 or 0 if something is seen.
        /// </summary>
        public bool BinaryMode { get; set; } = false;


        public EyeComponent(Entity owner): base(owner)
        {

        }
    }
}
