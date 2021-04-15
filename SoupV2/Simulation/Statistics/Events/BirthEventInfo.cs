using Microsoft.Xna.Framework;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Events
{
    public class BirthEventInfo : AbstractEventInfo
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public AbstractBrainGenotype ChildGenotype { get; set; }

        public BirthEventInfo(Vector2 location, uint tick, int parentId, int childId, AbstractBrainGenotype childGenotype): base(tick, location)
        {
            ParentId = parentId;
            ChildId = childId;
            ChildGenotype = childGenotype;
        }

        public override string ToString()
        {
            return $"Parent: {ParentId}, Child: {ChildId}";
        }
    }
}
