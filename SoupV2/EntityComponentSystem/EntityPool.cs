using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityComponentSystem.Exceptions;
using SoupV2.EntityComponentSystem;

namespace EntityComponentSystem
{
    /// <summary>
    /// The object that managed all your game Entities.
    /// </summary>
    public class EntityPool
    {
        public delegate void EntityChanged(EntityPool pool, Entity entity);

        public event EntityChanged EntityAdded;
        public event EntityChanged EntityRemoved;

        public event EntityChanged EntityComponentAdded;
        public event EntityChanged EntityComponentRemoved;

        public delegate void EntityRelationshipChanged(Entity parent, Entity child);

        public event EntityRelationshipChanged MadeChild;
        public event EntityRelationshipChanged DetachedFromParent;

        public List<Entity> Entities { get; private set; }

        public Stack<Entity> CachedEntities { get; private set; }

        public string Id { get; set; }

        // How many Entites the cache can store at a time.
        private readonly int MAX_CACHED_ENTITIES = 25;


        private int _nextEntityId = 0;

        public int GetNextId { get => _nextEntityId++; }
        public int PeekNextId { get => _nextEntityId+1; }

        public Dictionary<string, EntityDefinition> _definitionDict = new Dictionary<string, EntityDefinition>();

        public EntityPool(string Id)
        {
            Entities = new List<Entity>();
            CachedEntities = new Stack<Entity>();

            if (Id != null)
            {
                this.Id = Id;
            }
        }

        public HashSet<int> Ids { get; } = new HashSet<int>();

        //internal void AssertValidEntityId(int entityId)
        //{
        //    if (Ids.Contains(entityId))
        //        throw new DuplicateEntityException(this);

        //    if (string.IsNullOrEmpty(entityId))
        //        throw new Exception("The string you entered was blank or null.");

        //}
        /// <summary>
        /// Creates a new Entity with "entityId", adds it to active Entities and returns it.
        /// </summary>
        /// <param Id="entityId"></param>
        /// <returns>Final Entity</returns>
        public Entity CreateEntity(string tag)
        {

            int entityId = GetNextId;
            Ids.Add(entityId);

            Entity newEntity;

            if (CachedEntities.Any())
            {
                newEntity = CachedEntities.Pop();

                if (newEntity == null)
                    throw new EntityNotFoundException(this);

                newEntity.Id = entityId;
                newEntity.OwnerPool = this;
                newEntity.State = EntityState.Active;

                Entities.Add(newEntity);

                #if DEBUG
                    Console.WriteLine($"Retrieved {newEntity.Id} from cache.");
                #endif
            } else
            {
                newEntity = new Entity(tag)
                {
                    Id = entityId,
                    OwnerPool = this
                };

                Entities.Add(newEntity);

                #if DEBUG
                    Console.WriteLine($"Created new instance for {newEntity.Id} because the cache was empty.");
                #endif
            }
            newEntity.Tag = tag;

            EntityAdded?.Invoke(this, newEntity);

            return newEntity;
        }

        /// <summary>
        /// Add an entity using an entity definition.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        internal Entity AddEntityFromDefinition(string definitionId, Entity parent=null)
        {
            // Try get new entity
            int id = GetNextId;
            Ids.Add(id);

            var entity = Entity.FromDefinition(_definitionDict[definitionId]);

            AddDeserializedEntity(entity, parent);

            return entity;
        }

        /// <summary>
        /// Add an entity that has just been deserialized and does not have its parent values etc set.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        private void AddDeserializedEntity(Entity entity, Entity parent = null)
        {

            entity.Id = GetNextId;

            entity.Parent = parent;
            entity.OwnerPool = this;
            entity.State = EntityState.Active;

            foreach (var (_, component) in entity.Components)
            {
                component.Owner = entity;
            }
            if (entity.Children.Count > 0)
            {
                foreach (Entity e in entity.Children.Values)
                {
                    // Todo handle child ids
                    AddDeserializedEntity(e, entity);
                }
            }
            Entities.Add(entity);

            EntityAdded?.Invoke(this, entity);
        }

        public bool DoesEntityExist(int Id)
        {
            return Ids.Contains(Id);
        }

        public bool DoesEntityExist(Entity entity)
        {
            return DoesEntityExist(entity.Id);
        }

        public Entity GetEntity(int entityId)
        {
            var match = Entities.FirstOrDefault(ent => ent.Id == entityId);

            if (match != null) return match;

            throw new EntityNotFoundException(this);
        }

        /// <summary>
        /// Adds an Entity to the cache to be re-used if cachedEntities isn't full.
        /// If the cache is full, just remove completely.
        /// </summary>
        /// <param Id="entity"></param>
        public void DestroyEntity(Entity entity)
        {
            if (!entity.IsNotCached())
                return;

            if (!Entities.Contains(entity))
                throw new EntityNotFoundException(this);

            if (CachedEntities.Count < MAX_CACHED_ENTITIES)
            {
                // Reset the Entity.
                CachedEntities.Push(entity);
            }

            entity.Reset();
            Entities.Remove(entity);
            EntityRemoved?.Invoke(this, entity);
        }

        /// <summary>
        /// Clears the cached Entities stack.
        /// </summary>
        public void WipeCache()
        {
            CachedEntities.Clear();
        }

        /// <summary>
        /// Clears the active Entities list.
        /// </summary>
        public void WipeEntities()
        {
            Entities.Clear();
        }

        public void AddDefinition(string definitionId, EntityDefinition definition)
        {
            _definitionDict[definitionId] = definition;
        }

        internal void ComponentAdded(Entity entity)
        {
            EntityComponentAdded?.Invoke(this, entity);
        }

        internal void ComponentRemoved(Entity entity)
        {
            EntityComponentRemoved?.Invoke(this, entity);
        }

        public void EntityMadeChild(Entity parent, Entity child)
        {
            MadeChild?.Invoke(parent, child);
        }

        public void EntityDetachedFromParent(Entity parent, Entity child)
        {
           DetachedFromParent?.Invoke(parent, child);
        }
    }
}
