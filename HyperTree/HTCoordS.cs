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
    public class HTCoordS
    {
        private const int ZONE_LENGTH = 4;

        private int x = 0;
        private int y = 0;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public HTCoordS() { }

        public HTCoordS(HTCoordS z)
        {
            this.X = z.X;
            this.Y = z.Y;
        }

        public HTCoordS(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        internal void ProjectionEtoS(HTCoordE ze, HTCoordS sOrigin, HTCoordS sMax)
        {
            x = (int)Math.Round(ze.X * sMax.X) + sOrigin.X;
            y = -(int)Math.Round(ze.Y * sMax.Y) + sOrigin.Y;
        }

        internal bool Contains(HTCoordS zs)
        {
            int length = GetDistance(zs);
            if (length <= ZONE_LENGTH)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int GetDistance(HTCoordS z)
        {
            int d2 = (z.x - x) * (z.x - x) + (z.y - y) * (z.y - y);
            return (int)Math.Round(Math.Sqrt(d2));
        }

        override public String ToString()
        {
            return "(" + x + " : " + y + ")S";
        }
    }
}
