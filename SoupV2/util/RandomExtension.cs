using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util
{
    public static class RandomExtension
    {
        public static double Normal(this Random random, double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }

        /// <summary>
        /// Returns a shallow-copied shuffled version of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <param name="list">List to shuffle</param>
        /// <returns></returns>
        public static List<T> Shuffle<T> (this Random random, List<T> list)
        {
            var newList = new List<T>(list);

            for (int i = 0; i < newList.Count; i++)
            {
                int randomIndex = random.Next(newList.Count);
                T old = newList[randomIndex];
                newList[randomIndex] = newList[i];
                newList[i] = old;
            }

            return newList;
        }

    }
}
