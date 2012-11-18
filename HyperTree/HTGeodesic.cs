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
    internal class HTGeodesic
    {
        private const double EPSILON = 1.0E-10;

        private const int LINE = 0;
        private const int ARC = 1;

        private int type = LINE;

        private HTCoordE za = null;
        private HTCoordE zb = null;
        private HTCoordE zc = null;
        private HTCoordE zo = null;

        private HTCoordS a = null;
        private HTCoordS b = null;
        private HTCoordS c = null;

        internal HTGeodesic(HTCoordE za, HTCoordE zb)
        {
            this.za = za;
            this.zb = zb;
            zc = new HTCoordE();
            zo = new HTCoordE();

            a = new HTCoordS();
            b = new HTCoordS();
            c = new HTCoordS();

            Rebuild();
        }

        internal void RefreshScreenCoordinates(HTCoordS sOrigin, HTCoordS sMax)
        {
            a.ProjectionEtoS(za, sOrigin, sMax);
            b.ProjectionEtoS(zb, sOrigin, sMax);
            c.ProjectionEtoS(zc, sOrigin, sMax);
        }

        internal void Rebuild()
        {
            if ((Math.Abs(za.D()) < EPSILON) ||
                (Math.Abs(zb.D()) < EPSILON) ||
                (Math.Abs((za.X / zb.X) - (za.Y / zb.Y)) < EPSILON))
            {
                //Debug.WriteLine("za.d:" + za.D());
                //Debug.WriteLine("zb.d:" + zb.D());
                type = LINE;
            }
            else
            {
                type = ARC;
                double da = 1 + za.X * za.X + za.Y * za.Y;
                double db = 1 + zb.X * zb.X + zb.Y * zb.Y;
                double dd = 2 * (za.X * zb.Y - zb.X * za.Y);

                zo.X = (zb.Y * da - za.Y * db) / dd;
                zo.Y = (za.X * db - zb.X * da) / dd;

                double det = (zb.X - zo.X) * (za.Y - zo.Y) - (za.X - zo.X) * (zb.Y - zo.Y);
                double fa = za.Y * (za.Y - zo.Y) - za.X * (zo.X - za.X);
                double fb = zb.Y * (zb.Y - zo.Y) - zb.X * (zo.X - zb.X);

                zc.X = ((za.Y - zo.Y) * fb - (zb.Y - zo.Y) * fa) / det;
                zc.Y = ((zo.X - za.X) * fb - (zo.X - zb.X) * fa) / det;
            }
        }

        internal void Draw(Graphic g, Color color)
        {
            g.SetColor(color);
            switch (type)
            {
                case LINE:
                    g.DrawLine(a.X, a.Y, b.X, b.Y);
                    break;
                case ARC:
                    g.DrawCurve(a.X, a.Y, c.X, c.Y, b.X, b.Y);
                    break;
                default:
                    break;
            }
        }

        override public String ToString()
        {
            String result = "Geodesic between : " +
                            "\n\t A: " + za +
                            "\n\t B: " + zb +
                            "\n\t is ";
            switch (type)
            {
                case LINE:
                    result += "a line.";
                    break;
                case ARC:
                    result += "an arc.";
                    break;
                default:
                    result += "nothing ?";
                    break;
            }
            return result;
        }

    }
}
