using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.CustomMyra
{
    public class CustomTree : TreeNode
    {
        public CustomTree(Tree top) : base(top, null)
        {

        }
        public void AddNode(TreeNode parent, TreeNode toAdd)
        {
            toAdd.GridRow = ChildNodesGrid.Widgets.Count;

            ChildNodesGrid.Widgets.Add(toAdd);
            ChildNodesGrid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            UpdateMark();
        }

    }
}
