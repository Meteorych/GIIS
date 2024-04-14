using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GridSize = 2;
        private readonly List<Point> _points = [];
        private Polygon? _currentPolygon;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((FloodFillCheckBox.IsChecked ?? false) || (ScalineCheckBox.IsChecked ?? false))
            {
                MainCanvas.Children.Clear();
                FillPolygon();
                RasterScan(MainCanvas.Children.OfType<Polygon>().ElementAt(0));
                return;
            }

            if ((DrawEdgeRadioButton.IsChecked ?? false) || (DrawActiveEdgeRadioButton.IsChecked ?? false))
            {
                if (!PointInPolygon(e.GetPosition(MainCanvas))) return;
                RasterScan(MainCanvas.Children.OfType<Polygon>().ElementAt(0));
                return;
            }
            
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

        private void RasterScan(Polygon polygon)
        {
            if (_points.Count < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.", "Warning");
                return;
            }

            // Iterate through each edge of the polygon
            for (var i = 0; i < polygon.Points.Count; i++)
            {
                var p1 = polygon.Points[i];
                var p2 = polygon.Points[(i + 1) % polygon.Points.Count];

                // Draw rectangles along the line between p1 and p2
                DrawLineWithRectangles(p1, p2);
            }
        }

        private void DrawLineWithRectangles(Point p1, Point p2)
        {
            // Calculate the length of the line segment
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            var length = Math.Sqrt(dx * dx + dy * dy);

            // Calculate the number of rectangles needed
            var numRectangles = (int)Math.Ceiling(length / (GridSize*2.5));

            // Calculate the increment for each rectangle
            var xIncrement = dx / numRectangles;
            var yIncrement = dy / numRectangles;

            // Draw rectangles for each segment of the line
            for (var i = 0; i < numRectangles; i++)
            {
                var x = p1.X + i * xIncrement;
                var y = p1.Y + i * yIncrement;

                var rect = new Rectangle
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                MainCanvas.Children.Add(rect);
            }
        }

        private void FillPolygon()
        {
            if (_currentPolygon != null) _currentPolygon.Fill = Brushes.Black;
        }

        private void FloodFillPolygon(Point point)
        {
            var xCenter = (int)((point.X / GridSize) * GridSize) + (GridSize / 2);
            var yCenter = (int)((point.Y / GridSize) * GridSize) + (GridSize / 2);

            var stack = new Stack<Point>();
            stack.Push(new Point(xCenter, yCenter));

            var paintedPixels = new HashSet<Point>();

            while (stack.Count > 0)
            {
                var currentPoint = stack.Pop();
                var x = (int)currentPoint.X;
                var y = (int)currentPoint.Y;

                if (!PointInPolygon(currentPoint) ||
                    !paintedPixels.Add(currentPoint)) continue;
                // Example: Fill a rectangle at the current point
                var rect = new Rectangle
                {
                    Width = GridSize,
                    Height = GridSize,
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(rect, x - GridSize / 2);
                Canvas.SetTop(rect, y - GridSize / 2);
                MainCanvas.Children.Add(rect);

                // Add adjacent points to the stack
                stack.Push(new Point(x + GridSize, y));
                stack.Push(new Point(x - GridSize, y));
                stack.Push(new Point(x, y + GridSize));
                stack.Push(new Point(x, y - GridSize));
            }
        }

        private void ScalineFloodFill(Point point)
        {
            var xCenter = (int)((point.X / GridSize) * GridSize) + (GridSize / 2);
            var yCenter = (int)((point.Y / GridSize) * GridSize) + (GridSize / 2);

            var stack = new Stack<Point>();
            stack.Push(new Point(xCenter, yCenter));

            var paintedPixels = new HashSet<Point>();

            while (stack.Count > 0)
            {
                var currentPoint = stack.Pop();
                var x = (int)currentPoint.X;
                var y = (int)currentPoint.Y;

                if (x >= 0 && x < MainCanvas.ActualWidth && y >= 0 && y < MainCanvas.ActualHeight &&
                    paintedPixels.Add(currentPoint))
                {
                    var left = x;
                    var right = x;

                    // Scan left
                    while (left > 0 && !paintedPixels.Contains(new Point(left - GridSize, y)))
                    {
                        left -= GridSize;
                    }

                    // Scan right
                    while (right < MainCanvas.ActualWidth - 1 && !paintedPixels.Contains(new Point(right + GridSize, y)))
                    {
                        right += GridSize;
                    }

                    // Fill the scanline
                    for (var i = left; i <= right; i += GridSize)
                    {
                        var rect = new Rectangle
                        {
                            Width = GridSize,
                            Height = GridSize,
                            Fill = Brushes.Black
                        };
                        Canvas.SetLeft(rect, x - GridSize / 2);
                        Canvas.SetTop(rect, y - GridSize / 2);
                        MainCanvas.Children.Add(rect);

                        // Check for neighbors above and below
                        if (y > 0 && !paintedPixels.Contains(new Point(i, y - GridSize)))
                        {
                            stack.Push(new Point(i, y - GridSize));
                        }

                        if (y < MainCanvas.ActualHeight - GridSize && !paintedPixels.Contains(new Point(i, y + GridSize)))
                        {
                            stack.Push(new Point(i, y + GridSize));
                        }
                    }
                }
            }
        }
        private void CheckPointButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.", "Warning");
                return;
            }

            var pointString = PointValueText.Text;
            var coordinates = pointString.Split(',');
            if (coordinates.Length != 2)
            {
                MessageBox.Show("Invalid point coordinates format. Please enter as 'x, y'.", "Warning");
                return;
            }

            MessageBox.Show(
                PointInPolygon(new Point(double.Parse(coordinates.First()), double.Parse(coordinates.Last())))
                    ? $"The point ({coordinates.First()}, {coordinates.Last()}) is inside the polygon."
                    : $"The point ({coordinates.First()}, {coordinates.Last()}) is outside the polygon.");
        }

        private bool PointInPolygon(Point point)
        {
            if (_points.Count < 3)
                return false;

            var x = point.X;
            var y = point.Y;
            var n = _points.Count;
            var inside = false;
            var p1 = _points[0];
            for (var i = 1; i <= n; i++)
            {
                var p2 = _points[i % n];
                if (y > Math.Min(p1.Y, p2.Y))
                {
                    if (y <= Math.Max(p1.Y, p2.Y))
                    {
                        if (x <= Math.Max(p1.X, p2.X))
                        {
                            if (Math.Round(p1.Y, 2) != Math.Round(p2.Y, 2))
                            {
                                var xinters = (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                                if (Math.Round(p1.X, 2) != Math.Round(p2.X, 2) || x <= xinters)
                                {
                                    inside = !inside;
                                }
                            }
                        }
                    }
                }
                p1 = p2;
            }
            return inside;
        }
        

        private void CheckIntersectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.", "Warning");
            }

            var coordinates = IntersectionValueText.Text.Split(',');
            if (coordinates.Length != 4)
            {
                MessageBox.Show("Invalid line coordinates format. Please enter as 'x1, y1, x2, y2'.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (double.TryParse(coordinates[0], out var x1) && double.TryParse(coordinates[1], out var y1) &&
                double.TryParse(coordinates[2], out var x2) && double.TryParse(coordinates[3], out var y2))
            {
                var intersections = LinePolygonIntersections(new Point(x1, y1), new Point(x2, y2));
                if (intersections.Any())
                {
                    var intersectionPoints = string.Join(", ", intersections.Select(p => $"({Math.Round(p.X, 2)}, {Math.Round(p.Y, 2)})"));
                    MessageBox.Show($"The line intersects the polygon at {intersectionPoints}.", "Intersection Check", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("The line does not intersect the polygon.", "Intersection Check", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Invalid line coordinates format. Please enter as 'x1, y1, x2, y2'.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private List<Point> LinePolygonIntersections(Point point1, Point point2)
        {
            var intersections = new List<Point>();
            var n = _points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = _points[i];
                var p2 = _points[(i + 1) % n];
                var intersection = LineIntersection(point1, point2, p1, p2);
                if (intersection.HasValue)
                {
                    intersections.Add(intersection.Value);
                }
            }
            return intersections;
        }

        private Point? LineIntersection(Point point1, Point point2, Point point3, Point point4)
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
            if (Math.Min(x1, x2) <= px && px <= Math.Max(x1, x2) && Math.Min(y1, y2) <= py && py <= Math.Max(y1, y2) &&
                Math.Min(x3, x4) <= px && px <= Math.Max(x3, x4) && Math.Min(y3, y4) <= py && py <= Math.Max(y3, y4))
            {
                return new Point(px, py);
            }
            return null;
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
                if (!(crossProduct < 0)) continue;
                isConvex = false;
                break;
            }

            MessageBox.Show(isConvex ? "The polygon is convex." : "The polygon is not convex");
        }

        private void CalculateNormalsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                Console.WriteLine("At least 3 points are needed to form a polygon.");
                return;
            }

            var normals = new List<Vector>();
            var n = _points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = _points[i];
                var p2 = _points[(i + 1) % n];
                var normal = new Vector(p1.Y - p2.Y, p2.X - p1.X);
                normals.Add(normal);
            }

            var normalsInfo = "The normals are:\n";
            foreach (var normal in normals) 
            {
                normalsInfo += $"({Math.Round(normal.X, 2)}; {Math.Round(normal.Y, 2)})\n";
            }

            MessageBox.Show(normalsInfo);
        }

        private void GrahamsConvexHullButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                Console.WriteLine("At least 3 points are needed to form a polygon.");
                return;
            }
            var sortedPoints = _points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            MainCanvas.Children.Clear();
            var stack = new Stack<Point>();

            foreach (var point in sortedPoints)
            {
                while (stack.Count >= 3 && Orientation(stack.ElementAt(stack.Count - 2), stack.Peek(), point) <= 0)
                {
                    stack.Pop();
                }
                stack.Push(point);
            }

            var convexHull = stack.ToList();
            var polygon = new Polygon
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Points = new PointCollection(convexHull)
            };
            MainCanvas.Children.Add(polygon);
        }

        private static int Orientation(Point p, Point q, Point r)
        {
            var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0;
            }

            return (val > 0) ? 1 : -1;
        }

        private void JarvisConvexHullButton_Click(object sender, RoutedEventArgs e)
        {
            if (_points.Count < 3)
            {
                Console.WriteLine("At least 3 points are needed to form a polygon.");
                return;
            }
            var hull = new List<Point>();
            var startPoint = _points.OrderBy(p => p.X).ThenBy(p => p.Y).First();
            var currentPoint = startPoint;

            while (true)
            {
                hull.Add(currentPoint);
                var endpoint = _points[0];
                foreach (var point in _points)
                {
                    if (endpoint == currentPoint || Orientation(currentPoint, point, endpoint) == -1)
                    {
                        endpoint = point;
                    }
                }
                currentPoint = endpoint;
                if (currentPoint == startPoint)
                {
                    break;
                }
            }

            MessageBox.Show("Convex hull found.", "Convex Hull", MessageBoxButton.OK, MessageBoxImage.Information);
            var polygon = new Polygon
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Points = new PointCollection(hull)
            };
            MainCanvas.Children.Add(polygon);
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