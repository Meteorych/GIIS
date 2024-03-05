using System.Windows;

namespace Lab1_2.Circle;

public class Parabola
{
    public static List<Point> Draw(Point event1, Point event2)
    {
        var x1 = event1.X;
        var y1 = event1.Y;
        var x2 = event2.X;
        var y2 = event2.Y;

        Console.WriteLine(x1 + " " + y1);
        Console.WriteLine(x2 + " " + y2);

        var a = (y2 - y1) / Math.Pow((x2 - x1), 2); 
        var b = -2 * a * x1;
        var c = y1 - a * Math.Pow(x1, 2) - b * x1;

        var x = Math.Min(x1, x2);
        var xMax = Math.Max(x1, x2);
        var yValues = new List<double>();

        while (x <= xMax)
        {
            var y = (a * Math.Pow(x, 2) + b * x + c);
            yValues.Add(y);
            x++;
        }

        var xValues = new List<double>();
        for (var i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
        {
            xValues.Add(i);
        }

        Console.WriteLine(string.Join(", ", xValues));
        var pixels = new List<Point>();

        if (x1 < x2)
        {
            var t = 0;
            for (var i = 0; i < xValues.Count; i++)
            {
                pixels.Add(new Point(xValues[i], yValues[i]));
                if (i > 0)
                {
                    pixels.Add(new Point(xValues[i] - Math.Abs(xValues[0] - xValues[i]) - t, yValues[i]));
                }
                t++;
            }
        }
        else
        {
            var t = 0;
            for (var i = 0; i < xValues.Count; i++)
            {
                pixels.Add(new Point(xValues[i], yValues[i]));
                if (i > 0)
                {
                    pixels.Add(new Point(xValues[^i] + 1 + Math.Abs(xValues[^1] - xValues[0]), yValues[i]));
                }
                t++;
            }
        }

        foreach (var pixel in pixels)
        {
            Console.WriteLine("(" + pixel.X + ", " + pixel.Y + ")");
        }

        return pixels;
    }
}