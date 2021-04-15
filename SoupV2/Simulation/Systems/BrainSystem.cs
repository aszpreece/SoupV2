using EntityComponentSystem;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Systems
{
    public class BrainSystem : EntitySystem
    {
        public BrainSystem(EntityManager pool) : base(pool, (e) => e.HasComponents(typeof(BrainComponent)))
        {
        }

        public void Update(List<CritterTypeSetting> critterTypes)
        {
            Parallel.For(0, Compatible.Count, (i) => {
                
                var entity = Compatible[i];
                var brain = entity.GetComponent<BrainComponent>();
                // If this brain is not set up, find out what type it should be.
                if (!brain.SetUp)
                {
                    var critterType = critterTypes.Find((critterType) => critterType.DefinitionId == entity.Tag);
                    brain.SetUpLinks(critterType.BrainType);
                }
                brain.Calculate();

            });
        }

        /// <summary>
        /// Initial set up of brain components.
        /// </summary>
        /// <param name="critterTypes"></param>
        internal void SetUpBrains(List<CritterTypeSetting> critterTypes)
        {
            Parallel.For(0, Compatible.Count, (i) => {

                var entity = Compatible[i];
                var critterType = critterTypes.Find((critterType) => critterType.DefinitionId == entity.Tag);

                var brain = entity.GetComponent<BrainComponent>();
                if (!brain.SetUp)
                {
                    brain.SetUpLinks(critterType.BrainType);
                }
            });
        }
    }
}
