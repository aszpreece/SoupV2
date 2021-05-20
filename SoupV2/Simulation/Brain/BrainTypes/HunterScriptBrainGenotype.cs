using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Brain.BrainTypes
{
    public class HunterScriptBrainGenotype : AbstractBrainGenotype
    {

        public HashSet<string> Inputs { get; internal set; }
        public HashSet<string> Outputs { get; internal set; }

        public override object Clone()
        {
            return new HunterScriptBrainGenotype();
        }

        public override float CompareSimilarity(AbstractBrainGenotype other)
        {
            return 0;
        }

        public override void CreateBrain(BrainComponent brainComponent, IMutatorConfig config)
        {
            Inputs = new();
            Outputs = new();

            foreach (string input in brainComponent.InputMap.Values)
            {
                Inputs.Add(input);
            }
            foreach (string output in brainComponent.OutputMap.Values)
            {
                Outputs.Add(output);
            }
        }

        public override AbstractBrain GetBrain()
        {
            return new HunterScriptBrainPhenotype(this);
        }
    }
}
