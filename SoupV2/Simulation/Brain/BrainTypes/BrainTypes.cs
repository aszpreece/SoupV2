using SoupV2.NEAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Brain.BrainTypes
{
    public enum BrainType
    {
        NEAT,
        HunterScript
    }

    public static class BrainTypes
    {
        public static readonly Dictionary<BrainType, Type> BrainTypeMap = new Dictionary<BrainType, Type>()
        {
            {
                BrainType.NEAT, typeof(NeatBrainGenotype) 
            },
            {
                BrainType.HunterScript, typeof(HunterScriptBrainGenotype)
            }
    
        };
    }
}
