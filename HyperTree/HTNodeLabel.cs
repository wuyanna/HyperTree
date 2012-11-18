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
    public class HTNodeLabel
    {
        private HTDrawNode node = null;
        private int x = 0;
        private int y = 0;
        private int width = 0;
        private int height = 0;
        private bool active = false;

        public HTNodeLabel(HTDrawNode node)
        {
            this.node = node;
        }

        internal void Draw(Graphic g)
        {
            int fh = 30;
            int space = node.GetSpace();
            // TODO:unfinished
            if (space >= fh)
            {
                active = true;
                HTCoordS zs = node.GetScreenCoordinates();
                String name = node.GetName();
                Color color = node.GetColor();

                height = fh + 2 * node.GetSize();
                width = space + 10 + 2 * node.GetSize();
                x = zs.X - (width / 2) - node.GetSize();
                y = zs.Y - (fh / 2) - node.GetSize();

                g.DrawDot(zs.X, zs.Y, 3, color);
                g.DrawLabel(x, y, width, height, name, color);
                
            }
            else
            {
                active = false;
            }

        }

        internal bool Contains(HTCoordS zs)
        {
            if (active)
            {
                if ((zs.X >= x) && (zs.X <= (x + width)) &&
                    (zs.Y >= y) && (zs.Y <= (y + height)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return node.GetScreenCoordinates().Contains(zs);
            }
        }

        override public String ToString()
        {
            String result = "label of " + node.GetName() +
                "\n\tx = " + x + " : y = " + y +
                "\n\tw = " + width + " : h = " + height;
            return result;
        }
    }
}
