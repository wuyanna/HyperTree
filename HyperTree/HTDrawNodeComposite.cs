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
using System.Collections.Generic;
using System.Diagnostics;

namespace HyperTree
{
    public class HTDrawNodeComposite : HTDrawNode
    {

        private HTModelNodeComposite node = null;
        private ObservableCollection<HTDrawNode> children = null;
        private Dictionary<HTDrawNode, HTGeodesic> geodesics = null;

        internal HTDrawNodeComposite(HTDrawNodeComposite father, HTModelNodeComposite node, HTDraw model)
            : base(father, node, model)
        {
            this.node = node;
            this.children = new ObservableCollection<HTDrawNode>();
            this.geodesics = new Dictionary<HTDrawNode, HTGeodesic>();

            HTModelNode childNode = null;
            HTDrawNode child = null;
            HTDrawNode brother = null;
            bool first = true;
            bool second = false;
            for (IEnumerator i = node.Children(); i.MoveNext(); )
            {
                childNode = (HTModelNode)i.Current;
                if (childNode.IsLeaf())
                {
                    child = new HTDrawNode(this, childNode, model);
                }
                else
                {
                    child = new HTDrawNodeComposite(this, (HTModelNodeComposite)childNode, model);
                }
                AddChild(child);
                if (first)
                {
                    brother = child;
                    first = false;
                    second = true;
                }
                else if (second)
                {
                    child.SetBrother(brother);
                    brother.SetBrother(child);
                    brother = child;
                    second = false;
                }
                else
                {
                    child.SetBrother(brother);
                    brother = child;
                }
            }
        }

        IEnumerator Children()
        {
            return this.children.GetEnumerator();
        }

        void AddChild(HTDrawNode child)
        {
            children.Add(child);
            geodesics.Add(child, new HTGeodesic(GetCoordinates(), child.GetCoordinates()));
        }

        override internal void RefreshScreenCoordinates(HTCoordS sOrigin, HTCoordS sMax)
        {
            base.RefreshScreenCoordinates(sOrigin, sMax);
            HTDrawNode child = null;

            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.RefreshScreenCoordinates(sOrigin, sMax);
                HTGeodesic geod = (HTGeodesic)geodesics[child];
                if (geod != null)
                {
                    geod.RefreshScreenCoordinates(sOrigin, sMax);
                }
            }
        }

        override internal void DrawBranches(Graphic g)
        {
            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                HTGeodesic geod = (HTGeodesic)geodesics[child];
                
                if (geod != null)
                {

                    geod.Draw(g, child.GetBranchColor());
                }
                child.DrawBranches(g);
            }
        }

        override internal void DrawNodes(Graphic g)
        {
            if (fastMode == false)
            {
                base.DrawNodes(g);

                HTDrawNode child = null;
                for (IEnumerator i = Children(); i.MoveNext(); )
                {
                    child = (HTDrawNode)i.Current;
                    child.DrawNodes(g);
                }
            }
        }

        override internal int GetSpace()
        {
            int space = base.GetSpace();
            if (children.Count > 0)
            {
                HTDrawNode child = (HTDrawNode)children[0];
                HTCoordS zC = child.GetScreenCoordinates();
                int dC = zs.GetDistance(zC);

                if (space == -1)
                {
                    return dC;
                }
                else
                {
                    return Math.Min(space, dC);
                }
            }
            else
            {
                return space;
            }
        }

        override internal void Translate(HTCoordE t)
        {
            base.Translate(t);
            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.Translate(t);
                HTGeodesic geod = (HTGeodesic)geodesics[child];
                if (geod != null)
                {
                    geod.Rebuild();
                }
            }
        }

        override internal void Transform(HTTransformation t)
        {
            base.Transform(t);

            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.Transform(t);
                HTGeodesic geod = (HTGeodesic)geodesics[child];
                if (geod != null)
                {
                    geod.Rebuild();
                }
            }
        }

        override internal void EndTranslation()
        {
            base.EndTranslation();

            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.EndTranslation();
            }
        }

        override internal void Restore()
        {
            base.Restore();
            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.Restore();
                HTGeodesic geod = (HTGeodesic)geodesics[child];
                if (geod != null)
                {
                    geod.Rebuild();
                }
            }
        }

        override internal void FastMode(bool mode)
        {
            base.FastMode(mode);
            HTDrawNode child = null;
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                child.FastMode(mode);
            }
        }

        override internal HTDrawNode FindNode(HTCoordS zs)
        {
            HTDrawNode result = base.FindNode(zs);
            if (result != null)
            {
                return result;
            }
            else
            {
                HTDrawNode child = null;
                for (IEnumerator i = Children(); i.MoveNext(); )
                {
                    child = (HTDrawNode)i.Current;
                    result = child.FindNode(zs);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
        }

        override public String ToString()
        {
            String result = base.ToString();
            HTDrawNode child = null;
            result += "\n\tChildren :";
            for (IEnumerator i = Children(); i.MoveNext(); )
            {
                child = (HTDrawNode)i.Current;
                result += "\n\t-> " + child.GetName();
            }
            return result;
        }
    }
}
