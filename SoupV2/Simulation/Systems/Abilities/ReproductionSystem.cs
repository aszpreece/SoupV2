using EntityComponentSystem;
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
        private List<Species> _species = new List<Species>();

        public delegate void ChildBorn(BirthEventInfo e);
        public event ChildBorn BirthEvent;

        public ReproductionSystem(EntityPool pool, NeatMutationConfig config, InnovationIdManager innovationIdManager, EnergyManager energyManager, int currentMaxSpeciesId, float similarityThreshold) : base(pool, (e) => e.HasComponents(typeof(ReproductionComponent), typeof(EnergyComponent)))
        {
            _muationManager = new MutationManager(config);
            _innovationIdManager = innovationIdManager;
            _energyManager = energyManager;
            _nextSpeciesIdCounter = currentMaxSpeciesId + 1;
            _similarityThreshold = similarityThreshold;
        }

        public void Update()
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
                    var babyEntity = Pool.AddEntityFromDefinition(reproduction.ChildDefinitionId);

                    babyEntity.GetComponent<EnergyComponent>().Energy = charged;
                    var parentPosition = parentEntity.GetComponent<TransformComponent>().LocalPosition;
                    babyEntity.GetComponent<TransformComponent>().LocalPosition = parentPosition;

                    // The new child will need a mutated brain
                    var originalBrain = parentEntity.GetComponent<BrainComponent>().Brain;

        
                    AbstractGenotype parentGenotype = ((NeatPhenotype)originalBrain).Genotype;
                    NeatGenotype childGenotype = (NeatGenotype)parentGenotype.Clone();
                   
                    
                    _muationManager.Mutate((NeatGenotype)childGenotype, _innovationIdManager);
                    var newBrain = babyEntity.GetComponent<BrainComponent>();
                    newBrain.SetBrain(new NeatPhenotype(childGenotype));
                    newBrain.SetUpLinks();

                    // Check what species the child should go in.
                    // If compatible with the parent species then place it in that species.
                    // Else, create a new species.
                    if (parentGenotype.Species.Representative.CompareSimilarity(childGenotype) <= _similarityThreshold)
                    {
                        Debug.WriteLine("Old species");
                        childGenotype.Species = parentGenotype.Species;
                    } else
                    {
                        int newId = _nextSpeciesIdCounter;
                        _nextSpeciesIdCounter++;

                        // Create representative for species
                        NeatGenotype rep = (NeatGenotype)childGenotype.Clone();
                        rep.Species = null;

                        var newSpecies = new Species()
                        {
                            Id = newId,
                            Representative = rep,
                            TimeCreated = 0,
                        };
                        // Create new species.
                        _species.Add(newSpecies);
                        childGenotype.Species = newSpecies;
                        Debug.WriteLine("New species");
                    }
                    BirthEventInfo e = new BirthEventInfo()
                    {
                        ChildGenotype = childGenotype,
                        ChildId = babyEntity.Id,
                        Location = parentPosition,
                        ParentId = parentEntity.Id,
                        TimeStamp = 0,
                    };

                    BirthEvent?.Invoke(e);
                }
            }
        }

    }
}
