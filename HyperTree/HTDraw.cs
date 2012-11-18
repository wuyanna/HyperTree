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
using System.Collections.Generic;
using System.Threading;

namespace HyperTree
{
    public class HTDraw
    {
        public const int NBR_FRAMES = 10;

        private HTModel model = null;
        private HTView view = null;
        private HTDrawNode drawRoot = null;

        private HTCoordS sOrigin = null;
        private HTCoordS sMax = null;

        private double[] ray = null;
        private bool fastMode = false;

        private Dictionary<HTNode,HTDrawNode> drawToHTNodeMap;

        public HTDraw(HTModel model, HTView view)
        {
            drawToHTNodeMap = new Dictionary<HTNode, HTDrawNode>();

            this.view = view;
            this.model = model;
            HTModelNode root = model.GetRoot();
            sOrigin = new HTCoordS();
            sMax = new HTCoordS();

            ray = new double[4];
            ray[0] = model.GetLength();

            for (int i = 1; i < ray.Length; i++)
            {
                ray[i] = (ray[0] + ray[i - 1]) / (1 + (ray[0] * ray[i - 1]));
            }

            if (root.IsLeaf())
            {
                drawRoot = new HTDrawNode(null, root, this);
            }
            else
            {
                drawRoot = new HTDrawNodeComposite(null, (HTModelNodeComposite)root, this);
            }
            return;
        }

        internal void RefreshScreenCoordinates()
        {
            Thickness margin = view.Margin;
            sMax.X = ((int)view.Width - (int)margin.Left - (int)margin.Right) / 2;
            sMax.Y = ((int)view.Height - (int)margin.Top - (int)margin.Bottom) / 2;
            sOrigin.X = sMax.X + (int)margin.Left;
            sOrigin.Y = sMax.Y + (int)margin.Top;
            drawRoot.RefreshScreenCoordinates(sOrigin, sMax);
        }

        internal HTCoordS GetSOrigin()
        {
            return sOrigin;
        }

        internal HTCoordS GetSMax()
        {
            return sMax;
        }

        internal void DrawBranches(Graphic g)
        {
            drawRoot.DrawBranches(g);
        }

        internal void DrawNodes(Graphic g)
        {
            drawRoot.DrawNodes(g);
        }

        internal void Translate(HTCoordE zs, HTCoordE ze)
        {
            HTCoordE zo = new HTCoordE(drawRoot.GetOldCoordinates());
            zo.X = -zo.X;
            zo.Y = -zo.Y;
            HTCoordE zs2 = new HTCoordE(zs);
            zs2.Translate(zo);

            HTCoordE t = new HTCoordE();
            double de = ze.D2();
            double ds = zs2.D2();
            double dd = 1.0 - de * ds;
            t.X = (ze.X * (1.0 - ds) - zs2.X * (1.0 - de)) / dd;
            t.Y = (ze.Y * (1.0 - ds) - zs2.Y * (1.0 - de)) / dd;

            if (t.IsValid())
            {
                HTTransformation to = new HTTransformation();
                to.Composition(zo, t);

                drawRoot.Transform(to);
                view.Repaint();
            }
        }

        internal void EndTranslation()
        {
            drawRoot.EndTranslation();
        }

        internal void TranslateToOrigin(HTDrawNode node)
        {
            view.StopMouseListening();
            //AnimThread t = new AnimThread(node);
            //t.Start();
        }

        void Restore()
        {
            drawRoot.Restore();
            view.Repaint();
        }

        void FastMode(bool mode)
        {
            if (mode != fastMode)
            {
                fastMode = mode;
                drawRoot.FastMode(mode);
                if (mode == false)
                {
                    view.Repaint();
                }
            }
        }

        internal HTDrawNode FindNode(HTCoordS zs)
        {
            return drawRoot.FindNode(zs);
        }

        internal void MapNode(HTNode htNode, HTDrawNode drawNode)
        {
            drawToHTNodeMap.Add(htNode, drawNode);
            return;
        }

        internal HTDrawNode FindDrawNode(HTNode htNode)
        {
            HTDrawNode drawNode = (HTDrawNode)drawToHTNodeMap[htNode];
            return drawNode;
        }

    }
}
