using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public class BirthEventInfo : AbstractEventInfo
    {
        public int ParentId { get; set; }
        public string ParentTag { get; }
        public int ChildId { get; set; }
        public string ChildTag { get; }

        [JsonIgnore]
        public AbstractBrainGenotype ChildGenotype { get; set; }

        public BirthEventInfo(Vector2 location, float tick, int parentId, string parentTag, int childId, string childTag, AbstractBrainGenotype childGenotype, int speciesId): base(tick, location)
        {
            ParentId = parentId;
            ParentTag = parentTag;
            ChildId = childId;
            ChildTag = childTag;
            ChildGenotype = childGenotype;
        }

        public override string ToString()
        {
            return $"Parent: {ParentId}, Child: {ChildId}";
        }
    }
}
