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
    

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Canvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
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
                DrawCircle(points);
            }

            else if (Circle.IsChecked == true)
            {
                var points = CircleBresenham.Draw(_firstPoint, _secondPoint, true);
                DrawCircle(points);
            }
            else if (ParabolaButton.IsChecked == true)
            {
                var points = Parabola.Draw(_firstPoint, _secondPoint);
                DrawFigure(points);
            }
            else if (HyperbolaButton.IsChecked == true)
            {
                var points = Hyperbola.Draw(_firstPoint, _secondPoint);
                DrawFigure(points);
            }
        }
    }

    private void DrawFigure(List<Point> points)
    {
        var polyline = new Polyline()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        for (var index = 2; index < points.Count-2; index+=2)
        {
            if (!(points[index].X >= 0) || !(points[index].X <= MainCanvas.ActualWidth) ||
                !(points[index].Y >= 0) || !(points[index].Y <= MainCanvas.ActualHeight)) continue;
            
            polyline.Points.Add(points[index]);
            polyline.Points.Add(points[index + 2]);
        }
        MainCanvas.Children.Add(polyline);
        polyline = new Polyline()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        for (var index = 1; index < points.Count-2; index += 2)
        {
            if (!(points[index].X >= 0) || !(points[index].X <= MainCanvas.ActualWidth) ||
                !(points[index].Y >= 0) || !(points[index].Y <= MainCanvas.ActualHeight)) continue;
            
            polyline.Points.Add(points[index]);
            polyline.Points.Add(points[index + 2]);
        }
        MainCanvas.Children.Add(polyline);
    }

    private void DrawCircle(List<Point> points)
    {
        var polylines = new List<Polyline>();

        for (var i = 0; i < 4; i++)
        {
            var polyline = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            for (var index = i; index < points.Count; index += 4)
            {
                if (points[index].X >= 0 && points[index].X <= MainCanvas.ActualWidth &&
                    points[index].Y >= 0 && points[index].Y <= MainCanvas.ActualHeight)
                {
                    polyline.Points.Add(points[index]);
                }
            }

            polylines.Add(polyline);
        }

        foreach (var polyline in polylines)
        {
            MainCanvas.Children.Add(polyline);
        }
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MainCanvas.Children.Clear();
    }
}