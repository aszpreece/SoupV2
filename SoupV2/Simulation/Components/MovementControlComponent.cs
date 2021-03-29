using EntityComponentSystem;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class MovementControlComponent : IComponent
    {

        /// <summary>
        /// The maximum amount of movement force that can be exterted.
        /// </summary>
        public float MaxMovementForceNewtons { get; set; } = 1.0f;
        /// <summary>
        /// The maximum amount of rotational force that can be exterted.
        /// </summary>
        public float MaxRotationForceNewtons { get; set; } = 1.0f;

        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum movement force that is wished to be exterted.
        /// </summary>
        [Control]
        public float WishForceForward { get; set; } = 0.0f;


        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum rotational force that is wished to be exterted.
        /// </summary>
        [Control]
        public float WishRotForce { get; set; } = 0.0f;

        /// <summary>
        /// Returns the total forward force exterted.
        /// Same as WishForceForward * MaxMovementForceNewtons
        /// </summary>
        public float ForwardForce
        {
            get
            {
                return MaxMovementForceNewtons * WishForceForward;
            }
        }

        /// <summary>
        /// Returns the total rotational force exterted.
        /// Same as WishRotForce * MaxRotationForceNewtons
        /// </summary>
        public float RotationForce
        {
            get
            {
                return MaxRotationForceNewtons * WishRotForce;
            }
        }
    }
}
