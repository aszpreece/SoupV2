using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using EntityComponentSystem.Exceptions;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.EntityComponentSystem.Exceptions;

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
        [Browsable(false)]	
        public delegate void EntityComponentChanged(Entity entity, AbstractComponent component);
        [Browsable(false)]
        public delegate void EntityRelationshipChanged(Entity parent, Entity child);

        /// <summary>
        /// Events
        /// </summary>
        [Browsable(false)]
        public event EntityComponentChanged ComponentAdded;
        [Browsable(false)]
        public event EntityComponentChanged ComponentRemoved;
        [Browsable(false)]
        public event EntityRelationshipChanged MadeChild;
        [Browsable(false)]
        public event EntityRelationshipChanged DetachedFromParent;

        [Browsable(false)]
        [JsonIgnore]
        public (int, int)? Cell { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Id { get; set; }
        public string Tag { get; set; }

        /// <summary>
        /// The Id of this individual. This is seperate from the entity Id, which is recycled.
        /// </summary>
        [Browsable(false)]
        public uint IndividualId { get; set; }

        /// <summary>
        /// The pool which this Entity resides in.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public EntityManager OwnerPool { get; set; }

        /// <summary>
        /// The set of this entitities' components
        /// </summary>
        //public List<AbstractComponent> Components { get; set; } = new List<AbstractComponent>();
        [Browsable(false)]

        public Dictionary<int, AbstractComponent> Components { get; set; } = new Dictionary<int, AbstractComponent>();

        /// <summary>
        /// The map of this entity's children indexed by entity tag.
        /// </summary>
        /// 
        [Browsable(false)]
        public Dictionary<string, Entity> Children { get; set; } = new Dictionary<string, Entity>();

        /// <summary>
        /// The Entity which this Entity is a child of
        /// Is null if this Entity is root
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Entity Parent { get; set; }

        /// <summary>
        /// Walks up all the parents of this Entity and returns the top one
        /// This Entity is called "root"
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
        public EntityState State { get; internal set; } = EntityState.Active;

        public Entity()
        {

        }

        public static Entity FromDefinition(EntityDefinition definition, JsonSerializerSettings settings)
        {
            Entity e = JsonConvert.DeserializeObject<Entity>(definition.Json, settings);

            return e;
        }

        internal Entity(string tag)
        {
            Tag = tag;
        }
        
        public bool IsActive()
        {
            if (IsCached())
                return false;

            return (State == EntityState.Active);
        }

        public bool IsInactive()
        {
            if (IsCached())
                return false;

            return (State == EntityState.Inactive);
        }

        /// <summary>
        /// Checks if an entity already has a component and adds it if not.
        /// If it does, throws ComponentAlreadyExistsException.
        /// If entity is cached throws EntityCachedException.
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private AbstractComponent AddComponent(AbstractComponent component)
        {
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried accessing {component.GetType().Name}");

            // If it has a component of the same type as "component".
            if (HasComponent(component.GetType()))
                throw new ComponentAlreadyExistsException(this);

            Components.Add(component.GetHashCode(), component);
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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried removing {typeof(T).Name}");


            if (!this.HasComponent<T>())
                throw new ComponentNotFoundException(this, typeof(T));

            AbstractComponent componentToRemove = GetComponent<T>();

            Components.Remove(componentToRemove.GetHashCode());
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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried removing {componentType.Name}");

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not IComponent children.");

            if (!HasComponent(componentType)) throw new ComponentNotFoundException(this, componentType);

            AbstractComponent componentToRemove = GetComponent(componentType);

            Components.Remove(componentToRemove.GetHashCode());
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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried accessing {typeof(T).Name}");


            if (!Components.TryGetValue(typeof(T).GetHashCode(), out AbstractComponent match))
            {
                throw new ComponentNotFoundException(this, typeof(T));
            }

            return (T)match;
        }

        /// <summary>
        /// Tries to get component of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetComponent<T>(out T component)
            where T : AbstractComponent
        {
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried accessing {typeof(T).Name}");

            if (Components.TryGetValue(typeof(T).GetHashCode(), out AbstractComponent match))
            {
                component = (T)match;
                return true;
            } else
            {
                component = null;
                return false;
            }
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
            if (IsCached())
            {
                throw new EntityCachedException(this.Id, $"Tried accessing {componentType.Name}");
            }

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not IComponent children.");

            Components.TryGetValue(componentType.GetHashCode(), out AbstractComponent match);

            if (match != null)
            {
                return match;
            }

            throw new ComponentNotFoundException(this, componentType);
        }

        public AbstractComponent GetComponent(string componentType)
        {
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried accessing {componentType}");

            var match = Components.Values.FirstOrDefault(c => c.GetType().Name == componentType);
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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried moving {component.GetType().Name}");

            // If the component itself isn't null and its actually on "this".
            if (component == null && !HasComponent(component.GetType()))
                throw new ComponentNotFoundException(this, component.GetType());

            destination.AddComponent(component);
            Components.Remove(component.GetHashCode());
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
            if (IsCached())
                return false;

            return Components.ContainsKey(typeof(TComponent).GetHashCode());
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
            if (IsCached())
                return false;

            if (!componentType.IsComponent())
                throw new Exception("One or more of the types you passed were not AbstractComponent children..");

            return Components.ContainsKey(componentType.GetHashCode());
        }

        /// <summary>
        /// Check to see if this Entity has all the components in a collection
        /// If it does, return true, otherwise return false.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public bool HasComponents(IEnumerable<Type> types)
        {
            if (IsCached())
                return false;

            foreach (var t in types)
                if (!HasComponent(t)) return false;

            return true;
        }

        public bool HasComponents(params Type[] types)
        {
            if (IsCached())
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
            if (IsCached())
                return;

            while (Components.Count > 0)
            {
                RemoveComponent(Components.First().Value.GetType());
            }
            Components.Clear();
        }

        /// <summary>
        /// RemoveAllComponents(), reset Id, OwnerPool.
        /// </summary>
        public void Reset()
        {
            RemoveAllComponents();

            foreach(var key in Children.Keys)
            {
                var child = Children[key];
                OwnerPool.DestroyEntity(child);
            }

            Children.Clear();

            this.Tag = string.Empty;
            this.Id = -1;
            this.State = EntityState.Cached;
        }

        /// <summary>
        /// Add all the components in a collection to this Entity
        /// </summary>
        /// <param name="components"></param>
        public void AddComponents(IEnumerable<AbstractComponent> components)
        {
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried adding components.");


            foreach (var c in components) 
                AddComponent(c);
        }

        /// <summary>
        /// Add a list of components to this entity
        /// </summary>
        /// <param name="components"></param>
        public void AddComponents(params AbstractComponent[] components)
        {
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried adding components.");


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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried adding a child.");

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
            if (IsCached())
                throw new EntityCachedException(this.Id, $"Tried getting a child.");

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

        public bool IsNotCached()
        {
            return State != EntityState.Cached;
        }

        public bool IsCached()
        {
            return State == EntityState.Cached;
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
            // Don't need graphics device to convert TO json so pass in null
            string output = JsonConvert.SerializeObject(this, SerializerSettings.GetDefaultSettings(null));
            return new EntityDefinition(output);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}