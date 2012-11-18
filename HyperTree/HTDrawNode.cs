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

namespace HyperTree
{
    public class HTDrawNode
    {
        private HTDraw model = null;
        private HTModelNode node = null;

        private HTCoordE ze = null;
        private HTCoordE oldZe = null;
        protected HTCoordS zs = null;

        private HTDrawNodeComposite father = null;
        private HTDrawNode brother = null;

        private HTNodeLabel label = null;

        protected bool fastMode = false;

        public HTDrawNode(HTDrawNodeComposite father, HTModelNode node, HTDraw model)
        {
            this.father = father;
            this.node = node;
            this.model = model;

            label = new HTNodeLabel(this);

            ze = new HTCoordE(node.GetCoordinates());
            oldZe = new HTCoordE(ze);
            zs = new HTCoordS();

            model.MapNode(node.GetNode(), this);
            return;
        }

        internal void SetBrother(HTDrawNode brother)
        {
            this.brother = brother;
        }

        internal HTModelNode GetHTModelNode()
        {
            return node;
        }

        internal Color GetColor()
        {
            return node.GetNode().GetColor();
        }

        internal String GetName()
        {
            return node.GetName();
        }

        internal HTCoordE GetCoordinates()
        {
            return ze;
        }

        internal HTCoordE GetOldCoordinates()
        {
            return oldZe;
        }

        internal HTCoordS GetScreenCoordinates()
        {
            return zs;
        }

        internal virtual void RefreshScreenCoordinates(HTCoordS sOrigin, HTCoordS sMax)
        {
            zs.ProjectionEtoS(ze, sOrigin, sMax);
        }

        internal virtual void DrawBranches(Graphic g) { }

        internal virtual void DrawNodes(Graphic g)
        {
            if (fastMode == false)
            {
                label.Draw(g);
            }
        }

        internal Color GetBranchColor()
        {
            int colorWeight = this.node.GetNode().GetExtensionCount();
            return Color.FromArgb(255, (byte)Math.Min(colorWeight * 48, 255), 0, 0);
            
        }

        internal virtual int GetSpace()
        {
            int dF = -1;
            int dB = -1;

            if (father != null)
            {
                HTCoordS zF = father.GetScreenCoordinates();
                dF = zs.GetDistance(zF);
            }
            if (brother != null)
            {
                HTCoordS zB = brother.GetScreenCoordinates();
                dB = zs.GetDistance(zB);
            }

            if ((dF == -1) && (dB == -1))
            {
                return int.MaxValue;
            }
            else if (dF == -1)
            {
                return dB;
            }
            else if (dB == -1)
            {
                return dF;
            }
            else
            {
                return Math.Min(dF, dB);
            }
        }

        internal virtual void Translate(HTCoordE t)
        {
            ze.Translate(oldZe, t);
        }

        internal virtual void Transform(HTTransformation t)
        {
            ze.Copy(oldZe);
            ze.Transform(t);
        }

        internal virtual void EndTranslation()
        {
            oldZe.Copy(ze);
        }

        internal virtual void Restore()
        {
            HTCoordE orig = node.GetCoordinates();
            ze.X = orig.X;
            ze.Y = orig.Y;
            oldZe.Copy(ze);
        }

        internal virtual void FastMode(bool mode)
        {
            if (mode != fastMode)
            {
                fastMode = mode;
            }
        }

        internal virtual HTDrawNode FindNode(HTCoordS zs)
        {
            if (label.Contains(zs))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        override public String ToString()
        {
            String result = GetName() +
                            "\n\t" + ze +
                            "\n\t" + zs;
            return result;
        }

        public int GetSize()
        {
            return node.GetNode().GetSize();
        }

        public int GetBorderSize()
        {
            return node.GetNode().GetBorderSize();
        }

        public Image GetImage()
        {
            return node.GetNode().GetImage();
        }
    }
}
