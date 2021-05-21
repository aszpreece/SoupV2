using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoupV2.NEAT;
using SoupV2.NEAT.Genes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    [TestClass]
    public class TestPhenotype
    {
        [TestMethod]
        public void TestNamedNodes()
        {
            NeatBrainGenotype test = new NeatBrainGenotype();
            test.AddNamedNode("bias", NodeType.BIAS, ActivationFunctionType.RELU);
            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input2", NodeType.INPUT, ActivationFunctionType.RELU);

            Assert.IsTrue(test.NodeGenes.Count == 3);

            Assert.IsTrue(test.NodeNameMap.ContainsKey("bias"));
            Assert.IsTrue(test.NodeNameMap.ContainsKey("input1"));
            Assert.IsTrue(test.NodeNameMap.ContainsKey("input2"));

        }

        [TestMethod]
        public void TestAdder()
        {
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("hidden1", NodeType.HIDDEN, ActivationFunctionType.RELU);
            test.AddNamedNode("output1", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 1, 1));  // Input to hidden
            test.ConnectionGenes.Add(new ConnectionGene(2, 1, 1, 1, true, true)); // Hidden to hidden (recurrent)
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 2, 1)); //# Hidden to output

            var network = new NeatBrainPhenotype(test);

            network.SetInput("input1", 1.0f);
            Assert.AreEqual(1.0f, network.Get("input1"));
            network.Calculate();
            Assert.AreEqual(0, network.Get("output1"));
            network.SetInput("input1", 2.0f);
            network.Calculate();
            Assert.AreEqual(1, network.Get("output1"));
            network.SetInput("input1", 10.0f);
            network.Calculate();
            Assert.AreEqual(3, network.Get("output1"));
            Assert.AreEqual(13, network.Get("hidden1"));
            Assert.AreEqual(10, network.Get("input1"));


        }

        [TestMethod]
        public void TestRecurrentFromOutput()
        {
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input2", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input3", NodeType.INPUT, ActivationFunctionType.RELU);

            // Hidden genes
            test.NodeGenes.Add(new NodeGene(3, NodeType.HIDDEN, ActivationFunctionType.RELU));
            test.NodeGenes.Add(new NodeGene(4, NodeType.HIDDEN, ActivationFunctionType.RELU));

            // Output genes
            test.AddNamedNode("output", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.NodeGenes.Add(new NodeGene(5, NodeType.OUTPUT, ActivationFunctionType.RELU));


            // Input connections
            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 3, 1));
            test.ConnectionGenes.Add(new ConnectionGene(2, 1, 3, 1));
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 4, 1));
            test.ConnectionGenes.Add(new ConnectionGene(4, 2, 4, 1));
            // Hidden connection
            test.ConnectionGenes.Add(new ConnectionGene(5, 3, 4, 1));
            // The recurrent connection from 4 to 3
            test.ConnectionGenes.Add(new ConnectionGene(6, 4, 3, 1, true, true));
            // Hidden to output
            test.ConnectionGenes.Add(new ConnectionGene(7, 3, 5, 1));
            test.ConnectionGenes.Add(new ConnectionGene(8, 4, 5, 1));
            // The recurrent connection from 5 to 3
            test.ConnectionGenes.Add(new ConnectionGene(9, 5, 3, -1));

            var network = new NeatBrainPhenotype(test);

            network.SetInput("input1", 1.0f);
            network.SetInput("input2", 2.0f);
            network.SetInput("input3", 3.0f);
            network.Calculate();
            Assert.AreEqual(0.0d, network.Get(5));
            network.SetInput("input1", 1.0f);
            network.SetInput("input2", 2.0f);
            network.SetInput("input3", 3.0f);
            network.Calculate();
            Assert.AreEqual(8.0d, network.Get(5));
            network.SetInput("input1", 1.0f);
            network.SetInput("input2", 2.0f);
            network.SetInput("input3", 3.0f);
            network.Calculate();
            Assert.AreEqual(16.0d, network.Get(5));

        }

        [TestMethod]
        public void TestCreatesCycle()
        {
            // Test that a gene already in the genome can be tested fro recurrence
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input2", NodeType.INPUT, ActivationFunctionType.RELU);

            test.AddNamedNode("hidden1", NodeType.HIDDEN, ActivationFunctionType.RELU);
            test.AddNamedNode("hidden2", NodeType.HIDDEN, ActivationFunctionType.RELU);

            test.AddNamedNode("output1", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 2, 1));
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 3, 1));
            test.ConnectionGenes.Add(new ConnectionGene(4, 2, 4, 1));
            test.ConnectionGenes.Add(new ConnectionGene(5, 3, 4, 1));

            var nonRecurrentGene = new ConnectionGene(2, 2, 3, 1);
            Assert.IsFalse(test.CreatesCycle(nonRecurrentGene));
            test.ConnectionGenes.Add(nonRecurrentGene);

            var recurrentGene = new ConnectionGene(2, 3, 2, 1);
            Assert.IsTrue(test.CreatesCycle(recurrentGene));
        }

        [TestMethod]
        public void TestReEnableCreatesCycle()
        {
            // Test that a gene already in the genome can be tested fro recurrence
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("hidden1", NodeType.HIDDEN, ActivationFunctionType.RELU);
            test.AddNamedNode("output1", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 1, 1)); 
            var recurrentGene = new ConnectionGene(2, 1, 1, 1, false, true);
            test.ConnectionGenes.Add(recurrentGene); 
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 2, 1)); 

            // This new gene should not create a cycle
            Assert.IsTrue(test.CreatesCycle(recurrentGene));
        }

        [TestMethod]
        public void TestCreatesCycleIgnoresDisabledConnections()
        {
            // Test that a gene already in the genome can be tested fro recurrence
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input2", NodeType.INPUT, ActivationFunctionType.RELU);

            test.AddNamedNode("hidden1", NodeType.HIDDEN, ActivationFunctionType.RELU);
            test.AddNamedNode("hidden2", NodeType.HIDDEN, ActivationFunctionType.RELU);

            test.AddNamedNode("output1", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 2, 1));
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 3, 1));
            test.ConnectionGenes.Add(new ConnectionGene(4, 2, 4, 1));
            test.ConnectionGenes.Add(new ConnectionGene(5, 3, 4, 1));

            var nonRecurrentGene = new ConnectionGene(2, 2, 3, 1, false);
            Assert.IsFalse(test.CreatesCycle(nonRecurrentGene));
            test.ConnectionGenes.Add(nonRecurrentGene);

            var recurrentGene = new ConnectionGene(2, 3, 2, 1);
            Assert.IsFalse(test.CreatesCycle(recurrentGene));
        }


        [TestMethod]
        public void TestCreatesCycleIgnoresRecurrentConnections()
        {
            // Test that a gene already in the genome can be tested fro recurrence
            NeatBrainGenotype test = new NeatBrainGenotype();

            test.AddNamedNode("input1", NodeType.INPUT, ActivationFunctionType.RELU);
            test.AddNamedNode("input2", NodeType.INPUT, ActivationFunctionType.RELU);

            test.AddNamedNode("hidden1", NodeType.HIDDEN, ActivationFunctionType.RELU);
            test.AddNamedNode("hidden2", NodeType.HIDDEN, ActivationFunctionType.RELU);

            test.AddNamedNode("output1", NodeType.OUTPUT, ActivationFunctionType.RELU);

            test.ConnectionGenes.Add(new ConnectionGene(1, 0, 2, 1));
            test.ConnectionGenes.Add(new ConnectionGene(3, 1, 3, 1));
            test.ConnectionGenes.Add(new ConnectionGene(4, 2, 4, 1));
            test.ConnectionGenes.Add(new ConnectionGene(5, 3, 4, 1));

            var nonRecurrentGene = new ConnectionGene(2, 2, 3, 1, true, true);
            Assert.IsFalse(test.CreatesCycle(nonRecurrentGene));
            test.ConnectionGenes.Add(nonRecurrentGene);

            var recurrentGene = new ConnectionGene(2, 3, 2, 1);
            Assert.IsFalse(test.CreatesCycle(recurrentGene));
        }
    }
}
