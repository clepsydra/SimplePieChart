
using System.Windows;
using System.Windows.Media;

namespace PieChartTest
{
    using PieChartTest.Control;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dataPoints = new DataPoint[]
            {
                new DataPoint(10, Colors.Green), 
                new DataPoint(20, Colors.Red), 
                new DataPoint(20, Colors.Gray)
            };

            this.TheChart.DataPoints = dataPoints;
        }
    }
}
