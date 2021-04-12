using EntityComponentSystem;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.CustomMyra
{
    class ComponentTreeNode : TreeNode
    {
        public ComponentTreeNode(Tree top) : base(top, null)
        {

        }
        public AbstractComponent Component { get; set; }
    }
}
