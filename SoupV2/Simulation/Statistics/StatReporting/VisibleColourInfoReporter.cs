using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EventsAndInfo;
using SoupV2.Simulation.Statistics.StatReporting.ReportInfoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Statistics.StatReporting
{
    /// <summary>
    /// Gatheres info on entity position
    /// </summary>
    public class VisibleColourInfoReporter : AbstractReporter
    {


        public VisibleColourInfoReporter(EntityManager manager, float reportInterval) : base(manager, reportInterval, (entity) => entity.HasComponents(typeof(BrainComponent), typeof(VisibleColourComponent)))
        {

        }

        public override void GatherStats(uint tick, float gameSpeed)
        {
            Parallel.For(0, Compatible.Count, (i) => {

                var entity = Compatible[i];
                var col = entity.GetComponent<VisibleColourComponent>();
                var species = entity.GetComponent<BrainComponent>().Species.Id;
           
                var reportObj = new SimulationColourInfo(col.RealR, col.RealG, col.RealB, species, tick * gameSpeed);

                Report(reportObj);
            });
        }
    }
}
