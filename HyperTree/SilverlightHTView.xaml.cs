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

namespace HyperTree
{
    public partial class SilverlightHTView : UserControl,HTView
    {
        private HTModel model = null;
        private HTDraw draw = null;
        private HTAction action = null;
        private bool fastMode = false;
        private bool longNameMode = false;
        private bool circleMode = false;
        private bool transNotCorrected = false;
        private bool quadMode = false;

        private Image image = null;

        //private string toolTipText;

        public Graphic graphic { get; set; }

        public SilverlightHTView(HTModel model)
        {

            InitializeComponent();  //What does this method do?
            this.model = model;
            draw = new HTDraw(model, this);
            action = new HTAction(draw);
            this.Width = 500;
            this.Height = 500;

            /*TextBlock tiptext = new TextBlock() { TextWrapping = TextWrapping.Wrap };
            tiptext.Text = toolTipText;

            ToolTipService.SetToolTip(this,tiptext);*/
            
            StartMouseListening();
        }

        public void Repaint()
        {
            this.LayoutRoot.Children.Clear();
            PaintComponent(graphic);
        }

        public void PaintComponent(Graphic g)
        {
            draw.RefreshScreenCoordinates();
            draw.DrawBranches(g);
            draw.DrawNodes(g);
        }

        public void StopMouseListening()
        {
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(action.MousePressed);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(action.MouseReleased);
            this.MouseRightButtonUp -= new MouseButtonEventHandler(action.MouseClicked);
            this.MouseMove -= new MouseEventHandler(action.MouseDragged);
            
        }

        public void StartMouseListening()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(action.MousePressed);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(action.MouseReleased);
            this.MouseRightButtonUp += new MouseButtonEventHandler(action.MouseClicked);
            this.MouseMove += new MouseEventHandler(action.MouseDragged);
            //this.MouseMove += new MouseEventHandler(getToolTipText);
        }

        public void TranslateToOrigin(HTNode node)
        {
            HTDrawNode drawNode = draw.FindDrawNode(node);
            draw.TranslateToOrigin(drawNode);
            return;
        }

        /*
         * public void getToolTipText(object sender, MouseEventArgs e)
        {
            int x = (int)e.GetPosition((UIElement)sender).X;
            int y = (int)e.GetPosition((UIElement)sender).Y;

            HTDrawNode node = draw.FindNode(new HTCoordS(x, y));
            if (node != null)
            {
                toolTipText = node.GetName();
            }
            else
            {
                toolTipText = null;
            }
        }*/

        //TODO: set bg img
        /*public void SetImage(Image image)
        {
            this.image = image;
            return;
        }*/

    }
}
