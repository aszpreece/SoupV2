using SoupV2.NEAT;
using SoupV2.NEAT.mutation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Simulation.Settings
{

    [DefaultPropertyAttribute("WorldHeight")]
    public class SimulationSettings
    {

        private float _simSpeedMultiplier = 1f;
        [Category("Simulation"), Description("Makes the simulation fun faster or slower")]
        [DisplayName("Simulation Speed Multiplier")]
        public float SimSpeedMultiplier {
            get => _simSpeedMultiplier;
            set
            {
                if (value > 0)
                {
                    _simSpeedMultiplier = value;
                }
            }
        
        } 

        [Category("World Dimensions"), Description("The height of the world in world units.")]
        [DisplayName("World Height")]
        public int WorldHeight { get; set; } = 4000;
        [Category("World Dimensions"), Description("The width of the world in world units.")]
        [DisplayName("World Width")]
        public int WorldWidth { get; set; } = 4000;

        [Category("Physics"), Description("The mass desnity of the fluid in the world. Higher values will make it more difficult for physics objects to move.")]
        [DisplayName("Mass Density")]
        public float MassDensity { get; set; } = 1.2f;


        [Category("Ageing"), Description("Whether or not aging is enabled. If enabled entities with an aging component will die after their age timer tuns out.")]
        [DisplayName("Enable Old Age")]
        public bool OldAgeEnabled { get; set; } = true;
        [Category("Ageing"), Description("Slows down or speed up aging.")]
        [DisplayName("Old Age Speed Multiplier")]
        public float OldAgeMultiplier { get; set; } = 1.0f;

        [Category("Mutation"), Description("Slows down or speed up aging.")]
        [DisplayName("Mutation Config")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public NeatMutationConfig MutationConfig { get; set; } = new NeatMutationConfig();

        [Category("Reproduction"), Description("Defines the compatibility for young to be placed into their parent's species, or create a new one. Lower values mean more unique species.")]
        [DisplayName("Species Compatability Threshold")]
        public float SpeciesCompatabilityThreshold { get; set; } = 0.3f;
        public List<CritterTypeSetting> CritterTypes { get; set; } = new List<CritterTypeSetting>();


        public List<FoodTypeSetting> FoodTypes { get; set; } = new List<FoodTypeSetting>();

        [Category("World"), Description("The amount of spare energy the world energy manager has.")]
        [DisplayName("Initial World Energy")]
        public float InitialWorldEnergy { get; set; } = 30f;

        [Category("Statistics"), Description("The interval for critter positions to be reported")]
        [DisplayName("Critter Position Report Interval")]
        public float CritterPositionReportInterval { get; set; } = 30f;

        [Category("Statistics"), Description("The interval for food positions to be reported")]
        [DisplayName("Food Position Report Interval")]
        public float FoodPositionReportInterval { get;  set; } = 30f;

        [Category("Statistics"), Description("The interval for critter colours to be reported")]
        [DisplayName("Visible Colour Info Reporter Interval")]
        public float VisibleColourInfoReporterInterval { get;  set; } = 30f;
    }
}
