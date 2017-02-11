using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drzewo_ID3
{
    public class TreeNode
    {
        public List<TreeNode> aChild;
        public string Label;

        public TreeNode(string label)
        {
            this.Label = label;
            this.aChild = new List<TreeNode>();
        }

        public TreeNode AddTreeNode(TreeNode treenode)
        {
            this.aChild.Add(treenode);

            return treenode;
        }

        public override string ToString()
        {
            return this.Label;
        }
    }
}
