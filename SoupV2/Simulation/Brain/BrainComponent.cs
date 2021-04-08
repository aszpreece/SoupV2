using EntityComponentSystem;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;
using SoupV2.util;
using Newtonsoft.Json;

namespace SoupV2.Simulation.Brain
{

    /// <summary>
    /// Component for controlling an entity.
    /// </summary>
    public class BrainComponent : AbstractComponent
    {
        private readonly List<(Func<AbstractComponent, float>, AbstractComponent, string)> _brainInputs;
        private readonly List<(Action<AbstractComponent, float>, AbstractComponent, string)> _brainOutputs;

        [JsonIgnore]
        private AbstractBrain _brain;

        [JsonIgnore]
        public AbstractBrain Brain { get => _brain; }

        /// <summary>
        /// Mapping of named brain inputs and the named source on the component
        /// </summary>
        public Dictionary<string, string> InputMap { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Mapping of named brain outputs and a named desitnation for its value
        /// </summary>
        public Dictionary<string, string> OutputMap { get; set; } = new Dictionary<string, string>();

        public BrainComponent(Entity owner) : base(owner) 
        {
            _brainInputs = new List<(Func<AbstractComponent, float>, AbstractComponent, string)>();
            _brainOutputs = new List<(Action<AbstractComponent, float>, AbstractComponent, string)>();
        }

        public void SetBrain(AbstractBrain brain)
        {
            _brain = brain;
        }

        static Random rand = new Random();
        static Func<AbstractComponent, float> randomGetter = (_)=> (float)rand.NextDouble() -0.5f;
        public void SetUpLinks()
        {
            // 'Compile' brain inputs/outputs to a map of functions and their component objects.
            _brainInputs.Clear();
            _brainOutputs.Clear();

            foreach ((string brainInput, string source) in InputMap)
            {

                // take each brain input, figure out where to get the input from in the entity
                // then store the method to get the input, the component to get it from, and the name of the input to the brain to send it to.

                string[] separators = source.Split('.');
                if (separators.Length == 1 && separators[0] == "Random" )
                {
                    // Get getter for random node.
                    _brainInputs.Add((randomGetter, null, brainInput));
                    continue;
                }
                if (separators.Length < 2)
                {
     
                    throw new InvalidMappingException(source);
                }
                var currentEntity = Owner;
                for (int i = 0; i < separators.Length - 2; i++)
                {
                    // Get each child entity referenced in the link if any
                    currentEntity = currentEntity.GetChildByTag(separators[i]);
                }
                var component = currentEntity.GetComponent(separators[separators.Length - 2]);
                var getter = GetterSetterPointers.GetPropGetter<AbstractComponent, float>(separators[separators.Length - 1], component.GetType());

                _brainInputs.Add((getter, component, brainInput));

            }

            foreach ((string brainOutput, string source) in OutputMap)
            {

                // take each brain output, figure out where to put the output from the brain
                // then store the method to set the output, the component to set it in, and the name of the output node in the brain to get it from.

                string[] separators = source.Split('.');
                if (separators.Length < 2)
                {
                    throw new InvalidMappingException(source);
                }
                var currentEntity = Owner;
                for (int i = 0; i < separators.Length - 2; i++)
                {
                    currentEntity = currentEntity.GetChildByTag(separators[i]);
                }
                var component = currentEntity.GetComponent(separators[separators.Length - 2]);
                var getter = GetterSetterPointers.GetPropSetter<AbstractComponent, float>(separators[separators.Length - 1], component.GetType());

                _brainOutputs.Add((getter, component, brainOutput));

            }
        }

        public void Calculate()
        {
            if (_brain is null)
            {
                throw new Exception("Brain not initialized. Must call set brain before calling calculate.");
            }
            foreach(var (func, component, name) in _brainInputs)
            {
                float val = func(component);
                _brain.SetInput(name, val);
            }
            _brain.Calculate();
            foreach (var (func, component, name) in _brainOutputs)
            {
                float val = _brain.GetOutput(name);
                func(component, val);
            }
        }
    }
}
