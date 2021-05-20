using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Settings
{
    public abstract class AbstractEntityTypeSetting
    {
        public string TypeTag { get; set; } = "New";
        public string DefinitionId { get; set; }
        public uint InitialCount { get; set; }
        public uint MinimumCount { get; set; }

        /// <summary>
        /// The amount of free space around this entity there should be.
        /// Only checks entities with appropriate components, i.e check if food then check for mouths, if critter check for other brains.
        /// </summary>
        public float FreeSpaceWorldUnits { get; set; } = 30;

        /// <summary>
        /// Timer for when to respawn a entity should the population be too small. Used internally.
        /// </summary>
        [Browsable(false)]
        public float Timer { get; set; } = 0;

        /// <summary>
        /// Delay in ticks for when to try and respawn this entity should the population be too small.
        /// </summary>
        public float RespawnDelay { get; set; }

        /// <summary>
        /// Defines the amount of energy this entity starts with.
        /// </summary>
        public float StartEnergy { get; set; }

        public override string ToString()
        {
            return DefinitionId;
        }

        public float SpawnFromTick { get; set; } = 0;
        public float SpawnUntilTick { get; set; } = float.PositiveInfinity;


    }
}
