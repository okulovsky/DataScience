using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public static class Examples
    {
        public static AbstractAlgorithm NeuralNetworkTanhRegression()
        {
            return new BasicRegressionAlgorithm
            {
                XMin = 0,
                XMax = 1,
                FunctionToRegress = z => Math.Sin(30 * z) * z,
                Network = new NeuralNetwork(new Tanh(),1, 10, 1),
                Teacher = new Teacher
                {
                    Rate = 0.1,
                    Repetitions = 5,
                }
            };
        }

        public static AbstractAlgorithm NeuralNetworkSigmoidRegression()
        {
            var net = new NeuralNetwork(new Sigmoid(),1, 10, 1);
            
            return new BasicRegressionAlgorithm
            {
                XMin = 0,
                XMax = 1,
                FunctionToRegress = z => z * (Math.Sin(30 * z) + 1) / 2,
                Network = net,
                Teacher = new Teacher
                {
                    Rate = 0.1,
                    Repetitions = 5,
                }
            };
        }

    }
}
