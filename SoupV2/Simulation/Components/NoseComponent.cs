using EntityComponentSystem;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SoupV2.Simulation.Components
{
    class NoseComponent : AbstractComponent
    {

        public NoseComponent(Entity owner) : base(owner)
        {

        } 

        [JsonIgnore]
        [Browsable(false)]
        public float Activation { get; set; }

        /// <summary>
        /// Range of the eye in world units
        /// </summary>
        /// 
        private float _noseRange;
        private float _noseRangeSquared;
        public float NoseRange
        {
            get => _noseRange;
            set
            {
                if (value > 0)
                {
                    _noseRange = value;
                    _noseRangeSquared = value * value;
                }
            }
        }

        public float NoseRangeSquared { get => _noseRangeSquared; }

    }
}
