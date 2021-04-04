using EntityComponentSystem;
using SoupV2.NEAT;
using SoupV2.NEAT.Genes;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    class ReproductionSystem : EntitySystem
    {

        private MutationManager _muationManager;
        private InnovationIdManager _innovationIdManager;
        public ReproductionSystem(EntityPool pool, MutationConfig config, InnovationIdManager innovationIdManager) : base(pool, (e) => e.HasComponents(typeof(ReproductionComponent)))
        {
            _muationManager = new MutationManager(config);
            _innovationIdManager = innovationIdManager;
        }

        public void Update()
        {
            Random r = new Random();
            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var reproduction = entity.GetComponent<ReproductionComponent>();
                
                if (r.NextDouble() < 0)
                {
                    // Some set up for the new child, setting positions.
                    var added = Pool.AddEntityFromDefinition(reproduction.ChildDefinitionId);
                    added.GetComponent<TransformComponent>().LocalPosition = entity.GetComponent<TransformComponent>().LocalPosition;

                    // The new child will need a mutated brain
                    var originalBrain = entity.GetComponent<BrainComponent>().Brain;
                    // Messy!! But casting is necessary.
                    
                    Genotype copy = (Genotype)((Phenotype)originalBrain).Genotype.Clone();
                    _muationManager.Mutate((Genotype)copy, _innovationIdManager);
                    var newBrain = added.GetComponent<BrainComponent>();
                    newBrain.SetBrain(new Phenotype(copy));
                    newBrain.SetUpLinks();
                }
            }
        }

    }
}
