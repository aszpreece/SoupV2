using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Statistics
{

    public class BrainReport: AbstractSimulationInfo
    {
       public Vector2 Position { get; set; }

       public float Rotation { get; set; } 
       public Dictionary<string, float> Outputs { get; set; }
       public Dictionary<string, float> Inputs { get; set; }
    }

    /// <summary>
    /// Gathers statistics on entities with brains.
    /// </summary>
    public class BrainReporter : EntitySystem
    {
        private StatisticsGatherer _gatherer;
        public BrainReporter(EntityPool pool, StatisticsGatherer gatherer) : base(pool, (e) => e.HasComponents(typeof(BrainComponent)))
        {
            _gatherer = gatherer;

        }

        public void Update()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var brain = Compatible[i].GetComponent<BrainComponent>();

                Dictionary<string, float> inputs = new Dictionary<string, float>();
                foreach(string name in brain.InputMap.Values)
                {
                    float value = brain.Brain.GetInput(name);
                    inputs.Add(name, value);
                }

                Dictionary<string, float> outputs = new Dictionary<string, float>();
                foreach (string name in brain.OutputMap.Values)
                {
                    float value = brain.Brain.GetOutput(name);
                    inputs.Add(name, value);
                }

                BrainReport report = new BrainReport()
                {
                    Inputs = inputs,
                    Outputs = outputs,
                    Position = transform.WorldPosition,
                    Rotation = transform.WorldRotation.Theta,
                    TimeStamp = 0
                };

                _gatherer.InformationToLog.Enqueue(report);

            }
        }
    }
}
