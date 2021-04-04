using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SoupV2;
using SoupV2.NEAT.mutation;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class GenotypeTest
    {
        [TestMethod]
        public void TestSerialization()
        {

            var mutators = new List<AbstractMutator> {
                new ConnectionWeightMutator() {
                    ProbabiltiyOfMutation = 1f,
                    ProbPerturbWeight = 1.0f, 
                    NewWeightStd = 0.4f, 
                    ProbResetWeight=1.0f, 
                    NewWeightPower = 1.0f, 
                    ProbEnableConn =0.5f,
                    ProbDisableConn = 0.8f,
                    WeightPerturbScale = 0.2f
                },
                new SplitConnectionMutator() {
                    ProbabiltiyOfMutation=0.02f,
                    
                }
            };

            MutationConfig config = new MutationConfig() { Mutators = mutators };
            
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var json = JsonConvert.SerializeObject(config, settings);

            MutationConfig deserializedConfig = JsonConvert.DeserializeObject<MutationConfig>(json, settings);

            Assert.IsTrue(deserializedConfig.Mutators[0] is ConnectionWeightMutator);
            Assert.IsTrue(deserializedConfig.Mutators[1] is SplitConnectionMutator);

        }
    }
}
