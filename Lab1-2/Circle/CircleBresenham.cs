using System.Windows;

namespace Lab1_2.Circle;

public class CircleBresenham
{
    public static List<Point> Draw(Point event1, Point event2, bool circle = false)
    {
        var points = new List<Point>();

        var rx = Math.Abs(event1.X - event2.X) / 2;
        var ry = Math.Abs(event1.Y - event2.Y) / 2;
        var xc = Math.Min(event1.X, event2.X) + rx;
        var yc = Math.Min(event1.Y, event2.Y) + ry;

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