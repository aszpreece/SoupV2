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
        private EnergyManager _energyManager;
        private int _nextSpeciesIdCounter;
        public ReproductionSystem(EntityPool pool, MutationConfig config, InnovationIdManager innovationIdManager, EnergyManager energyManager, int currentMaxSpeciesId) : base(pool, (e) => e.HasComponents(typeof(ReproductionComponent), typeof(EnergyComponent)))
        {
            _muationManager = new MutationManager(config);
            _innovationIdManager = innovationIdManager;
            _energyManager = energyManager;
            _nextSpeciesIdCounter = currentMaxSpeciesId + 1;
        }

        public void Update()
        {
            Random r = new Random();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var reproduction = entity.GetComponent<ReproductionComponent>();

                if (reproduction.Reproduce > reproduction.ReproductionThreshold)
                {
                    var energy = entity.GetComponent<EnergyComponent>();
                    
                    // check if we have the energy
                    if (!energy.CanAfford(reproduction.ReproductionEnergyCost))
                    {
                        continue;
                    }
                    if (energy.Energy - reproduction.ReproductionEnergyCost < reproduction.RequiredRemaining)
                    {
                        continue;
                    }

                    // charge teh parent energy. Each critter has an efficency rating, so some energy gets wasted an the rest gets sent to the child.
                    float charged = energy.ChargeEnergy(reproduction.ReproductionEnergyCost);

                    float toChild = reproduction.Efficency * charged;
                    float wasted = charged - toChild;

                    // Let the energy manager know about the wasted energy.
                    _energyManager.DepositEnergy(wasted);

                    // Some set up for the new child, setting positions/giving energy.
                    var added = Pool.AddEntityFromDefinition(reproduction.ChildDefinitionId);

                    added.GetComponent<EnergyComponent>().Energy = charged;
                    added.GetComponent<TransformComponent>().LocalPosition = entity.GetComponent<TransformComponent>().LocalPosition;

                    // The new child will need a mutated brain
                    var originalBrain = entity.GetComponent<BrainComponent>().Brain;
           
                    // Messy!! But casting is necessary.
                    NeatGenotype copy = (NeatGenotype)((NeatPhenotype)originalBrain).Genotype.Clone();
                    _muationManager.Mutate((NeatGenotype)copy, _innovationIdManager);
                    var newBrain = added.GetComponent<BrainComponent>();
                    newBrain.SetBrain(new NeatPhenotype(copy));
                    newBrain.SetUpLinks();

                    if (copy.Species.)

                }
            }
        }

    }
}
