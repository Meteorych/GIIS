using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1_2.CurveLine
{
    class BSpline
    {
        private readonly Canvas _canvas;
        public BSpline(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Draw(List<Point> controlPoints)
        {
            var n = controlPoints.Count - 1;
            var step = 0.01;

            var polyline = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            

            for (double t = 2; t <= n; t += step)
            {
                var x = 0.0;
                var y = 0.0;

                for (var i = 0; i <= n; i++)
                {
                    var basis = BSplineBasis(i, 3, t);
                    x += basis * controlPoints[i].X;
                    y += basis * controlPoints[i].Y;
                }
                if (x >= 0 && x <= _canvas.ActualWidth &&
                y >= 0 && y <= _canvas.ActualHeight)
                    polyline.Points.Add(new Point(x, y));
            }

            
            _canvas.Children.Add(polyline);
        }

        private double BSplineBasis(int i, int k, double t)
        {
            if (k == 1)
            {
                if (i <= t && t < i + 1)
                    return 1;
                return 0;
            }

            double denominator1 = i + k - 1 - i;
            double denominator2 = i + k - 1 - (i + k - 1 - 1);
            double basis1 = 0, basis2 = 0;
            if (denominator1 != 0)
                basis1 = ((t - i) / denominator1) * BSplineBasis(i, k - 1, t);
            if (denominator2 != 0)
                basis2 = ((i + k - t) / denominator2) * BSplineBasis(i + 1, k - 1, t);
            return basis1 + basis2;
        }
    }
}
