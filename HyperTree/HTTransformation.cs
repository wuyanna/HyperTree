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
    public class HTTransformation
    {
        internal HTCoordE P = null;
        internal HTCoordE O = null;

        internal HTTransformation()
        {
            P = new HTCoordE();
            O = new HTCoordE();
        }

        internal void Composition(HTCoordE first, HTCoordE second)
        {
            P.X = first.X + second.X;
            P.Y = first.Y + second.Y;

            HTCoordE d = new HTCoordE(second);
            d.Y = -d.Y;
            d.Multiply(first);
            d.X += 1;
            P.Divide(d);

            O.X = first.X;
            O.Y = -first.Y;
            O.Multiply(second);
            O.X += 1;
            O.Divide(d);
        }

        override public String ToString()
        {
            String result = "Transformation : " +
                            "\n\tP = " + P +
                            "\n\tO = " + O;
            return result;
        }
    }
}
