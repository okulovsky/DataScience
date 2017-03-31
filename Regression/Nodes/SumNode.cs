using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Sum : Node, IVariableSizeNode
    {
        int inputsCount;

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
                return 0;
            }
        }

        public override double Compute(Indexer inputs, Indexer weights)
        {
            return inputs.Sum();
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            for (int i = 0; i < InputsCount; i++)
                inputsDerivative[i] += error;
        }
    }
}
