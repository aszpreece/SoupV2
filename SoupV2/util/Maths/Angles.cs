using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util.Maths
{
    public static class Angles
    {

        /// <summary>
        /// Calculate the difference between two angles in Radians.
        /// </summary>
        /// <param name="">Angle 1</param>
        /// <param name="">Angle 2</param>
        /// <returns></returns>
        public static float CalculateAngleDiff(float a1, float a2) {
            float diff = a1 - a2;
            return Mod((diff + MathHelper.Pi), MathHelper.TwoPi) - MathHelper.Pi;
        }

        /// <summary>
        /// Calculates the angle between two points.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float AngleBetweenPoints(Vector2 p1, Vector2 p2)
        {
            var diff = p2 - p1;
            return (float)Math.Atan2(diff.Y, diff.X);

        }

        /// <summary>
        /// Takes the true modulo of two floats
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static float Mod(float a, float b)
        {
            return (float)(a - b * Math.Floor(a / b));
        }

    }
}
