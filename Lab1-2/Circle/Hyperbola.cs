using System.Windows;

namespace Lab1_2.Circle;

public class Hyperbola
{
    public static List<Point> Draw(Point startPoint, Point finishPoint)
    {
        var a = Math.Abs(finishPoint.X - startPoint.X) / 2;
        var b = Math.Abs(finishPoint.Y - startPoint.Y) / 2;
        var h = (startPoint.X + finishPoint.X) / 2;
        var k = (startPoint.Y + finishPoint.Y) / 2;
        var x = 0;
        var y = b;
        var d = b * b - a * a * b + a * a / 4;

        var points = new List<Point>();

        while (a * a * (2 * y - 1) > 2 * b * b * (x + 1))
        {
            if (d < 0)
            {
                d += b * b * (2 * x + 3);
                x++;
            }
            else
            {
                d += b * b * (2 * x + 3) + a * a * (-2 * y + 2);
                x++;
                y--;
            }
            points.Add(new Point(x + h, -y + k));
            points.Add(new Point(-x + Math.Abs(startPoint.X - finishPoint.X) + h, y - Math.Abs(startPoint.Y - finishPoint.Y) + k));
        }

        d = b * b * (x + 1) * (x + 1) + a * a * (y - 1) * (y - 1) - a * a * b * b;

        while (y > 0)
        {
            if (d < 0)
            {
                d += b * b * (2 * x + 2) + a * a * (-2 * y + 3);
                x++;
                y--;
            }
            else
            {
                d += a * a * (-2 * y + 3);
                y--;
            }
            points.Add(new Point(x + h, -y + k));
            points.Add(new Point(-x + Math.Abs(startPoint.X - finishPoint.X) + h, y - Math.Abs(startPoint.Y - finishPoint.Y) + k));
        }

        return points;
    }
}