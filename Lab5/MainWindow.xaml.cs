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
            if (IntersectionValueText.Text.Length < 3)
            {
                MessageBox.Show("At least 3 points are needed to form a polygon.", "Warning");
            }
        }

        private void CheckConvexityButton_Click(object sender, RoutedEventArgs e)
        {

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
            MainCanvas.Children.Clear();
        }

        
    }
}