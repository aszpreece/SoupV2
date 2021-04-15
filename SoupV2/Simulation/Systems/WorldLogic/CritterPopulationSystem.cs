using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using SoupV2.Simulation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Systems.WorldLogic
{

    /// <summary>
    /// Maintains the population of each type of critter in the population.
    /// </summary>
    public class CritterPopulationSystem
    {
        private EntityManager _pool;
        private EnergyManager _energyManager;
        private AdjacencyGrid _grid;

        private Random _rand;
        private Simulation _simulation;
        private List<CritterTypeSetting> _critterTypes;

        // Special data structure for mainting each type of critter.
        private Dictionary<string, (CritterTypeSetting, HashSet<Entity>)> Compatible = new Dictionary<string, (CritterTypeSetting, HashSet<Entity>)>();

        public CritterPopulationSystem(EntityManager pool,
            EnergyManager energyManager,
            AdjacencyGrid grid,
            List<CritterTypeSetting> critterTypes,
            Simulation simulation)
        {
            _pool = pool;
            _energyManager = energyManager;
            _grid = grid;
            _rand = new Random();
            _simulation = simulation;
            _critterTypes = critterTypes;

            pool.EntityAdded += OnPoolEntityAdded;
            pool.EntityRemoved += OnPoolEntityRemoved;

            // Initialize the hash sets
            foreach (CritterTypeSetting critterTypeSetting in _critterTypes)
            {
                Compatible.Add(critterTypeSetting.DefinitionId, (critterTypeSetting, new HashSet<Entity>()));
            }
        }

        /// <summary>
        /// Maintains list of our populations
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="entity"></param>
        protected virtual void OnPoolEntityAdded(EntityManager pool, Entity entity)
        {
            // Check if we are tracking this tag
            if(Compatible.ContainsKey(entity.Tag))
            {
                Compatible[entity.Tag].Item2.Add(entity);
            }
        }

        protected virtual void OnPoolEntityRemoved(EntityManager pool, Entity entity)
        {
            // Check if we are tracking this tag
            if (Compatible.ContainsKey(entity.Tag))
            {
                Compatible[entity.Tag].Item2.Remove(entity);
            }
        }

        public void Update(uint tick, float gameSpeed)
        {
            foreach (var (critterTypeSetting, populationSet) in Compatible.Values)
            {
                // Only spawn a new critter if the population has dipped too low.
                if (populationSet.Count >= critterTypeSetting.MinimumCount)
                {
                    continue;
                }

                // Taken this out, as critters should be able to spawn no matter what.
                // The energy manager will just have to go into debt
                //  && _energyManager.CanAfford(critterTypeSetting.StartEnergy)
                critterTypeSetting.Timer += gameSpeed;
                if (critterTypeSetting.Timer >= critterTypeSetting.RespawnDelay)
                {
                    // Only attempt a few times to avoid blocking up the simulation loop.
                    int attempts = 0;
                    while (attempts < 10)
                    {
                        // If the delay has passed respawn a critter pellet
                        int x = _rand.Next(-_grid.WorldWidth / 2, _grid.WorldWidth / 2);
                        int y = _rand.Next(-_grid.WorldHeight / 2, _grid.WorldHeight / 2);
                        var pos = new Vector2(x, y);

                        //// Make sure the immediate radius is free of anything with a brain.
                        if (_grid.GetNearbyEntities(pos, critterTypeSetting.FreeSpaceWorldUnits, (e) => e.HasComponent<BrainComponent>()).Any())
                        {
                            attempts++;
                            continue;
                        }

                        Entity critter = _pool.AddEntityFromDefinition(critterTypeSetting.DefinitionId, _simulation.JsonSettings);
                        
                        if (critter.HasComponent<EnergyComponent>())
                        {
                            critter.GetComponent<EnergyComponent>().Energy = critterTypeSetting.StartEnergy;
                            _energyManager.ChargeEnergy(critterTypeSetting.StartEnergy);
                        }

                        var transform = critter.GetComponent<TransformComponent>();
                        transform.LocalPosition = pos;
                        transform.LocalRotation = new util.Rotation((float)(_rand.NextDouble() * MathHelper.TwoPi)); 

                        // If we placed a critter reset the timer.
                        critterTypeSetting.Timer = 0;
                        break;
                    }
                }
            }

        }
    }
}
