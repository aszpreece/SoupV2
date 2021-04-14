using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using System;
using System.Linq;

namespace SoupV2.Simulation.Systems
{
    enum FoodRespawnStyle
    {

    }

    public class FoodRespawnSystem
    {
        private EntityPool _pool;
        private EnergyManager _energyManager;
        private AdjacencyGrid _grid;
        private string _foodPelletDefinitionName;
        private readonly float _delaySeconds;
        private Random _rand;
        public FoodRespawnSystem(EntityPool pool, EnergyManager energyManager, AdjacencyGrid grid, string foodPelletDefinitionName, float delaySeconds)
        {
            _pool = pool;
            _energyManager = energyManager;
            _grid = grid;
            _foodPelletDefinitionName = foodPelletDefinitionName;
            _delaySeconds = delaySeconds;
            _rand = new Random(); 
        }

        private float timer = 0;
        private float _foodEnergy = 3;
        private float freeSpaceWU = 30f;
        public void Update(uint tick, float gameSpeed)
        {
            timer += gameSpeed;
            if (timer >= _delaySeconds && _energyManager.CanAfford(_foodEnergy))
            {
                timer = 0;

                int attempts = 0;
                while (attempts < 5)
                {
                    // If the delay has passed respawn a food pellet
                    int x = _rand.Next(-_grid.WorldWidth / 2, _grid.WorldWidth / 2);
                    int y = _rand.Next(-_grid.WorldHeight / 2, _grid.WorldHeight / 2);
                    var pos = new Vector2(x, y);

                    // Make sure the immediate radius is free of anything that might eat the food
                    if (_grid.GetNearbyEntities(pos, freeSpaceWU, (e) => !e.HasComponent<MouthComponent>()).Any())
                    {
                        attempts++;
                        continue;
                    }

                    Entity food =_pool.AddEntityFromDefinition(_foodPelletDefinitionName);
                    food.GetComponent<TransformComponent>().LocalPosition = pos;
                    food.GetComponent<EnergyComponent>().Energy = _foodEnergy;
                    _energyManager.ChargeEnergy(_foodEnergy);
                    break;
                }
            }

        }
    }
}
