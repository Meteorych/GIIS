using System.Windows;

namespace Lab1_2.Circle
{
    public class HyperbolaDrawing
    {
        public List<Tuple<double, double>> Draw(Point event1, Point event2)
        {
            var a = Math.Abs(event2.X - event1.X) / 2;
            var b = Math.Abs(event2.Y - event1.Y) / 2;
            var h = (event1.X + event2.X) / 2;
            var k = (event1.Y + event2.Y) / 2;
            var x = 0;
            var y = b;
            var d = b * b - a * a * b + a * a / 4;

            var pixels = new List<Tuple<double, double>>();

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
                pixels.Add(new Tuple<double, double>(x + h, -y + k));
                pixels.Add(new Tuple<double, double>(-x + Math.Abs(event1.X - event2.X) + h, y - Math.Abs(event1.Y - event2.Y) + k));
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
                pixels.Add(new Tuple<double, double>(x + h, -y + k));
                pixels.Add(new Tuple<double, double>(-x + Math.Abs(event1.X - event2.X) + h, y - Math.Abs(event1.Y - event2.Y) + k));
            }

            return pixels;
        }
    }
}
