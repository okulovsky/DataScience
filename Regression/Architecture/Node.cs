using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public abstract class Node
    {
        public abstract int InputsCount { get; }
        public abstract int WeightsCount { get; }
        public abstract double Compute(Indexer inputs, Indexer weights);
        public abstract void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative);
        public int Index { get; internal set; }
        internal protected Indexer Weights { get; set; }
        internal protected Network Network { get; set; }

        public void MakeOutput()
        {
            Network.MakeOutput(this);
        }

    }
}
