using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Regression
{
    public class Teacher
    {
        public double Rate = 0.1;
        public int Repetitions = 1;
        public double Momentum = 0;//0.2;
        public double Regularization = 0;//1e-10;
        Random rnd = new Random();

        Dictionary<Network, double[]> momentums = new Dictionary<Network, double[]>();


        double ComputeError(Network network, double[] inputs, double[] answers, out double[] delta)
        {
            var output = network.Compute(inputs);
            delta = new double[output.Length];
            for (int i = 0; i < output.Length; i++)
                delta[i] = 2 * (answers[i] - output[i]);
            return delta.Select(z => Math.Pow(z / 2, 2)).Sum();
        }

        double GetTotalError(Network network, double[][] inputs, double[][] answers)
        {
            double[] delta;
            var error = 0.0;
            for (int i = 0; i < inputs.Length; i++)
                error += ComputeError(network, inputs[i], answers[i], out delta);
            return error / inputs.Length;
        }

        public Tuple<double,double> Iteration(Network Network, double[] inputs, double[] answers)
        {
            if (!momentums.ContainsKey(Network))
                momentums[Network] = new double[Network.Weights.Count];
            var memory = momentums[Network];

            double initialError = 0;

            for (int j = 0; j < Repetitions; j++)
            {
                double[] delta;
                var error= ComputeError(Network, inputs, answers, out delta);
                if (j == 0)
                    initialError = error;
                
                var grad=Network.ComputeDerivative(delta).OnWeights;

                for (int i = 0; i < grad.Length; i++)
                {
                    var add = 
                        + Rate * grad[i] 
                        + Momentum * memory[i] 
                        - Network.Weights[i] * Regularization ;
                    if (double.IsNaN(add))
                        throw new Exception();
                    Network.Weights[i] += add;
                    memory[i] = add;
                }
            }
            double[] delta1;
            var resultingError = ComputeError(Network, inputs, answers, out delta1);
            return Tuple.Create(initialError, resultingError);
        }

        public Tuple<double,double> RandomIteration(Network network, double[][] inputs, double[][] answers)
        {
            var ind = rnd.Next(inputs.Length);
            return Iteration(network, inputs[ind], answers[ind]);
        }

        public List<Tuple<double,double>> RunEpoch(Network network, double[][] inputs, double[][] answers)
        {

            var result = new List<Tuple<double, double>>();

            for (int i = 0; i < inputs.Length; i++)
            {
                result.Add(Iteration(network, inputs[i], answers[i]));
            }

            return result;
        }
        

    }
}
