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
    public class HTCoordE
    {
        private const double EPSILON = 1.0E-10;

        private double x = 0.0;
        private double y = 0.0;

        public double X
        {
          get { return x; }
          set { x = value; }
        }        

        public double Y
        {
          get { return y; }
          set { y = value; }
        }

        public HTCoordE() { }

        public HTCoordE(HTCoordE z)
        {
            this.Copy(z);
        }

        public HTCoordE(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        internal void Copy(HTCoordE z)
        {
            this.X = z.X;
            this.Y = z.Y;
        }

        internal void ProjectionStoE(int x, int y, HTCoordS sOrigin, HTCoordS sMax)
        {
            this.X = (double)(x - sOrigin.X) / (double)sMax.X;
            this.Y = -((double)(y - sOrigin.Y) / (double)sMax.Y);
        }

        internal bool IsValid()
        {
            return (this.D2() < 1.0);
        }

        internal void Multiply(HTCoordE z)
        {
            double tx = x;
            double ty = y;
            x = (tx * z.X) - (ty * z.Y);
            y = (tx * z.Y) + (ty * z.X);
        }

        internal void Divide(HTCoordE z)
        {
            double d = z.D2();
            double tx = x;
            double ty = y;
            x = ((tx * z.X) + (ty * z.Y)) / d;
            y = ((ty * z.X) - (tx * z.Y)) / d;
        }

        void Sub(HTCoordE a, HTCoordE b)
        {
            x = a.X - b.X;
            y = a.Y - b.Y;
        }

        internal double Arg()
        {
            double a = Math.Atan(y / x);
            if (x < 0)
            {
                a += Math.PI;
            }
            else if (y < 0)
            {
                a += 2 * Math.PI;
            }
            return a;
        }

        internal double D2()
        {
            return (x * x) + (y * y);
        }

        internal double D()
        {
            return Math.Sqrt(D2());
        }

        internal double D(HTCoordE p)
        {
            return Math.Sqrt((x - p.x) * (x - p.x) + (y - p.y) * (y - p.y));
        }

        internal void Translate(HTCoordE t)
        {
            double denX = (x * t.x) + (y * t.y) + 1;
            double denY = (y * t.x) - (x * t.y);
            double dd = (denX * denX) + (denY * denY);

            double numX = x + t.x;
            double numY = y + t.y;

            x = ((numX * denX) + (numY * denY)) / dd;
            y = ((numY * denX) - (numX * denY)) / dd;
        }

        internal void Translate(HTCoordE s, HTCoordE t)
        {
            this.Copy(s);
            this.Translate(t);
        }

        internal void Transform(HTTransformation t)
        {
            HTCoordE z = new HTCoordE(this);
            Multiply(t.O);
            x += t.P.x;
            y += t.P.y;

            HTCoordE d = new HTCoordE(t.P);
            d.y = -d.y;
            d.Multiply(z);
            d.Multiply(t.O);
            d.x += 1;

            Divide(d);
        }

        override public String ToString()
        {
            return "(" + x + " : " + y + ")E";
        }
    }
}
