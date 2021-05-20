using EntityComponentSystem;
using Newtonsoft.Json;
using SoupV2.Simulation.Brain;
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
        [Input]
        public float CosActivation { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        [Input]
        public float SinActivation { get; set; }

        /// <summary>
        /// If true only the closest food will be taken into account.
        /// </summary>
        public bool ClosestOnly { get; set; } = false;

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


        /// <summary>
        /// Determines whether distance is taken into account for activation of nose
        /// </summary>
        /// 
        [Description("If true, will not multiply returned results dist the inverse of the distance squared")]
        public bool ConsiderRange { get;  set; } = false;
    }
}
