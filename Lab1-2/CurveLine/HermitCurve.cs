using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1_2.CurveLine
{
    class HermitCurve
    {
        private readonly Canvas _canvas;

        public HermitCurve(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Draw(Point startPoint, Point endPoint, Point startTangent, Point endTangent)
        {
            var polyline = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            var points = new List<Point>();
            var step = 0.01;

            for (double t = 0; t <= 1.0; t += step)
            {
                var x = CalculateHermiteValue(startPoint.X, startTangent.X, endPoint.X, endTangent.X, t);
                var y = CalculateHermiteValue(startPoint.Y, startTangent.Y, endPoint.Y, endTangent.Y, t);
                points.Add(new Point(x, y));
            }

            polyline.Points = new PointCollection(points);
            _canvas.Children.Add(polyline);
        }

        private double CalculateHermiteValue(double p0, double t0, double p1, double t1, double t)
        {
            var t2 = t * t;
            var t3 = t2 * t;
            var h1 = 2 * t3 - 3 * t2 + 1;
            var h2 = -2 * t3 + 3 * t2;
            var h3 = t3 - 2 * t2 + t;
            var h4 = t3 - t2;

            return h1 * p0 + h2 * p1 + h3 * t0 + h4 * t1;
        }
    }
}

