using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
   


    public class Network
    {
        List<double> inputs = new List<double>();
        List<double> weigts = new List<double>();
        List<double> inputsDerivative = new List<double>();
        List<double> weightsDerivative = new List<double>();
        List<NodeData> Nodes = new List<NodeData>();
        List<int> outputIndex = new List<int>();
        List<int> inputIndex = new List<int>();

        public IEnumerable<Node> AllNodes { get { return Nodes.Select(z => z.Node); } }

        public ListDecorator Weights { get; private set; }

        public Network()
        {
            Weights = new ListDecorator(weigts);
        }

        public int InputSize { get; private set; }
       
        public InputNode CreateInputNode()
        {
            inputs.Add(0);
            inputsDerivative.Add(0);
            inputIndex.Add(inputs.Count-1);
            InputSize++;
            return new InputNode { Index = inputs.Count - 1 };
        }


        public T CreateNode<T>(T node, bool isHidden, params Node[] inputs)
            where T : Node
        {
            if (node is IVariableSizeNode)
                ((IVariableSizeNode)node).SetInputsCount(inputs.Length);
            AddNode(node, isHidden, inputs);
            return node;
        }

        public T CreateNode<T>(Func<T> factory, bool isHidden, params Node[] inputs)
            where T : Node
        {
            return CreateNode(factory(), isHidden, inputs);
        }

        public T CreateNode<T>(bool isHidden, params Node[] inputs)
            where T : Node, new()
        {
            return CreateNode(new T(), isHidden, inputs);
        }

        public T CreateNode<T>(params Node[] inputs)
            where T : Node, new()
        {
            return CreateNode<T>(true, inputs);
        }

        public T CreateOutput<T>(params Node[] inputs)
            where T :Node,new()
        {
            return CreateNode<T>(false, inputs);
        }
        

        public void MakeOutput(Node node)
        {
            outputIndex.Add(node.Index);
        }

        private void AddNode(Node node, bool isHidden, params Node[] parents)
        {
            if (parents.Length != node.InputsCount)
                throw new ArgumentException();
            var inputIndices = parents.Select(z => z.Index).ToList();

            node.Index = inputs.Count;
            node.Network = this;
            inputs.Add(0);
            inputsDerivative.Add(0);

            if (!isHidden)
                outputIndex.Add(node.Index);

            var weightsIndices = new List<int>();
            for (int i=0;i<node.WeightsCount;i++)
            {
                weightsIndices.Add(weigts.Count);
                weigts.Add(0);
                weightsDerivative.Add(0);
            }

            var data = new NodeData();
            data.Node = node;
            data.OutputIndex= node.Index;
            data.Input = new Indexer(inputIndices, inputs);
            data.InputDerivative = new Indexer(inputIndices, inputsDerivative);
            data.Weights = new Indexer(weightsIndices, weigts);
            data.WeightsDerivative = new Indexer(weightsIndices, weightsDerivative);
            node.Weights = data.Weights;
            Nodes.Add(data);
        }
        
        public double[] Compute(double[] input)
        {
            if (input.Length != InputSize)
                throw new ArgumentException();
            for (int i = 0; i < InputSize; i++)
                inputs[inputIndex[i]] = input[i];
            for (int i=0;i<Nodes.Count;i++)
            {
                var result = Nodes[i].Node.Compute(Nodes[i].Input, Nodes[i].Weights);
                inputs[Nodes[i].OutputIndex] = result;
            }
            double[] output = new double[outputIndex.Count];
            for (int i = 0; i < outputIndex.Count; i++)
                output[i] = inputs[outputIndex[i]];
            return output;
        }

        public class Derivatives
        {
            public double[] OnWeights;
            public double[] OnInputs;
        }


        public Derivatives ComputeDerivative(double[] error)
        {
            if (error.Length != outputIndex.Count)
                throw new ArgumentException();

            for (int i = 0; i < inputsDerivative.Count; i++)
                inputsDerivative[i] = 0;
            for (int i = 0; i < weightsDerivative.Count; i++)
                weightsDerivative[i] = 0;

            for (int i = 0; i < outputIndex.Count; i++)
                inputsDerivative[outputIndex[i]] = error[i];




            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                Nodes[i].Node.Derivative(
                    inputsDerivative[Nodes[i].Node.Index],
                    Nodes[i].Input,
                    Nodes[i].Weights,
                    Nodes[i].InputDerivative,
                    Nodes[i].WeightsDerivative);
                if (Nodes[i].WeightsDerivative.Any(z => double.IsNaN(z)))
                    throw new Exception();
                if (Nodes[i].InputDerivative.Any(z => double.IsNaN(z)))
                    throw new Exception();
            }
            return new Derivatives
            {
                OnInputs = inputsDerivative.ToArray(),
                OnWeights = weightsDerivative.ToArray()
            };

        }

        public double ComputeSimpleFunction(double x)
        {
            return Compute(new[] { x })[0];
        }
    }
}
