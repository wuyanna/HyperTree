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

namespace HyperTree
{
    public abstract class AbstractHTNode : HTNode
    {
        public virtual Color GetColor()
        {
            return Colors.White;
        }

        public virtual Image GetImage()
        {
            return null;
        }

        public virtual int GetSize()
        {
            return 0;
        }

        public virtual int GetBorderSize()
        {
            return 1;
        }

        public abstract IEnumerator Children();

        public abstract bool IsLeaf();

        public abstract String GetName();

        public abstract String GetShortName();

        public abstract int GetExtensionCount();
    }
}
