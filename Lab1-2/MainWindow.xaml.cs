using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab1_2.Circle;
using Point = System.Windows.Point;

namespace Lab1_2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
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

            if (Eclipse.IsChecked == true)
            {
                var points = CircleBresenham.Draw(_firstPoint, _secondPoint);
                await DrawCircle(points);
            }

            else if (Circle.IsChecked == true)
            {
                var points = CircleBresenham.Draw(_firstPoint, _secondPoint, true);
                await DrawCircle(points);
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

    /// <summary>
    /// Method for drawing Circle and Eclipse
    /// </summary>
    /// <param name="points">List of points for drawing</param>
    /// <returns></returns>
    private async Task DrawCircle(List<Point> points)
    {
        var polylines = new List<Polyline>();

        for (var i = 0; i < 4; i++)
        {
            var polyline = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            if (_debugMode)
            {
                MainCanvas.Children.Add(polyline);
            }
            for (var index = i; index < points.Count; index += 4)
            {
                if (points[index].X >= 0 && points[index].X <= MainCanvas.ActualWidth &&
                    points[index].Y >= 0 && points[index].Y <= MainCanvas.ActualHeight)
                {
                    polyline.Points.Add(points[index]);
                    if (_debugMode)
                    {
                        await Task.Delay(500);// Adjust delay time as needed
                    }
                    
                }
            }

            polylines.Add(polyline);

            
        }

        if (!_debugMode)
        {
            foreach (var polyline in polylines)
            {
                if (MainCanvas.Children.Contains(polyline)) continue;
                MainCanvas.Children.Add(polyline);
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