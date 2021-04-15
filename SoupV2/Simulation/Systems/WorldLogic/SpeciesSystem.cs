using EntityComponentSystem;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Settings;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Systems.WorldLogic
{
    public class SpeciesSystem : EntitySystem
    {
        /// <summary>
        /// The a dictionary of species based on the critter types in the simulation.
        /// The 0th position of each list is regarded as the 'origin' species.
        /// Each new critter is placed into this species.
        /// This way there isn't an explosion of species- only new critters that are successful and have babies create new species.
        /// </summary>
        public Dictionary<string, List<Species>> Species { get; set; } = new Dictionary<string, List<Species>>();

        public int NextSpeciesId { get; set; } = 0;

        private float _similarityThreshold;
        private Simulation _simulation;
        public SpeciesSystem(EntityManager pool, Simulation simulation, float similarityThreshold) : base(pool, (e) => e.HasComponents(typeof(BrainComponent)))
        {
            _similarityThreshold = similarityThreshold;
            _simulation = simulation;
        }
        protected override void OnPoolEntityChanged(EntityManager pool, Entity entity)
        {
            var index = Compatible.IndexOf(entity);
            // If we already have the entity, check if we are still compatible
            if (index >= 0)
            {
                if (!CompatiblePredicate(entity))
                {
                    SwapRemove.SwapRemoveList(Compatible, index);
                }
            }
            else
            // If we do not have it but we are compatible, add it
            {
                if (CompatiblePredicate(entity))
                {
                    Compatible.Add(entity);
                    var brain = entity.GetComponent<BrainComponent>();
     
                    // This will happen with the initial critters. Place into 'origin' species.
                    if (brain.ParentSpecies is null )
                    {
                        brain.Species = Species[entity.Tag][0];
                        return;
                    }
                    // Check what species the child should go in.
                    // If compatible with the parent species then place it in that species.
                    // Else, create a new species.
                    if (brain.ParentSpecies.Representative.CompareSimilarity(brain.BrainGenotype) <= _similarityThreshold)
                    {
                        brain.Species = brain.ParentSpecies;
                    }
                    else
                    {
                        int newId = NextSpeciesId;
                        NextSpeciesId++;

                        // Create representative for species
                        AbstractBrainGenotype rep = (AbstractBrainGenotype)brain.BrainGenotype.Clone();

                        var newSpecies = new Species()
                        {
                            Id = newId,
                            Representative = rep,
                            TimeCreated = _simulation.Tick * _simulation.GameSpeed,
                        };
                        // Create new species.
                        Species[entity.Tag].Add(newSpecies);
                        brain.Species = newSpecies;
                    }
                }
            }
        }

        public void InitializeSpecies(List<CritterTypeSetting> critterTypes)
        {
            // Initialize origin species with a random genotype
            foreach (CritterTypeSetting critterTypeSetting in critterTypes)
            {
                var originGenotype = (AbstractBrainGenotype)Activator.CreateInstance(critterTypeSetting.BrainType);
                Species originSpecies = new Species()
                {
                    Id = NextSpeciesId,
                    Representative = originGenotype,
                    TimeCreated = 0,
                };
                NextSpeciesId++;
                Species.Add(critterTypeSetting.DefinitionId, new List<Species>() { originSpecies });
            }
        }
    }
}
