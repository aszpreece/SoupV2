using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT
{

    static class ActivationFunctions
    {
        public static double Sigmoid(double x)
        {
            if (x < 0)
            {
                return Math.Exp(x) / (1 + Math.Exp(x));
            }
            else
            {
                return 1 / (1 + Math.Exp(-x));
            }
        }

        // Just sigmoid but centred around 0
        public static double Softsign(double x)
        {
            if (x < 0)
            {
                return 2 * (Math.Exp(x) / (1 + Math.Exp(x))) - 1;
            }
            else
            {
                return 2 / (1 + Math.Exp(-x)) - 1;
            }
        }

        public static double Sign(double x)
        {
            if (x > 0)
            {
                return 1.0d;
            } else
            {
                return 0.0d;
            }
        }


        public static double Relu(double x)
        {
            return x > 0 ? x : 0.0;
        }


        public readonly static Dictionary<string, Func<double, double>> Functions = new Dictionary<string, Func<double, double>>() {
            { "sigmoid", Sigmoid },
            {"tanh", Math.Tanh },
            {"sign", Sign },
            {"relu", Relu },
            {"softsign", Softsign }
        };
    }
}
