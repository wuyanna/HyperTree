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
    public interface HTView
    {
        void StartMouseListening();
        void StopMouseListening();
        void Repaint();
        double Height { get; set; }
        double Width { get; set; }
        Thickness Margin { get; set; }
        //public HTNode GetNodeUnderTheMouse(MouseEventArgs e);
        void TranslateToOrigin(HTNode node);
        //public void SetImage(Image image);
    }
}
