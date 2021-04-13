using SoupV2.NEAT;
using SoupV2.NEAT.mutation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation
{

    [DefaultPropertyAttribute("WorldHeight")]
    public class SimulationSettings
    {
        [CategoryAttribute("World Dimensions"), DescriptionAttribute("The height of the world in world units.")]
        [DisplayName("World Height")]
        public int WorldHeight { get; set; } = 4000;
        [CategoryAttribute("World Dimensions"), DescriptionAttribute("The width of the world in world units.")]
        [DisplayName("World Width")]
        public int WorldWidth { get; set; } = 4000;

        [CategoryAttribute("Physics"), DescriptionAttribute("The mass desnity of the fluid in the world. Higher values will make it more difficult for physics objects to move.")]
        [DisplayName("Mass Density")]
        public float MassDensity { get; set; } = 1.2f;


        [CategoryAttribute("Food"), DescriptionAttribute("The entity definition for the food objects that will be respawned.")]
        [DisplayName("Food Object")]
        public string FoodObjectName { get; set; } = "Food";

        [CategoryAttribute("Ageing"), DescriptionAttribute("Whether or not aging is enabled. If enabled entities with an aging component will die after their age timer tuns out.")]
        [DisplayName("Enable Old Age")]
        public bool OldAgeEnabled { get; set; } = true;
        [CategoryAttribute("Ageing"), DescriptionAttribute("Slows down or speed up aging.")]
        [DisplayName("Old Age Speed Multiplier")]
        public float OldAgeMultiplier { get; set; } = 1.0f;

        //[CategoryAttribute("Mutation"), DescriptionAttribute("Slows down or speed up aging.")]
        //[DisplayName("Mutation Config")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public NeatMutationConfig MutationConfig { get; set; } = new NeatMutationConfig();

        [CategoryAttribute("Reproduction"), DescriptionAttribute("Defines the compatibility for young to be placed into their parent's species, or create a new one. Lower values mean more unique species.")]
        [DisplayName("Species Compatability Threshold")]
        public float SpeciesCompatabilityThreshold { get; set; } = 0.3f;

        public List<CritterTypeSetting> CritterTypes { get; set; } = new List<CritterTypeSetting>()
        {
            new CritterTypeSetting()
            {
                DefinitionName= "Critterling",
                InitialCount = 150,
                MinimumCount = 0
            }
        };
    }
}
