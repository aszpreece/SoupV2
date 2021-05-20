using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using System.ComponentModel;

namespace SoupV2.NEAT
{

    public enum OutputActivationFunction
    {
        SOFTSIGN = ActivationFunctionType.SOFTSIGN,
        PROBABLITY = ActivationFunctionType.PROBABILITY,
        TANH =  ActivationFunctionType.TANH
    }
    public class NeatMutationConfig : IMutatorConfig
    {
        public OutputActivationFunction OutputActivationFunction { get; set; } = OutputActivationFunction.SOFTSIGN;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AddConnectionMutator AddConnectionMutator { get; set; } = new AddConnectionMutator()
        {
            AllowRecurrentConns = true,
            NewWeightPower = 1.0f,
            NewWeightStd = 1.0f,
            ProbabiltiyOfMutation = 0.02f

        };
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SplitConnectionMutator SplitConnectionMutator { get; set; } = new SplitConnectionMutator()
        {
            ProbabiltiyOfMutation = 0.02f,
        };
        [TypeConverter(typeof(ExpandableObjectConverter))]
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