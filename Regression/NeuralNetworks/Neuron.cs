using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Neuron : Node, IVariableSizeNode
    {
        int inputsCount;
        public IActivationFunction Function = new Tanh();

        public Neuron(IActivationFunction function)
        {
            this.Function = function;
        }

        public void SetInputsCount(int inputsCount)
        {
            this.inputsCount = inputsCount;
        }

        public override int InputsCount
        {
            get
            {
                return inputsCount;
            }
        }

        public override int WeightsCount
        {
            get
            {
                return inputsCount + 1;
            }
        }

        public override double Compute(Indexer inputs, Indexer weights)
        {
            var sum = weights[inputsCount];
            for (int i = 0; i < inputsCount; i++)
                sum += inputs[i] * weights[i];
            return Function.Function(sum);
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            var sum = weights[inputsCount];
            for (int i = 0; i < inputsCount; i++)
                sum += inputs[i] * weights[i];

            var d = Function.Derivative(sum);
            if (double.IsNaN(d))
                throw new Exception();
            weightsDerivative[inputsCount] += error * d;
            for (int i=0;i<inputsCount;i++)
            {
                weightsDerivative[i] += error * d * inputs[i];
                inputsDerivative[i] += error * d * weights[i];
            }
        }
    }
}
