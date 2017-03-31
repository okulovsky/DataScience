using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class NeuralNetwork: Network
    {
        public static Node[] Build(Network network, bool makeOutputs, Random rnd, Node[] inputs, IActivationFunction function, params int[] layers)
        {
          
            for (int i = 0; i < layers.Length; i++)
            {
                bool output = i == layers.Length - 1;
                var l = Enumerable
                    .Range(0, layers[i])
                    .Select(z => network.CreateNode(()=>new Neuron(function), !makeOutputs || !output, inputs))
                    .Cast<Node>()
                    .ToArray();


                for (int j = 0; j < l.Length; j++)
                    for (int k = 0; k < l[j].Weights.Count; k++)
                        l[j].Weights[k] = rnd.NextDouble() * 2 - 1;

                inputs = l;
            }
            return inputs;
        }

        public NeuralNetwork(IActivationFunction function, int inputCount, params int[] layers)
        {
            var previousLayer = Enumerable
                .Range(0, inputCount)
                .Select(z => CreateInputNode())
                .Cast<Node>()
                .ToArray();
            Build(this, true, new Random(), previousLayer, function, layers);
        }
    }
}
