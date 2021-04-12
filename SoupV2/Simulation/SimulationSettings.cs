using SoupV2.NEAT.mutation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation
{
    public class SimulationSettings
    {
        public int WorldHeight { get; set; } = 4000;
        public int WorldWidth { get; set; } = 4000;
        public float MassDensity { get; set; } = 1.2f;
        public string FoodObjectName { get; set; } = "Food";
        public MutationConfig MutationConfig { get; set; } = new MutationConfig()
        {
            NeatMutators = new List<AbstractMutator> {
                new AddConnectionMutator()
                {
                    AllowRecurrentConns= true,
                    NewWeightPower = 1.0f,
                    NewWeightStd = 1.0f,
                    ProbabiltiyOfMutation = 0.02f

                },
                new SplitConnectionMutator() {
                    ProbabiltiyOfMutation=0.02f,

                },
                new ConnectionWeightMutator() {
                    ProbabiltiyOfMutation = 1f,
                    ProbPerturbWeight = 0.8f,
                    NewWeightStd = 1f,
                    ProbResetWeight=0.1f,
                    NewWeightPower = 1f,
                    ProbEnableConn =0.01f,
                    ProbDisableConn = 0.01f,
                    WeightPerturbScale = 0.4f
                },
            }
        };
        public float SpeciesCompatabilityThreshold { get; set; } = 0.3f;

        public List<CritterTypeSetting> CritterTypes { get; set; } = new List<CritterTypeSetting>()
        {
            new CritterTypeSetting()
            {
                DefinitionName= "Critterling",
                InitialCount = 150,
                MinimumCount = 0
            }
        };
    }
}
