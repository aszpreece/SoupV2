using EntityComponentSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class EyeComponent : AbstractComponent
    {
        /// <summary>
        /// Activation of this eye's red channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        public float ActivationR { get; set; }
        /// <summary>
        /// Activation of this eye's green channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        public float ActivationG { get; set; }
        /// <summary>
        /// Activation of this eye's blue channel. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        public float ActivationB { get; set; }

        /// <summary>
        /// Field of view of this eye. Between 0 and Pi
        /// </summary>
        public float Fov { get; set; }

        /// <summary>
        /// Range of the eye in world units
        /// </summary>
        /// 
        private float _eyeRange;
        private float _eyeRangeSquared;
        public float EyeRange { 
            get => _eyeRange;
            set {
                _eyeRange = value;
                _eyeRangeSquared = value * value;
            } 
        }

        public float EyeRangeSquared { get => _eyeRangeSquared; }


        public EyeComponent(Entity owner): base(owner)
        {

        }
    }
}
