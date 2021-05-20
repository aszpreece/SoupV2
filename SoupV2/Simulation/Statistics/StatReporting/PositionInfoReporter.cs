using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Statistics.StatReporting
{
    /// <summary>
    /// Gatheres info on entity positions
    /// </summary>
    public class PositionInfoReporter : AbstractReporter
    {


        public PositionInfoReporter(EntityManager manager, float reportInterval, Func<Entity, bool> predicate) : base(manager, reportInterval, predicate)
        {

        }

        public override void GatherStats(uint tick, float gameSpeed)
        {
            Parallel.For(0, Compatible.Count, (i) => {

                var entity = Compatible[i];
                var transform = entity.GetComponent<TransformComponent>();

                var reportObj = new SimulationPositionInfo
                (tick * gameSpeed, 
                transform.WorldPosition, 
                transform.WorldRotation.Theta, 
                entity.Id, 
                entity.Tag
                );

                Report(reportObj);
            });
        }
    }
}
