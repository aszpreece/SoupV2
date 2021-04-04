using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem
{
    
    /// <summary>
    /// Stores a blueprint of an entity.
    /// Keep track of the Components required to create this entity and all of its children.
    /// </summary>
    public class EntityDefinition
    {
        public string Json { get; set; }

        public EntityDefinition() { }
        public EntityDefinition(string json)
        {
            Json = json;
        }

    }
}
