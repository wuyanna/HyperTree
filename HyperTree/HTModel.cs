using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace HyperTree
{
    public class HTModel
    {
        private HTModelNode root = null;

        private double      length = 0.3;
        private int         nodes = 0;

        public HTModel(HTNode root)
        {
            if (root.IsLeaf())
            {
                this.root = new HTModelNode(root, this);
                //Debug.WriteLine("root:" + this.root);
            }
            else
            {
                this.root = new HTModelNodeComposite(root, this);
                //Debug.WriteLine("root:" + this.root);
            }
            this.root.LayoutHyperbolicTree();
        }

        internal HTModelNode GetRoot()
        {
            return root;
        }

        internal double GetLength()
        {
            return length;
        }

        internal void IncrementNumberOfNodes()
        {
            nodes++;
        }

        int GetNumberOfNodes()
        {
            return nodes;
        }

    }
}
