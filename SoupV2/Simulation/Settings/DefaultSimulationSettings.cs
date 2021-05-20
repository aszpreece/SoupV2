using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Settings
{
    public static class DefaultSimulationSettings
    {
        public static SimulationSettings GetSettings()
        {
            SimulationSettings settings = new();

            settings.CritterTypes.Add(
            new CritterTypeSetting()
            {
                DefinitionId = "Soupling",
                InitialCount = 10,
                MinimumCount = 40,
                StartEnergy = 5,
                FreeSpaceWorldUnits = 30,
                RespawnDelay = 0.2f,
            });

            settings.FoodTypes.Add(
            new FoodTypeSetting()
            {
                DefinitionId = "DefaultFood",
                InitialCount = 150,
                MinimumCount = 70,
                StartEnergy = 5,
                FreeSpaceWorldUnits = 30,
                RespawnDelay = 0.2f,
            });
            return settings;
        }
    }
}
