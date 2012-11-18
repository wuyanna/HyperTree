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
    public class HTModelNode
    {
        private HTNode node = null;
        protected HTModel model = null;
        protected HTModelNodeComposite parent = null;
        protected HTCoordE z = null;
        protected double weight = 1.0;

        public HTModelNode(HTNode node, HTModel model) : this(node,null,model)
        {
        }

        public HTModelNode(HTNode node, HTModelNodeComposite parent, HTModel model)
        {
            this.node = node;
            this.parent = parent;
            this.model = model;
            model.IncrementNumberOfNodes();

            z = new HTCoordE();
        }

        internal HTNode GetNode()
        {
            return node;
        }

        public String GetName()
        {
            return node.GetName();
        }

        public double GetWeight()
        {
            return weight;
        }

        HTModelNodeComposite GetParent()
        {
            return parent;
        }

        public virtual bool IsLeaf()
        {
            return true;
        }

        public HTCoordE GetCoordinates()
        {
            return z;
        }

        internal void LayoutHyperbolicTree()
        {
            this.Layout(0.0, Math.PI, model.GetLength());
        }

        public virtual void Layout(double angle, double width, double length)
        {
            if (parent == null)
            {
                return;
            }

            HTCoordE zp = parent.GetCoordinates();

            z.X = length * Math.Cos(angle);
            z.Y = length * Math.Sin(angle);

            z.Translate(zp);
            //Debug.WriteLine("modelnodel.z:" + z);
            //Debug.WriteLine("modelnodel.zparent:" + parent);
        }

        override public String ToString()
        {
            String result = GetName() +
                            "\n\t" + z +
                            "\n\tWeight = " + weight;
            return result;
        }

    }
}
