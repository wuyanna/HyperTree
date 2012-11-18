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
    public class HyperTree
    {
        private HTModel model = null;

        public HyperTree(HTNode root)
        {
            model = new HTModel(root);
        }

        public SilverlightHTView GetSLView()
        {
            return new SilverlightHTView(model);
        }
    }
}
