using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Brain
{
    public class Species
    {
        /// <summary>
        /// Unique Id of the species
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Timestamp for when the species was created in seconds.
        /// </summary>
        public double TimeCreated { get; set; }

        /// <summary>
        /// The representative of this species.
        /// </summary>
        private AbstractGenotype Representative { get; set; };

    }
}
