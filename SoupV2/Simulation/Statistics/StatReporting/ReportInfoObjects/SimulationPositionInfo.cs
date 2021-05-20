using Microsoft.Xna.Framework;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Statistics.StatReporting
{
    public class SimulationPositionInfo : AbstractSimulationInfo
    {

        public Vector2 Position { get; set; }

        public float RotationRadians { get; set; }
        public int EntityId { get; set; }
        public string EntityTag { get; set; }

        public SimulationPositionInfo(float timestamp, Vector2 position, float rotation, int entityId, string entityTag)
        {
            TimeStamp = timestamp;
            Position = position;
            EntityId = entityId;
            EntityTag = entityTag;
            RotationRadians = rotation;
        }
    }
}
