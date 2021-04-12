using EntityComponentSystem;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.CustomMyra
{
    public class EntityTreeNode : TreeNode
    {
        public EntityTreeNode(Tree top) : base(top, null)
        {

        }
        public Entity entity;
    }
}
