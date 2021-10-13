using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PieChartTest.Control
{
    /// <summary>
    /// <para>A Pie Chart.</para>
    /// <para>The radius is half of the width.</para>
    /// </summary>
    public partial class SimplePieChart : UserControl
    {
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SimplePieChart pieChart = (SimplePieChart)obj;
            pieChart.ValueChanged();
        }

        /// <summary>
        /// The Data Points, i.e. the values vs. colors
        /// </summary>
        public IReadOnlyList<DataPoint> DataPoints
        {
            get { return (IReadOnlyList<DataPoint>)GetValue(DataPointsProperty); }
            set { SetValue(DataPointsProperty, value); }
        }

        public static readonly DependencyProperty DataPointsProperty =
            DependencyProperty.Register("DataPoints", typeof(IReadOnlyList<DataPoint>), typeof(SimplePieChart), new PropertyMetadata(Array.Empty<DataPoint>(), ValueChanged));
       
        /// <summary>
        /// Radius of not filled inner circle - e.g. for Rings or Gauges
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(SimplePieChart), new PropertyMetadata(0.0, ValueChanged));
        
        /// <summary>
        /// Start Angle. 0 degree is "North". Useful e.g. for Gauges in combination with the <see cref="EndAngle"/> and <see cref="InnerRadius"/>
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(SimplePieChart), new PropertyMetadata(0.0, ValueChanged));

        /// <summary>
        /// End Angle. 360 degree is "North". Useful e.g. for Gauges in combination with the <see cref="StartAngle"/> and <see cref="InnerRadius"/>
        /// </summary>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(SimplePieChart), new PropertyMetadata(360.0, ValueChanged));

        public SimplePieChart()
        {
            InitializeComponent();
            Loaded += PieChart_Loaded;
            this.SizeChanged += PieChart_SizeChanged;
        }

        private void PieChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RedrawPie();
        }

        private void RedrawPie()
        {
            this.TheGrid.Children.Clear();
            if (this.DataPoints == null || this.DataPoints.Count == 0)
            {
                return;
            }

            var ordered = this.DataPoints.Where(d => d.Value > 0).ToArray();
            var total = this.DataPoints.Sum(d => d.Value);

            double radius = this.RenderSize.Width / 2.0;

            Point center = new Point(this.RenderSize.Width / 2.0, this.RenderSize.Height / 2.0);
            var startPoint = GetPointOnCircle(center, radius, this.StartAngle);

            double startAngle = this.StartAngle;
            foreach (var dataPoint in ordered)
            {
                double currentAngle = dataPoint.Value / total * (this.EndAngle - this.StartAngle);

                double totalAngle = startAngle + currentAngle;
                var pathGeometry = new PathGeometry();

                var segments = new List<PathSegment>();

                var pointA = GetPointOnCircle(center, radius, totalAngle);
                var pointB = GetPointOnCircle(center, this.InnerRadius, totalAngle);
                var pointC = GetPointOnCircle(center, this.InnerRadius, startAngle);

                segments.Add(new ArcSegment(pointA, new Size(radius, radius), 0, currentAngle > 180, SweepDirection.Clockwise, true));
                segments.Add(new LineSegment(pointB, true));

                segments.Add(new ArcSegment(pointC, new Size(this.InnerRadius, this.InnerRadius), 0, currentAngle > 180, SweepDirection.Counterclockwise, true));

                var pathFigure = new PathFigure(startPoint, segments, true);

                pathGeometry.Figures.Add(pathFigure);

                this.TheGrid.Children.Add(new Path { Fill = new SolidColorBrush(dataPoint.Color), StrokeThickness = 0, Data = pathGeometry });

                startPoint = pointA;
                startAngle = totalAngle;
            }
        }

        private static Point GetPointOnCircle (Point center, double radius, double angle)
        {
            double x = center.X + radius * Math.Sin(angle / 180.0 * Math.PI);
            double y = center.Y - radius * Math.Cos(angle / 180.0 * Math.PI);
            var point = new Point(x, y);
            return point;
        }

        private void ValueChanged()
        {
            this.RedrawPie();
        }

        private void PieChart_Loaded(object sender, RoutedEventArgs e)
        {
            this.RedrawPie();
        }
    }
}
