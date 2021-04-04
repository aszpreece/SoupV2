using EntityComponentSystem;
using Newtonsoft.Json;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class MovementControlComponent : AbstractComponent
    {

        public MovementControlComponent(Entity owner) : base(owner)
        {

        }
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
        public float WishForceForward
        {
             get;
            [Control]
            set; } = 0.0f;


        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum rotational force that is wished to be exterted.
        /// </summary>
        [Control]
        public float WishRotForce { 
            get;
            [Control]
            set; 
        } = 0.0f;

        /// <summary>
        /// Returns the total forward force exterted.
        /// Same as WishForceForward * MaxMovementForceNewtons
        /// </summary>
        [JsonIgnore]
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
        [JsonIgnore]
        public float RotationForce
        {
            get
            {
                return MaxRotationForceNewtons * WishRotForce;
            }
        }
    }
}
