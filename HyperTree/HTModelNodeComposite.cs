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
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HyperTree
{
    public class HTModelNodeComposite : HTModelNode
    {
        private ObservableCollection<HTModelNode> children = null;
        private double globalWeight = 0.0;

        public HTModelNodeComposite(HTNode node, HTModel model) : this(node,null,model)
        {
        }

        public HTModelNodeComposite(HTNode node, HTModelNodeComposite parent, HTModel model)
            : base(node, parent, model)
        {
            this.children = new ObservableCollection<HTModelNode>();
            HTNode childNode = null;
            HTModelNode child = null;
            for (IEnumerator i = node.Children(); i.MoveNext(); )
            {
                childNode = (HTNode)i.Current;
                if (childNode.IsLeaf())
                {
                    child = new HTModelNode(childNode, this, model);
                    //Debug.WriteLine("HTModelNode:" + child);
                }
                else
                {
                    child = new HTModelNodeComposite(childNode, this, model);
                    //Debug.WriteLine("HTModelNodeComposite:" + child);
                }
                AddChild(child);                
            }
            ComputeWeight();
        }

        private void ComputeWeight()
        {
            HTModelNode child = null;

            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTModelNode)i.Current;
                globalWeight += child.GetWeight();
            }
            if (globalWeight != 0.0)
            {
                weight += Math.Log(globalWeight);
            }
        }

        internal IEnumerator Children()
        {
            return this.children.GetEnumerator();
        }

        private void AddChild(HTModelNode child)
        {
            children.Add(child);
        }

        override public bool IsLeaf()
        {
            return false;
        }

        override public void Layout(double angle, double width, double length)
        {
            base.Layout(angle, width, length);

            if (parent != null)
            {
                HTCoordE a = new HTCoordE(Math.Cos(angle), Math.Sin(angle));
                HTCoordE nz = new HTCoordE(-z.X, -z.Y);
                a.Translate(parent.GetCoordinates());
                a.Translate(nz);
                angle = a.Arg();

                double c = Math.Cos(width);
                double A = 1 + length * length;
                double B = 2 * length;
                width = Math.Acos((A * c - B) / (A - B * c));
            }

            HTModelNode child = null;
            HTCoordE dump = new HTCoordE();

            int nbrChild = children.Count;
            double l1 = (0.95 - model.GetLength());
            double l2 = Math.Cos((20.0 * Math.PI) / (2.0 * nbrChild + 38.0));
            length = model.GetLength() + (l1 * l2);

            double startAngle = angle - width;

            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTModelNode)i.Current;

                double percent = child.GetWeight() / globalWeight;
                double childWidth = width * percent;
                double childAngle = startAngle + childWidth;
                child.Layout(childAngle, childWidth, length);
                startAngle += 2.0 * childWidth;
            }
        }

        override public String ToString()
        {
            String result = base.ToString();
            HTModelNode child = null;
            result += "\n\tChildren :";
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTModelNode)i.Current;
                result += "\n\t-> " + child.GetName();
            }
            return result;
        }
        
    }
}
