using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Components
{
    public class OldAgeComponent : AbstractComponent
    {
        public OldAgeComponent(Entity owner): base(owner)
        {

        }
        public float MaxAge { get; set; } = 30f * 400f;
        public float CurrentAge { get; set; } = 0;

    }
}
