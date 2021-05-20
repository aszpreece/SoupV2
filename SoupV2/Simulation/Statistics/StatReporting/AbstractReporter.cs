using EntityComponentSystem;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Statistics.StatReporting
{
    public abstract class AbstractReporter : EntitySystem
    {
        public delegate void PositionInfoHandler(AbstractSimulationInfo info);
        public event PositionInfoHandler OnReport;

        public float ReportInterval { get; set; } 
        public AbstractReporter(EntityManager pool, float reportInterval, Func<Entity, bool> predicate) : base(pool, predicate)
        {
            ReportInterval = reportInterval;
        }

        /// <summary>
        /// Updates the reporter. If it is time ot gather the statistics then do so and reset the timer.
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="gameSpeed"></param>
        public void Update(uint tick, float gameSpeed)
        {
            _reportTimer += gameSpeed;
            if (_reportTimer > ReportInterval)
            {
                _reportTimer = 0;
                GatherStats(tick, gameSpeed);
            }
        }

        float _reportTimer = 0.0f;
        public abstract void GatherStats(uint tick, float gameSpeed);

        public void Report(AbstractSimulationInfo info)
        {
            OnReport?.Invoke(info);
        }
    }
}
