using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public interface IVariableSizeNode
    {
        void SetInputsCount(int inputsCount);
    }
}
