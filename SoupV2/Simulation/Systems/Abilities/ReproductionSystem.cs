using EntityComponentSystem;
using Newtonsoft.Json;
using SoupV2.NEAT;
using SoupV2.NEAT.Genes;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class ReproductionSystem : EntitySystem
    {

        private MutationManager _muationManager;
        private InnovationIdManager _innovationIdManager;
        private EnergyManager _energyManager;
        private int _nextSpeciesIdCounter;
        private float _similarityThreshold;

        public delegate void ChildBorn(BirthEventInfo e);
        public event ChildBorn BirthEvent;

        private Simulation _simulation;
        public ReproductionSystem(EntityManager pool, NeatMutationConfig config, InnovationIdManager innovationIdManager, EnergyManager energyManager, int currentMaxSpeciesId, float similarityThreshold, Simulation simulation) 
            : base(pool, (e) => e.HasComponents(typeof(ReproductionComponent), typeof(EnergyComponent)))
        {
            _muationManager = new MutationManager(config);
            _innovationIdManager = innovationIdManager;
            _energyManager = energyManager;
            _nextSpeciesIdCounter = currentMaxSpeciesId + 1;
            _similarityThreshold = similarityThreshold;
            _simulation = simulation;
        }

        public void Update(uint tick, float _gameSpeed)
        {
            Random r = new Random();

            for (int i = 0; i < Compatible.Count; i++)
            {
                var parentEntity = Compatible[i];
                var reproduction = parentEntity.GetComponent<ReproductionComponent>();

                if (reproduction.Reproduce > reproduction.ReproductionThreshold)
                {
                    var energy = parentEntity.GetComponent<EnergyComponent>();
                    
                    // check if we have the energy
                    if (!energy.CanAfford(reproduction.ReproductionEnergyCost))
                    {
                        continue;
                    }
                    if (energy.Energy - reproduction.ReproductionEnergyCost < reproduction.RequiredRemainingEnergy)
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
                    var babyEntity = Pool.AddEntityFromDefinition(reproduction.ChildDefinitionId, _simulation.JsonSettings, parentEntity.Tag);

                    babyEntity.GetComponent<EnergyComponent>().Energy = toChild;
                    var parentPosition = parentEntity.GetComponent<TransformComponent>().LocalPosition;
                    babyEntity.GetComponent<TransformComponent>().LocalPosition = parentPosition;

                    // The new child will need a mutated brain
                    var originalBrainComponent = parentEntity.GetComponent<BrainComponent>();
                    var originalBrain = originalBrainComponent.Brain;


                    AbstractBrainGenotype parentGenotype = ((NeatBrainPhenotype)originalBrain).Genotype;
                    NeatBrainGenotype childGenotype = (NeatBrainGenotype)parentGenotype.Clone();
                   
                    
                    _muationManager.Mutate(childGenotype, _innovationIdManager);
                    var newBrain = babyEntity.GetComponent<BrainComponent>();
                    newBrain.SetGenotype(childGenotype);

                    // Do not need to pass it type as we have already set the brain, therefore it does not need initializing.
                    newBrain.SetUpLinks(null, _simulation.Settings);
                    newBrain.ParentSpecies = originalBrainComponent.Species;

                    BirthEventInfo e = new BirthEventInfo(parentPosition, tick * _gameSpeed, parentEntity.Id, parentEntity.Tag, babyEntity.Id, babyEntity.Tag, childGenotype, originalBrainComponent.Species.Id);
                    BirthEvent?.Invoke(e);
                }
            }
        }

    }
}
