namespace PieChartTest.Control
{
    using System.Windows.Media;

    public class DataPoint
    {
        public DataPoint(double value, Color color)
        {
            this.Value = value;
            this.Color = color;
        }

        public double Value { get; }

        public Color Color { get; }
    }
}