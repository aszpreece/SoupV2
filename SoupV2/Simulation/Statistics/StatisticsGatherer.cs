using SoupV2.Simulation.Events;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SoupV2.Simulation.Statistics
{
    /// <summary>
    /// Gathers all the statistics from the simulation in a single stack concurrent stack.
    /// Designed so that a logger can log the stats
    /// </summary>
    public class StatisticsGatherer
    {
        public ConcurrentQueue<AbstractSimulationInfo> InformationToLog { get; set; } = new ConcurrentQueue<AbstractSimulationInfo>();

        public void HandleInfo(AbstractSimulationInfo eventInfo) {
            InformationToLog.Enqueue(eventInfo);
        }
    }
}
