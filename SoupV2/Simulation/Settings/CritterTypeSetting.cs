﻿using Newtonsoft.Json;
using SoupV2.NEAT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Settings
{
    public class CritterTypeSetting : AbstractEntityTypeSetting
    {
        /// <summary>
        /// If this critter has a brain, this is the type of the brain.
        /// </summary>
        public Type BrainType = typeof(NeatBrainGenotype);
    }
}
