using System.Windows;
using System.Windows.Input;

namespace Lab1_2
{
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

        private void Draw(object sender, RoutedEventArgs e)
        {

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
            }
        }
    }
}