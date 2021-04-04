using System;
using System.Collections.Generic;
using System.Linq;

using EntityComponentSystem.Exceptions;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;

namespace EntityComponentSystem
{
    public enum EntityState
    {
        Active,
        Inactive,
        Cached,
    }

    public sealed class Entity
    {
        /// <summary>
        /// Delegate for ComponentAdded and ComponentRemoved.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        public delegate void EntityComponentChanged(Entity entity, AbstractComponent component);

        public delegate void EntityRelationshipChanged(Entity parent, Entity child);

        /// <summary>
        /// Events
        /// </summary>
        public event EntityComponentChanged ComponentAdded;
        public event EntityComponentChanged ComponentRemoved;

        public event EntityRelationshipChanged MadeChild;
        public event EntityRelationshipChanged DetachedFromParent;

        [JsonIgnore]
        public int Id { get; set; }
        public string Tag { get; set; }


        /// <summary>
        /// The pool which this Entity resides in.
        /// </summary>
        [JsonIgnore]
        public EntityPool OwnerPool { get; set; }

        /// <summary>
        /// The set of this entitities' components
        /// </summary>
        //public List<AbstractComponent> Components { get; set; } = new List<AbstractComponent>();
        public HashSet<AbstractComponent> Components { get; set; } = new HashSet<AbstractComponent>();

        /// <summary>
        /// The map of this entity's children indexed by entity tag.
        /// </summary>
        public Dictionary<string, Entity> Children { get; set; } = new Dictionary<string, Entity>();

        /// <summary>
        /// The Entity which this Entity is a child of
        /// Is null if this Entity is root
        /// </summary>
        [JsonIgnore]
        public Entity Parent { get; set; }

        /// <summary>
        /// Walks up all the parents of this Entity and returns the top one
        /// This Entity is called "root"
        /// </summary>
        [JsonIgnore]
        public Entity RootEntity 
        {
            get 
            {
                if (this.Parent == null)
                    return this;

                var parent = Parent;

                while (parent != null) 
                {
                    if (parent.Parent == null)
                        return parent;

                    parent = parent.Parent;
                }

                throw new Exception($"Entity \"{Id}\" has no Root!");
            }
        }

        /// <summary>
        /// Holds the current state of this Entity
        /// </summary>
        [JsonIgnore]
        public EntityState State { get; internal set; } = EntityState.Active;

        public Entity()
        {

        }

        public static Entity FromDefinition(EntityDefinition definition)
        {
            Entity e = JsonConvert.DeserializeObject<Entity>(definition.Json, SerializerSettings.DefaultSettings);

            return e;
        }

        internal Entity(string tag)
        {
            Tag = tag;
        }
        
        /// <summary>
        /// Set this Entity's state to active, providing it isn't in the cache
        /// </summary>
        public void Activate()
        {
            if (!IsAvailable())
                return;

            State = EntityState.Active;
        }

        /// <summary>
        /// Set this Entity's state to inactive, providing it isn't in the cache
        /// </summary>
        public void Deactivate()
        {
            if (!IsAvailable())
                return;

            State = EntityState.Inactive;
        }
        public bool IsActive()
        {
            if (!IsAvailable())
                return false;

            return (State == EntityState.Active);
        }

        public bool IsInactive()
        {
            if (!IsAvailable())
                return false;

            return (State == EntityState.Inactive);
        }

        /// <summary>
        /// Checks if this already has "component"
        /// If it does, ComponentAlreadyExistsException.
        /// Otherwise, add the component to "Components".
        /// And fire the ComponentAdded event (if it's also not null)
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private AbstractComponent AddComponent(AbstractComponent component)
        {
            if (!IsAvailable())
                return null;

            // If it has a component of the same type as "component".
            if (HasComponent(component.GetType()))
                throw new ComponentAlreadyExistsException(this);

            Components.Add(component);
            ComponentAdded?.Invoke(this, component);

            if (!(OwnerPool is null))
            {
                OwnerPool.ComponentAdded(this);
            }

            return component;
        }

        /// <summary>
        /// Remove a component by generic type parameter and notify the appropriate Entity pool and Systems
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveComponent<T>() 
            where T : AbstractComponent
        {
            if (!IsAvailable())
                return;

            if (!this.HasComponent<T>())
                throw new ComponentNotFoundException(this, typeof(T));

            AbstractComponent componentToRemove = GetComponent<T>();

            Components.Remove(componentToRemove);
            ComponentRemoved?.Invoke(this, componentToRemove);
            OwnerPool.ComponentRemoved(this);
        }

        /// <summary>
        /// Remove a Component by a Type parameter and notify the appropriate Entity pool and Systems
        /// Uses runtime type checking to make sure the type you passed implements IComponent
        /// </summary>
        /// <param name="componentType"></param>
        public void RemoveComponent(Type componentType)
        {
            if (!IsAvailable())
                return;

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not IComponent children.");

            if (!HasComponent(componentType)) throw new ComponentNotFoundException(this, componentType);

            AbstractComponent componentToRemove = GetComponent(componentType);

            Components.Remove(componentToRemove);
            ComponentRemoved?.Invoke(this, componentToRemove);
            OwnerPool.ComponentRemoved(this);
        }

        /// <summary>
        /// Checks through Components for a component of type "T"
        /// If it doesn't have one, throw ComponentNotFoundException.
        /// Otherwise returns the component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>()
            where T : AbstractComponent
        {
            if (!IsAvailable())
                return default(T);

            var match = Components.OfType<T>().FirstOrDefault();

            if (match == null) throw new ComponentNotFoundException(this, typeof(T));

            return match;
        }

        /// <summary>
        /// Checks through Components for a component of type "componentType"
        /// If it doesn't have one, throw ComponentNotFoundException.
        /// Otherwise return the component.
        /// Uses runtime type checking to make sure "componentType" implements IComponent
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public AbstractComponent GetComponent(Type componentType)
        {
            if (!IsAvailable())
                return null;

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not IComponent children.");

            var match = Components.FirstOrDefault(c => c.GetType() == componentType);
            if (match != null) return match;

            throw new ComponentNotFoundException(this, componentType);
        }

        public AbstractComponent GetComponent(string componentType)
        {
            if (!IsAvailable())
                return null;

            var match = Components.FirstOrDefault(c => c.GetType().Name == componentType);
            if (match != null) return match;

            throw new ComponentNotFoundException(this, componentType);
        }

        /// <summary>
        /// Moves a component between this and "destination".
        /// If destination or the component are null, throw a ComponentNotFoundException.
        /// Otherwise add the component to the destination and remove it from this.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="destination"></param>
        public void MoveComponent(AbstractComponent component, Entity destination)
        {
            if (!IsAvailable())
                return;

            // If the component itself isn't null and its actually on "this".
            if (component == null && !HasComponent(component.GetType()))
                throw new ComponentNotFoundException(this, component.GetType());

            destination.AddComponent(component);
            Components.Remove(component);
        }

        /// <summary>
        /// Checks if "this" contains a component of type "TComponent".
        /// If it does, return true. Otherwise, return false.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public bool HasComponent<TComponent>() 
            where TComponent : AbstractComponent
        {
            if (!IsAvailable())
                return false;

            var match = Components.Any(c => c.GetType() == typeof(TComponent));

            if (match) return true;
            else return false;
        }

        /// <summary>
        /// Checks if "this" contains a component of type "TComponent".
        /// If it does, return true. Otherwise, return false.
        /// Uses runtime type checking to make sure "componentType" implements IComponent
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public bool HasComponent(Type componentType)
        {
            if (!IsAvailable())
                return false;

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not IComponent children.");

            var cMatch = Components.Any(c => c.GetType() == componentType);
            if (cMatch) return true;

            return false;
        }

        /// <summary>
        /// Check to see if this Entity has all the components in a collection
        /// If it does, return true, otherwise return false.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public bool HasComponents(IEnumerable<Type> types)
        {
            if (!IsAvailable())
                return false;

            foreach (var t in types)
                if (!HasComponent(t)) return false;

            return true;
        }

        public bool HasComponents(params Type[] types)
        {
            if (!IsAvailable())
                return false;

            foreach (var t in types)
                if (!HasComponent(t)) return false;

            return true;
        }

        /// <summary>
        /// Remove all components.
        /// </summary>
        public void RemoveAllComponents()
        {
            if (!IsAvailable())
                return;

            while(Components.Count > 0)
            {
                RemoveComponent(Components.First().GetType());
            }
            Components.Clear();
        }

        /// <summary>
        /// RemoveAllComponents(), reset Id, OwnerPool.
        /// </summary>
        public void Reset()
        {
            if (!IsAvailable())
                return;

            RemoveAllComponents();

            foreach(var key in Children.Keys)
            {
                var child = Children[key];
                OwnerPool.DestroyEntity(child);
            }

            Children.Clear();

            this.Id = -1;
            this.OwnerPool = null;
            this.State = EntityState.Cached;
        }

        /// <summary>
        /// Add all the components in a collection to this Entity
        /// </summary>
        /// <param name="components"></param>
        public void AddComponents(IEnumerable<AbstractComponent> components)
        {
            if (!IsAvailable())

            foreach (var c in components) 
                AddComponent(c);
        }

        /// <summary>
        /// Allows an infinite(?) number of components as parameters and adds them all at once to "this".
        /// </summary>
        /// <param name="components"></param>
        public void AddComponents(params AbstractComponent[] components)
        {
            if (!IsAvailable())
                return;

            foreach (var c in components) 
                AddComponent(c);
        }

        /// <summary>
        /// Adds an existing Entity as a child to this Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity AddChild(Entity entity)
        {
            if (!IsAvailable())
                return null;

            entity.Parent = this;
            Children[entity.Tag] = entity;

            MadeChild?.Invoke(this, entity);
            
            OwnerPool?.EntityMadeChild(this, entity);

            return entity;
        }

        /// <summary>
        /// Get a child by Tag
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        public Entity GetChildByTag(string childTag)
        {
            if (!IsAvailable())
                return null;

            return Children[childTag];
        }


        /// <summary>
        /// Returns the whole "family tree" of this Entity (all children, "grandchildren", etc.)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> FamilyTree()
        {
            var childSelector = new Func<Entity, IEnumerable<Entity>>(ent => ent.Children.Values);

            var stack = new Stack<Entity>(Children.Values);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        public bool IsAvailable()
        {
            return State != EntityState.Cached;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
            {
                return false;
            }
            return Id == ((Entity)obj).Id;
        }

        public EntityDefinition ToDefinition()
        {
            string output = JsonConvert.SerializeObject(this, SerializerSettings.DefaultSettings);
            return new EntityDefinition(output);
        }
    }
}