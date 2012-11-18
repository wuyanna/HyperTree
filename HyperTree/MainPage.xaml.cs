using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

using HyperTree.demo;

namespace HyperTree
{
    public partial class MainPage : UserControl
    {
        //private HTFileNode root = null;
        private HTXMLNode root = null;
        private HyperTree hypertree = null;

        public MainPage()
        {
            InitializeComponent();

            XDocument xdoc = XDocument.Load("resources/result.xml");

            root = new HTXMLNode(xdoc.Element("xbrldata"));

            hypertree = new HyperTree(root);

            //TODO：是否使用静态方法？
            Graphic graphic = new Graphic();
            //graphic.Canvas = this.LayoutRoot;

            SilverlightHTView view = hypertree.GetSLView();
            graphic.Canvas = view.LayoutRoot;
            view.graphic = graphic;
            view.Repaint();

            this.Width = 500;
            this.Height = 500;
            
            this.LayoutRoot.Children.Add(view);           
        }

     }
}
