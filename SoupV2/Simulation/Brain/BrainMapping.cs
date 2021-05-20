using EntityComponentSystem;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Brain
{
    public static class BrainMapping
    {

        public static readonly List<string> SpecialInputs = new List<string>()
        {
            "Bias",
            "Random"
        };

        /// <summary>
        /// Checks if all the inputs and outputs for a brain are valid and returns invalid ones
        /// </summary>
        /// <param name="brain"></param>
        /// <param name="entity"></param>
        /// <param name="invalidInputs"></param>
        /// <param name="invalidControls"></param>
        /// <returns></returns>
        public static bool ValidateBrain(BrainComponent brain, Entity entity, out List<string> invalidInputs, out List<string> invalidControls)
        {
            invalidInputs = new List<string>();
            invalidControls = new List<string>();

            var validInputs = GetAllInputs(entity);
            var validOutputs = GetAllControls(entity);

            foreach(string input in brain.InputMap.Values)
            {
                if (!validInputs.Contains(input))
                {
                    invalidInputs.Add(input);
                }
            }

            foreach (string output in brain.OutputMap.Values)
            {
                if (!validOutputs.Contains(output))
                {
                    invalidControls.Add(output);
                }
            }

            return invalidControls.Count == 0 && invalidInputs.Count == 0;
        }


        private static List<string> GetAllPropertiesWithAttribute(Entity e, Type attributeType, string parentNameSpace)
        {
            List<string> properties = new List<string>();
            foreach (AbstractComponent ac in e.Components.Values)
            {
                // Fetch properties that have a control attribute
                var componentControls = ac.GetType()
                    .GetProperties()
                    .Where((p) => p.GetCustomAttributes(attributeType, true).Any())
                    .Select((p) => $"{parentNameSpace}{ac.GetType().Name}.{p.Name}");
                properties.AddRange(componentControls);
            }

            // Get all controls from the children
            foreach(Entity child in e.Children.Values)
            {
                // root entities should not have a . prepended
                string correctNameSpace = 
                    parentNameSpace == "" ? 
                    $"{child.Tag}." 
                    : $"{parentNameSpace}.{child.Tag}.";

                properties.AddRange(GetAllPropertiesWithAttribute(child, attributeType, correctNameSpace));
            }

            return properties;
        }

        /// <summary>
        /// Returns a list of valid controls that an entity's brain can control.
        /// These are namespaced by tags
        /// </summary>
        /// <param name="e"></param>
        public static List<string> GetAllControls(Entity e)
        {
            return GetAllPropertiesWithAttribute(e, typeof(ControlAttribute), "");
        }

        /// <summary>
        /// Returns a list of valid inputs that an entity's brain can take input from.
        /// These are namespaced by tags
        /// </summary>
        /// <param name="e"></param>
        public static List<string> GetAllInputs(Entity e)
        {
            var inputs = GetAllPropertiesWithAttribute(e, typeof(InputAttribute), "");
            inputs.AddRange(SpecialInputs);
            return inputs;
        }
    }
}

