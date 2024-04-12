using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly List<Point> _points = [];
        private Polygon? _currentPolygon;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _points.Add(e.GetPosition(MainCanvas));
            var ellipse = new Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = Brushes.Black
            };
            MainCanvas.Children.Add(ellipse);

            if (_currentPolygon != null)
            {
                MainCanvas.Children.Remove(_currentPolygon);
            }

            _currentPolygon = new Polygon
            {
                Points = new PointCollection(_points),
                Stroke = Brushes.Blue,
                Fill = Brushes.Transparent,
                StrokeThickness = 2
            };
            MainCanvas.Children.Add(_currentPolygon);
        }

        private void CheckPointButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckIntersectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.", "Warning");
            }
        }

        private void CheckConvexityButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.");
                return;
            }

            var isConvex = true;
            var n = _points.Count;
            for (var i = 0; i < n; i++)
            {
                var dx1 = _points[(i + 2) % n].X - _points[(i + 1) % n].X;
                var dy1 = _points[(i + 2) % n].Y - _points[(i + 1) % n].Y;
                var dx2 = _points[i].X - _points[(i + 1) % n].X;
                var dy2 = _points[i].Y - _points[(i + 1) % n].Y;
                var crossProduct = dx1 * dy2 - dy1 * dx2;
                if (crossProduct < 0)
                {
                    isConvex = false;
                    break;
                }
            }

            MessageBox.Show(isConvex ? "The polygon is convex." : "The polygon is not convex");
        }

        private void CalculateNormalsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrahamsConvexHullButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void JarvisConvexHullButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _points.Clear();
            MainCanvas.Children.Clear();
        }

        private List<Point> LinePolygonIntersection(Point point1, Point point2)
        {
            var intersections = new List<Point>();
            var length = _points.Count;
            for (var i = 0; i < length; i++)
            {
                var p1 = _points[i];
                var p2 = _points[(i + 1) % length];
                var intersection = FindIntersection(point1, point2, p1, p2);
                if (intersection != null)
                {
                    intersections.Add(intersection.Value);
                }
            }
            return intersections;
        }

        private static Point? FindIntersection(Point point1, Point point2, Point point3, Point point4)
        {
            var x1 = point1.X;
            var y1 = point1.Y;
            var x2 = point2.X;
            var y2 = point2.Y;
            var x3 = point3.X;
            var y3 = point3.Y;
            var x4 = point4.X;
            var y4 = point4.Y;

            var denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (denominator == 0)
            {
                return null;
            }

            var px = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
            var py = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

            if (Math.Min(x1, x2) <= px && px <= Math.Max(x1, x2) &&
                Math.Min(y1, y2) <= py && py <= Math.Max(y1, y2) &&
                Math.Min(x3, x4) <= px && px <= Math.Max(x3, x4) &&
                Math.Min(y3, y4) <= py && py <= Math.Max(y3, y4))
            {
                return new Point(px, py);
            }

            return null;
        }
    }
}