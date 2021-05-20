using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT
{
    public enum ActivationFunctionType
    {
        SIGMOID,
        TANH,
        SIGN,
        RELU,
        SOFTSIGN,
        PROBABILITY
    }
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

        /// <summary>
        /// Passes input through a sigmoid and treats that as a probability, outputting -1 or 1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// 

        public static double Probability(double x)
        {
            return Convert.ToSingle(StaticRandom.NextDouble() < Sigmoid(x)) * 2 - 1;
        }

        /// <summary>
        /// Applies some normal noise to soft sign.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double NoisySoftSign(double x)
        {
            if (x < 0)
            {
                return (2 * (Math.Exp(x) / (1 + Math.Exp(x))) - 1) + StaticRandom.Normal(0, 0.2);
            }
            else
            {
                return (2 / (1 + Math.Exp(-x)) - 1) + StaticRandom.Normal(0, 0.2);
            }
        }

        /// <summary>
        /// Noisy soft sign but clamped to [-1, 1]
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double NoisySoftSignClamp(double x)
        {
            return Math.Clamp(NoisySoftSign(x), -1, 1);
        }

        public readonly static Dictionary<ActivationFunctionType, Func<double, double>> Functions = new Dictionary<ActivationFunctionType, Func<double, double>>() {
            {ActivationFunctionType.SIGMOID, Sigmoid },
            {ActivationFunctionType.TANH, Math.Tanh },
            {ActivationFunctionType.SIGN, Sign },
            {ActivationFunctionType.RELU, Relu },
            {ActivationFunctionType.SOFTSIGN, Softsign },
            {ActivationFunctionType.PROBABILITY, Probability }
        };
    }
}
