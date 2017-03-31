using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Parameter : Node
    {
        
        public bool IsFixed { get; set; }
        public double LearningQ = 1;

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
                return 1;
            }
        }

        public override double Compute(Indexer inputs, Indexer weights)
        {
            return weights[0];
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            if (!IsFixed)
                weightsDerivative[0] += error * LearningQ;
        }

        public double Value { get { return Weights[0]; } set { Weights[0] = value; } }

        public Parameter Assign(double value)
        {
            Value = value;
            return this;
        }

        public Parameter Fix()
        {
            IsFixed = true;
            return this;
        }
    }
}
