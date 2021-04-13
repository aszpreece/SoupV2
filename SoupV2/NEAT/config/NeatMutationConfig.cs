using SoupV2.NEAT.mutation;

namespace SoupV2.NEAT
{
    public class NeatMutationConfig
    {

        public AddConnectionMutator AddConnectionMutator { get; set; } = new AddConnectionMutator()
        {
            AllowRecurrentConns = true,
            NewWeightPower = 1.0f,
            NewWeightStd = 1.0f,
            ProbabiltiyOfMutation = 0.02f

        };
        public SplitConnectionMutator SplitConnectionMutator { get; set; } = new SplitConnectionMutator()
        {
            ProbabiltiyOfMutation = 0.02f,
        };
        public ConnectionWeightMutator ConnectionWeightMutator { get; set; } = new ConnectionWeightMutator()
        {
            ProbabiltiyOfMutation = 1f,
            ProbPerturbWeight = 0.8f,
            NewWeightStd = 1f,
            ProbResetWeight = 0.1f,
            NewWeightPower = 1f,
            ProbEnableConn = 0.01f,
            ProbDisableConn = 0.01f,
            WeightPerturbScale = 0.4f
        };
    }
}