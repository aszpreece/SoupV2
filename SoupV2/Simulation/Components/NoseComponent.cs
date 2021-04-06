using EntityComponentSystem;
using Newtonsoft.Json;

namespace SoupV2.Simulation.Components
{
    class NoseComponent : AbstractComponent
    {

        public NoseComponent(Entity owner) : base(owner)
        {

        } 

        [JsonIgnore]
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
                _noseRange = value;
                _noseRangeSquared = value * value;
            }
        }

        public float NoseRangeSquared { get => _noseRangeSquared; }

    }
}
