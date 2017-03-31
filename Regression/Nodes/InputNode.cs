using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class InputNode : Node
    {
        public override int InputsCount
        {
            get
            {
                return 0;
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
            throw new NotImplementedException();
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            throw new NotImplementedException();
        }
    }
}
