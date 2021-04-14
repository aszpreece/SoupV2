using EntityComponentSystem;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Systems
{
    public class BrainSystem : EntitySystem
    {
        public BrainSystem(EntityPool pool) : base(pool, (e) => e.HasComponents(typeof(BrainComponent)))
        {
        }

        public void Update()
        {
            Parallel.For(0, Compatible.Count, (i) => {
                var entity = Compatible[i];
                var brain = entity.GetComponent<BrainComponent>();
                brain.Calculate();

            });
        }
    }
}
