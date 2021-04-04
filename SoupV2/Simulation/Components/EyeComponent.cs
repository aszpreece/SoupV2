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
        /// Activation of this eye. Between 0 and 1.
        /// </summary>
        [JsonIgnore]
        public float Activation { get; set; }

        /// <summary>
        /// Field of view of this eye. Between 0 and Pi
        /// </summary>
        public float Fov { get; set; }

        /// <summary>
        /// Range of the eye in world units
        /// </summary>
        /// 
        private float _eyeRange;
        private float _eyeRangeSqaured;
        public float EyeRange { 
            get => _eyeRange;
            set {
                _eyeRange = value;
                _eyeRangeSqaured = value * value;
            } 
        }

        public float EyeRangeSquared { get => _eyeRangeSqaured; }


        public EyeComponent(Entity owner): base(owner)
        {

        }
    }
}
