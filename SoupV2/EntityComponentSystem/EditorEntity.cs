using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem
{
    /// <summary>
    /// Version of entity for editing purposes
    /// </summary>
    class EditorEntity
    {
        public string Tag { get; set; } = string.Empty;
        public List<AbstractComponent> Components { get; set; } = new List<AbstractComponent>();
        public List<EditorEntity> Children { get; set; } = new List<EditorEntity>();

    }
}
