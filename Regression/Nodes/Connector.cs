using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Connector : Node
    {
        public double LastComputedValue { get; private set; }

        public bool DisableLearing { get; set; }

        public override int InputsCount
        {
            get
            {
                return 1;
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
            
            return LastComputedValue = inputs[0];
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            if (!DisableLearing)
                inputsDerivative[0] += error;
        }
    }
}
