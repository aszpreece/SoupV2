using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Components.util
{
    /// <summary>
    /// Static class for storing component compatibility
    /// </summary>
    public static class ComponentCompatibility
    {

        public static List<Type> AllComponents = Assembly.GetExecutingAssembly().GetTypes()
              .Where(t => t.Namespace == "SoupV2.Simulation.Components")
               .Where(t => typeof(AbstractComponent).IsAssignableFrom(t))
              .ToList();

        /// <summary>
        /// Map for deciding whether a component is compatible with an entity
        /// </summary>
        public static Dictionary<Type, Func<Entity, (string, bool)>> CompatiblePredicates = new Dictionary<Type, Func<Entity, (string, bool)>>()
        {
            {
                typeof(RigidBodyComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }

                        if (!e.HasComponents(typeof(VelocityComponent), typeof(CircleColliderComponent)))
                        {
                            return ("Must have VelocityComponent and CircleColliderComponent.", false);
                        }
                        return ("", true);
                    }
                },
                { 
                typeof(OldAgeComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(BrainComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(DragComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(HealthComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(MovementControlComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        if (!e.HasComponent<RigidBodyComponent>())
                        {
                            return ("Must have a rigid body component.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(ReproductionComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(EdibleComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                },
                {
                typeof(EnergyComponent),
                (e) =>
                    {
                        if (!e.IsRoot)
                        {
                            return ("Must be root entity.", false);
                        }
                        return ("", true);
                    }
                }
        };

        public static List<Type> GetCompatibleComponents(Entity e)
        {
            List<Type> validComponents = new List<Type>();
            foreach (Type component in AllComponents)
            {
                // Cannot contain duplicate components
                if (e.Components.ContainsKey(component.GetType().Name))
                {
                    continue;
                }
                if (ValidateComponent(component, e).Item2)
                {
                    validComponents.Add(component);
                }
            }

            return validComponents;
        }

        /// <summary>
        /// Validate if a component is valid on an entity
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static (string, bool) ValidateComponent(Type component, Entity owner)
        {
            // if there is not predicate in the list then we can assume it is compatible.
            if (!CompatiblePredicates.ContainsKey(component))
            {
                return ("", true);
            }

            // check predicate if there is one
            (string, bool) result = CompatiblePredicates[component].Invoke(owner);
            return result;
        }

        /// <summary>
        /// Validates if all components on an entity are valid.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="errorReasons"></param>
        /// <returns></returns>
        public static bool ValidateEntityComponents(Entity e, out List<string> errorReasons)
        {
            errorReasons = e.Components.Values
                .Select((ac) =>
                    (ac.GetType().Name, ValidateComponent(ac.GetType(), e)))
                .Where((pair) => !pair.Item2.Item2)
                .Select((args) => 
                    $"{e.Tag}.{args.Item1}: {args.Item2.Item1}")
                .ToList();
            return errorReasons.Count == 0;
        }

        /// <summary>
        /// Validates an entity tree.
        /// </summary>
        /// <param name="e"></param>
        /// <param name=""></param>
        /// <param name="errorReasons"></param>
        /// <returns></returns>
        public static bool ValidateEntity(Entity e, out List<string> errorReasons)
        {
            ValidateEntityComponents(e, out List<string> reasons);

            foreach (Entity child in e.Children.Values)
            {
                ValidateEntity(child, out List<string> childReasons);
                reasons.AddRange(childReasons);
            }
            errorReasons = reasons;

            return reasons.Count == 0;
        }
    }
}
