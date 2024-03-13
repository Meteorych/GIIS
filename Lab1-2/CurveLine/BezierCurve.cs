using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1_2.CurveLine
{
    class BezierCurve
    {
        private readonly Canvas _canvas;
        public BezierCurve(Canvas canvas)
        {
            _canvas = canvas;
        }
        public void Draw(List<Point> controlPoints)
        {
            const int n = 3;
            const double step = 0.01;

            var polyline = new Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black
            };

            for (var t = 0.0; t <= 1.0; t += step)
            {
                var x = 0.0;
                var y = 0.0;

                for (var i = 0; i <= n; i++)
                {
                    var coefficient = BinomialCoefficient(n, i) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
                    x += coefficient * controlPoints[i].X;
                    y += coefficient * controlPoints[i].Y;
                }

                polyline.Points.Add(new Point(x, y));
            }

            _canvas.Children.Add(polyline);
        }

        private int BinomialCoefficient(int n, int k)
        {
            var res = 1;
            if (k > n - k)
                k = n - k;
            for (var i = 0; i < k; ++i)
            {
                res *= (n - i);
                res /= (i + 1);
            }
            return res;
        }
    }
}
