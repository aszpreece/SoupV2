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
    /// Maintains the amount of food in the simulation.
    /// </summary>
    public class FoodRespawnSystem
    {
        private EntityManager _pool;
        private EnergyManager _energyManager;
        private AdjacencyGrid _grid;
        private Random _rand;
        private Simulation _simulation;
        private List<FoodTypeSetting> _foodTypes;

        // Special data structure for mainting each type of food.
        private Dictionary<string, (FoodTypeSetting, HashSet<Entity>)> Compatible = new Dictionary<string, (FoodTypeSetting, HashSet<Entity>)>();

        public FoodRespawnSystem(EntityManager pool,
            EnergyManager energyManager,
            AdjacencyGrid grid,
            List<FoodTypeSetting> foodTypes,
            Simulation simulation)
        {
            _pool = pool;
            _energyManager = energyManager;
            _grid = grid;
            _rand = new Random();
            _simulation = simulation;
            _foodTypes = foodTypes;

            pool.EntityAdded += OnPoolEntityAdded;
            pool.EntityRemoved += OnPoolEntityRemoved;

            // Initialize the hash sets
            foreach (FoodTypeSetting foodTypeSetting in _foodTypes)
            {
                Compatible.Add(foodTypeSetting.TypeTag, (foodTypeSetting, new HashSet<Entity>()));
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
            if (Compatible.ContainsKey(entity.Tag))
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
            foreach (var (foodTypeSetting, populationSet) in Compatible.Values)
            {
                // Only spawn if tick is between time settings
                if (!(foodTypeSetting.SpawnFromTick < tick * gameSpeed && tick * gameSpeed < foodTypeSetting.SpawnUntilTick))
                {
                    continue;
                }

                // Only spawn a new food if the population has dipped too low and we can afford to spawn a cluster
                if (populationSet.Count > foodTypeSetting.MinimumCount - foodTypeSetting.ClusterSize)
                {
                    continue;
                }

                foodTypeSetting.Timer += gameSpeed;

                // Calculate cost of spawning this type of food.
                float cost = foodTypeSetting.StartEnergy * foodTypeSetting.ClusterSize;

                if (foodTypeSetting.Timer >= foodTypeSetting.RespawnDelay && _energyManager.CanAfford(cost))
                {  
                    
                    // Only attempt a few times to avoid blocking up the simulation loop.
                    int attempts = 0;
                    while (attempts < 10)
                    {
                        // If the delay has passed respawn a critter pellet
                        int clusterX = _rand.Next(-_grid.WorldWidth / 2, _grid.WorldWidth / 2);
                        int clusterY = _rand.Next(-_grid.WorldHeight / 2, _grid.WorldHeight / 2);
                        var clusterpos = new Vector2(clusterX, clusterY);

                        //// Make sure the immediate radius is free of anything with a mouth.
                        if (_grid.GetNearbyEntities(clusterpos, foodTypeSetting.FreeSpaceWorldUnits, (e) => e.HasComponent<MouthComponent>()).Any())
                        {
                            attempts++;
                            continue;
                        }

                        for (int i = 0; i < foodTypeSetting.ClusterSize; i++)
                        {
                            // Add noise to area so food is a little bit spread out over an area.

                            Vector2 foodPos = clusterpos + new Vector2((float)_rand.NextDouble() - 0.5f, (float)_rand.NextDouble() - 0.5f) * 2 * foodTypeSetting.ClusterRadius;

                            // Ensures food spawn within the world bounds
                            var foodPosTuple = _grid.ClampToWorldCoords((foodPos.X, foodPos.Y));
                            foodPos = new Vector2(foodPosTuple.Item1, foodPosTuple.Item2);

                            Entity foodEntity = _pool.AddEntityFromDefinition(foodTypeSetting.DefinitionId, _simulation.JsonSettings, foodTypeSetting.TypeTag);

                            if (foodEntity.HasComponent<EnergyComponent>())
                            {
                                foodEntity.GetComponent<EnergyComponent>().Energy = foodTypeSetting.StartEnergy;
                                _energyManager.ChargeEnergy(foodTypeSetting.StartEnergy);
                            }
                            var transform = foodEntity.GetComponent<TransformComponent>();
                            transform.LocalPosition = foodPos;
                        }
                        // If we placed a food reset the timer.
                        foodTypeSetting.Timer = 0;
                        break;
                    }
                }
            }

        }
    }
}
