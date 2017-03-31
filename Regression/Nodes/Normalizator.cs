using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class FixedAffineTransform : Node
    {
        public double Multiplier = 1;
        public double ShiftBeforeMultiplication;
        public double ShiftAfterMultuplication;

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
            return ShiftAfterMultuplication + Multiplier * (ShiftBeforeMultiplication + inputs[0]);
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            inputsDerivative[0] += error/Multiplier;
        }
    }
}
