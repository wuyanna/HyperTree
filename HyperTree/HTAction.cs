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
    public class HTAction
    {

        private HTDraw model = null;
        private HTCoordE startPoint = null;
        private HTCoordE endPoint = null;
        private HTCoordS clickPoint = null;
        private bool isDragging = false;

        public HTAction(HTDraw model)
        {
            this.model = model;
            startPoint = new HTCoordE();
            endPoint = new HTCoordE();
            clickPoint = new HTCoordS();
        }

        public void MousePressed(object sender, MouseEventArgs e)
        {
            // TODO: is Shift down -> fastMode=true
            Debug.WriteLine("MousePressed!!");
            startPoint.ProjectionStoE((int)e.GetPosition((UIElement)sender).X, (int)e.GetPosition((UIElement)sender).Y, 
                                        model.GetSOrigin(), model.GetSMax());
            isDragging = true;
        }

        public void MouseReleased(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("MouseReleased!!");
            model.EndTranslation();
            isDragging = false;
        }

        public void MouseClicked(object sender, MouseEventArgs e)
        {
            // TODO: is shift down -> model.restore()
            Debug.WriteLine("MouseClicked!!");
            clickPoint.X = (int)e.GetPosition((UIElement)sender).X;
            clickPoint.Y = (int)e.GetPosition((UIElement)sender).Y;

            HTDrawNode node = model.FindNode(clickPoint);
            if (node != null)
            {
                model.TranslateToOrigin(node);
            }
        }

        public void MouseDragged(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                //Debug.WriteLine("MouseDragged!!");
                if (startPoint.IsValid())
                {
                    endPoint.ProjectionStoE((int)e.GetPosition((UIElement)sender).X, (int)e.GetPosition((UIElement)sender).Y,
                                                model.GetSOrigin(), model.GetSMax());
                    if (endPoint.IsValid())
                    {
                        model.Translate(startPoint, endPoint);
                    }
                }
            }
        }


    }
}
