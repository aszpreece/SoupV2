using EntityComponentSystem;
using Newtonsoft.Json;
using SoupV2.Simulation.Brain;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{

    public enum MovementMode
    {
        MoveAndSteer,
        TwoWheels
    }

    public class MovementControlComponent : AbstractComponent
    {

        public MovementControlComponent(Entity owner) : base(owner)
        {

        }

        public MovementMode MovementMode { get; set; } = MovementMode.MoveAndSteer;

        private float _maxMovementForceNewtons = 1.0f;
        /// <summary>
        /// The maximum amount of movement force that can be exterted.
        /// </summary>
        public float MaxMovementForceNewtons {
            get => _maxMovementForceNewtons;
            set
            {
                if (value >= 0)
                {
                    _maxMovementForceNewtons = value;
                }
            }
        }

        private float _maxRotationForceNewtons = 1.0f;

        /// <summary>
        /// The maximum amount of rotational force that can be exterted.
        /// </summary>
        public float MaxRotationForceNewtons {
            get => _maxRotationForceNewtons;
            set
            {
                if (value >= 0)
                {
                    _maxRotationForceNewtons = value;
                }
            }
        }

        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum movement force that is wished to be exterted.
        /// </summary>
        [Browsable(false)]
        [Control]
        public float WishForceForward
        {
             get;
            set; } = 0.0f;


        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum rotational force that is wished to be exterted.
        /// </summary>
        [Control]
        [Browsable(false)]
        public float WishRotForce { 
            get;
            set; 
        } = 0.0f;

        /// <summary>
        /// Returns the total forward force exterted.
        /// Same as WishForceForward * MaxMovementForceNewtons
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
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
        [Browsable(false)]
        public float RotationForce
        {
            get
            {
                return MaxRotationForceNewtons * WishRotForce;
            }
        }



        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum movement force that is wished to be exterted by the left wheel.
        /// </summary>
        [Browsable(false)]
        [Control]
        public float WishWheelLeftForce
        {
            get;
            set;
        } = 0.0f;


        /// <summary>
        /// The percentage (represented as a number between -1 and 1) of the maximum movement force that is wished to be exterted by the right wheel.
        /// </summary>
        [Browsable(false)]
        [Control]
        public float WishWheelRightForce
        {
            get;
            set;
        } = 0.0f;



        /// <summary>
        /// The same as MaxMovementForceNewtons / 2 * WishWheelLeftForce
        /// </summary>
        [Browsable(false)]
        [Control]
        public float LeftWheelForce
        {
            get => WishWheelLeftForce * MaxMovementForceNewtons * 0.5f;
        }


        /// <summary>
        /// The same as MaxMovementForceNewtons / 2 * WishWheelRightForce
        /// </summary>
        [Browsable(false)]
        [Control]
        public float RightWheelForce
        {
            get => WishWheelRightForce * MaxMovementForceNewtons * 0.5f;
        }

    }
}
