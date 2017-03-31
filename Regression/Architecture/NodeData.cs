using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    class NodeData
    {
        public Node Node;
        public Indexer Input;
        public Indexer Weights;
        public Indexer InputDerivative;
        public Indexer WeightsDerivative;
        public int OutputIndex;
    }
}
