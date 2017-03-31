using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public interface IActivationFunction
    {
        double Function(double x);
        double Derivative(double x);
    }


    public class Tanh : IActivationFunction
    {
        public double Beta;

        public Tanh(double Beta = 1) { this.Beta = Beta; }

        public double Function(double x)
        {
            return Math.Tanh(x * Beta);
        }

        public double Derivative(double x)
        {
            var f = Function(x);
            return Beta * (1 - f * f);
        }

    }

    public class Sigmoid : IActivationFunction
    {
        public double Beta = 1;

        public double Derivative(double x)
        {
            return Beta * Math.Exp(Beta * x) / Math.Pow(1+Math.Exp(Beta * x), 2);
        }

        public double Function(double x)
        {
            return 1 / (1 + Math.Exp(-Beta*x));
        }
    }
}
