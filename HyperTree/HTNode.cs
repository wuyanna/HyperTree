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
    public interface HTNode
    {
        IEnumerator Children();

        bool IsLeaf();

        String GetName();

        Color GetColor();

        int GetSize();

        int GetBorderSize();

        Image GetImage();

        String GetShortName();

        int GetExtensionCount();
    }
}
