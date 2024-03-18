using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab1_2.Circle;
using Lab1_2.CurveLine;
using Point = System.Windows.Point;

namespace Lab1_2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// TODO: Architecture of this program is crippled shit...Too much ifs, no cohesion, absolutely no unity between different methods, absolutely no polymorphism
public partial class MainWindow : Window
{
    private Point _firstPoint;
    private Point _secondPoint;
    private int _pointCount;
    private bool _debugMode = false;

    public MainWindow()
    {
        InitializeComponent();
        DebugModeButton.Background = Brushes.Red;
    }

    private async void Canvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_pointCount == 0)
        {
            _firstPoint = e.GetPosition(MainCanvas);
            _pointCount++;
        }
        else
        {
            _secondPoint = e.GetPosition(MainCanvas);
            _pointCount = 0;

            if (Ellipse.IsChecked == true)
            {
                var circleDrawing = new CircleBresenham(MainCanvas);
                await circleDrawing.Draw(_firstPoint, _secondPoint, false, _debugMode);
            }

            else if (Circle.IsChecked == true)
            {
                var circleDrawing = new CircleBresenham(MainCanvas);
                await circleDrawing.Draw(_firstPoint, _secondPoint, true, _debugMode);
            }
            else if (ParabolaButton.IsChecked == true)
            {
                var points = Parabola.Draw(_firstPoint, _secondPoint);
                await DrawFigure(points);
            }
            else if (HyperbolaButton.IsChecked == true)
            {
                var points = Hyperbola.Draw(_firstPoint, _secondPoint);
                await DrawFigure(points);
            }
            else if (CurveBezier.IsChecked == true)
            {
                var bezierCurve = new BezierCurve(MainCanvas);
                bezierCurve.Draw([
                    _firstPoint, new Point(_firstPoint.X + 100, _secondPoint.Y + 100), _secondPoint,
                    _secondPoint with { X = _secondPoint.X + 200 }
                ]);
            }
            else if (CurveHermit.IsChecked == true)
            {
                var hermitCurve = new HermitCurve(MainCanvas);
                hermitCurve.Draw(_firstPoint, new Point(_firstPoint.X + 50, _firstPoint.Y + 100), _secondPoint with { X = _secondPoint.X -50, Y = _secondPoint.Y - 100 },
                    _secondPoint);
            }
            else if (BSplineButton.IsChecked == true)
            {
                var bSpline = new BSpline(MainCanvas);
                bSpline.Draw([_firstPoint, new Point(_firstPoint.X + 50, _firstPoint.Y + 100), _secondPoint with { X = _secondPoint.X - 50, Y = _secondPoint.Y - 100 },
                    _secondPoint]);
            }
        }
    }

    /// <summary>
    /// Method for drawing Parabola and Hyperbola
    /// </summary>
    /// <param name="points">List of points for drawing</param>
    /// <returns></returns>
    private async Task DrawFigure(List<Point> points)
    {
        var polyline1 = new Polyline()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        var polyline2 = new Polyline()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        if (_debugMode)
        {
            MainCanvas.Children.Add(polyline1);
            MainCanvas.Children.Add(polyline2);
        }
        
        for (var index = 2; index < points.Count - 2; index += 2)
        {
            if (!(points[index].X >= 0) || !(points[index].X <= MainCanvas.ActualWidth) ||
                !(points[index].Y >= 0) || !(points[index].Y <= MainCanvas.ActualHeight)) continue;

            polyline1.Points.Add(points[index]);
            polyline1.Points.Add(points[index + 2]);

            if (_debugMode)
            {
                await Task.Delay(500); // Adjust delay time as needed
            }
        }

        for (var index = 1; index < points.Count - 2; index += 2)
        {
            if (!(points[index].X >= 0) || !(points[index].X <= MainCanvas.ActualWidth) ||
                !(points[index].Y >= 0) || !(points[index].Y <= MainCanvas.ActualHeight)) continue;

            polyline2.Points.Add(points[index]);
            polyline2.Points.Add(points[index + 2]);

            if (_debugMode)
            {
                await Task.Delay(500); // Adjust delay time as needed
            }
        }

        if (!_debugMode)
        {
            if (!MainCanvas.Children.Contains(polyline1))
            {
                MainCanvas.Children.Add(polyline1);
            }

            if (!MainCanvas.Children.Contains(polyline2))
            {
                MainCanvas.Children.Add(polyline2);
            }
        }
    }


    private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
    {
        MainCanvas.Children.Clear();
    }

    private void DebugMode_ButtonClick(object sender, RoutedEventArgs e)
    {
        if (_debugMode is false)
        {
            DebugModeButton.Background = Brushes.LightGreen;
            _debugMode = true;
        }
        else
        {
            DebugModeButton.Background = Brushes.Red;
            _debugMode = false;
        }
    }
}