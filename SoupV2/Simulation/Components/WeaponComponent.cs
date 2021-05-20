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
    class WeaponComponent : AbstractComponent
    {

        /// <summary>
        /// The health damage this weapon does on contact per second
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Signals if the wepaon is active. 0 = off 1 = on.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public float Active { get; set; }

        /// <summary>
        /// The cooldown between attacks for this weapon
        /// </summary>
        public float CooldownTimeSeconds { get; set; }

        /// <summary>
        /// The cooldown left before this weapon can attack again
        /// </summary>
        [JsonIgnore]
        public float CooldownLeftSeconds { get; set; }

        /// <summary>
        /// The amount of time this weapon is active and causing damage for.
        /// </summary>
        public float AttackTimeSeconds { get; set; }

        /// <summary>
        /// The amount of time this weapon will still be active for.
        /// </summary>
        [JsonIgnore]
        public float AttackTimeLeft { get; set; }


        /// <summary>
        /// Controls if this weapon should attack. Will attempt to attack if greater than the activate threshold.
        /// </summary>
        [Control]
        [JsonIgnore]
        [Browsable(false)]
        public float Activation { get; set; }

        /// <summary>
        /// The amount of activation needed for the weapon to attack.
        /// </summary>
        public float ActivateThreshold { get; set; }

        /// <summary>
        /// The cost in energy of performing this attack.
        /// </summary>
        public float AttackCost { get; set; }


        /// <summary>
        /// Indicates whether an attack hit or not.
        /// </summary>
        [Input]
        public float Hit { get; set; } = 0.0f;

        /// <summary>
        /// Controls if the weapon should siphon energy from the victim
        /// </summary>
        public bool SiphonEnergy { get; set; } = false;

        /// <summary>
        /// If siphon enabled, the amount of energy to take
        /// </summary>
        public float SiphonAmount { get; set; } = 0.0f;

        public WeaponComponent(Entity owner) : base(owner)
        {

        }

    }
}
