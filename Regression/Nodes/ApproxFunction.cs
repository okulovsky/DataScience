using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class ApproxFunction : Node
    {
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

        public List<Tuple<double, double>> Table;


        public override double Compute(Indexer inputs, Indexer weights)
        {
            var prevIndex = -1;
            var nextIndex = -1;
            for (int i = 0; i < Table.Count; i++)
            {
                if (Table[i].Item1 < inputs[0])
                {
                    prevIndex = i;
                }
                else
                {
                    nextIndex = i;
                    break;
                }
            }
            if (prevIndex==-1)
            {
                prevIndex = nextIndex;
                nextIndex++;
            }
            if (nextIndex == -1)
            {
                nextIndex = prevIndex;
                prevIndex--;
            }

            var prev = Table[prevIndex];
            var next = Table[nextIndex];

            var k = (next.Item2 - prev.Item2) / (next.Item1 - prev.Item1);
            var b = prev.Item2 - k * prev.Item1;
            return k * inputs[0] + b;
        }

        public override void Derivative(double error, Indexer inputs, Indexer weights, Indexer inputsDerivative, Indexer weightsDerivative)
        {
            
        }
    }
}
