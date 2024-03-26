using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1_2.Circle;

public class CircleBresenham
{
    private readonly Canvas _canvas;

    public CircleBresenham(Canvas canvas)
    {
        _canvas = canvas;
    }
    /// <summary>
    /// Method for drawing Circle or Ellipse.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="circle"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public async Task Draw(Point startPoint, Point endPoint, bool circle = false, bool mode = false)
    {
        var points = FindPoints(startPoint, endPoint, circle); 
        var polylines = new List<Polyline>();
        
        for (var i = 0; i < 4; i++)
        {
            var polyline = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            if (mode)
            {
                _canvas.Children.Add(polyline);
            }
            for (var index = i; index < points.Count; index += 4)
            {
                if (points[index].X >= 0 && points[index].X <= _canvas.ActualWidth &&
                    points[index].Y >= 0 && points[index].Y <= _canvas.ActualHeight)
                {
                    polyline.Points.Add(points[index]);
                    if (mode)
                    {
                        await Task.Delay(500);// Adjust delay time as needed
                    }

                }
            }

            polylines.Add(polyline);
        }

        if (!mode)
        {
            foreach (var polyline in polylines)
            {
                if (_canvas.Children.Contains(polyline)) continue;
                _canvas.Children.Add(polyline);
            }
        }
    }

    private static List<Point> FindPoints(Point startPoint, Point endPoint, bool circle = false)
    {
        var points = new List<Point>();

        var rx = Math.Abs(startPoint.X - endPoint.X) / 2;
        var ry = Math.Abs(startPoint.Y - endPoint.Y) / 2;
        var xc = Math.Min(startPoint.X, endPoint.X) + rx;
        var yc = Math.Min(startPoint.Y, endPoint.Y) + ry;

        if (circle)
        {
            ry = rx;
        }

        var x = 0;
        var y = ry;

        // Initial decision parameter of region 1
        var d1 = ry * ry - rx * rx * ry + rx * rx / 4;
        var dx = 2 * ry * ry * x;
        var dy = 2 * rx * rx * y;

        // For region 1
        while (dx < dy)
        {
            // Print points based on 4-way symmetry
            points.Add(new Point(x + xc, y + yc));
            points.Add(new Point(-x + xc, y + yc));
            points.Add(new Point(x + xc, -y + yc));
            points.Add(new Point(-x + xc, -y + yc));

            // Checking and updating value of
            // decision parameter based on algorithm
            if (d1 < 0)
            {
                x++;
                dx += 2 * ry * ry;
                d1 += dx + ry * ry;
            }
            else
            {
                x++;
                y--;
                dx += 2 * ry * ry;
                dy -= 2 * rx * rx;
                d1 += dx - dy + ry * ry;
            }
        }

        // Decision parameter of region 2
        var d2 = ry * ry * (x + 0.5) * (x + 0.5) + rx * rx * (y - 1) * (y - 1) - rx * rx * ry * ry;

        // Plotting points of region 2
        while (y >= 0)
        {
            // Print points based on 4-way symmetry
            points.Add(new Point(x + xc, y + yc));
            points.Add(new Point(-x + xc, y + yc));
            points.Add(new Point(x + xc, -y + yc));
            points.Add(new Point(-x + xc, -y + yc));

            // Checking and updating parameter
            // value based on algorithm
            if (d2 > 0)
            {
                y--;
                dy -= 2 * rx * rx;
                d2 += rx * rx - dy;
            }
            else
            {
                y--;
                x++;
                dx += 2 * ry * ry;
                dy -= 2 * rx * rx;
                d2 += dx - dy + rx * rx;
            }
        }

        return points;
    }

}