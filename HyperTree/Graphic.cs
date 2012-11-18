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
    public class Graphic
    {
        private Color _color = Colors.Black;
        public Canvas Canvas { get; set; }

        //set line color
        public void SetColor(Color color)
        {
            _color = color;
        }

        public void DrawDot(int x, int y, int radius, Color color)
        {
            Ellipse dot = new Ellipse();
            dot.Width = 2 * radius;
            dot.Height = 2 * radius;
            dot.SetValue(Canvas.LeftProperty, (double)(x - radius));
            dot.SetValue(Canvas.TopProperty, (double)(y - radius));
            dot.Fill = new SolidColorBrush(color);
            Canvas.Children.Add(dot);
        }

        public void DrawLabel(int x, int y, int width, string text)
        {
            DrawLabel(x, y, width, 40, text, Colors.White);
        }

        public void DrawLabel(int x, int y, int width, int height, string text, Color color)
        {
            Label label = new Label();
            label.Content = text;
            label.SetValue(Canvas.LeftProperty, (double)x);
            label.SetValue(Canvas.TopProperty, (double)y);
            label.Background = new SolidColorBrush(Colors.Transparent);
            label.Foreground = new SolidColorBrush(color);
            if (label.Width > width)
            {
                label.Width = width;
            }
            //label.Width = 20;
            label.Height = height;

            ToolTipService.SetToolTip(label, text);

            Canvas.Children.Add(label);
        }


        public void DrawLine(int x1, int y1, int x2, int y2) {
            Path path = new Path() { Stroke = new SolidColorBrush(_color), StrokeThickness = 1 };

            LineSegment line = new LineSegment() { Point = new Point(x2, y2) };

            PathFigure figure = new PathFigure() { StartPoint = new Point(x1, y1) };
            figure.Segments.Add(line);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            path.Data = geometry;
            
            Canvas.Children.Add(path);
            //Debug.WriteLine("Line Start Point: ( " + x1 + ", " + y1 + ")");
            //Debug.WriteLine("Line End Point: ( " + x2 + ", " + y2 + ")");
        }

        public void DrawCurve(int x1, int y1, int ctrlx, int ctrly, int x2, int y2)
        {
            Path path = new Path() { Stroke = new SolidColorBrush(_color), StrokeThickness = 1 };

            QuadraticBezierSegment bezier = new QuadraticBezierSegment() { Point1 = new Point(ctrlx, ctrly), Point2 = new Point(x2, y2) };

            PathFigure figure = new PathFigure() { StartPoint = new Point(x1, y1) };
            figure.Segments.Add(bezier);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            path.Data = geometry;

            Canvas.Children.Add(path);
            //Debug.WriteLine("Curve Start Point: ( " + x1 + ", " + y1 + ")");
            //Debug.WriteLine("Curve End Point: ( " + x2 + ", " + y2 + ")");
        }

    }
}
