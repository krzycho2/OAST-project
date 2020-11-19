using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mm1Sim
{
    /// <summary>
    /// Interaction logic for ChartMaker.xaml
    /// </summary>
    public partial class ChartMaker : Window
    {
        List<Point> Data;
        public ChartMaker()
        {
            InitializeComponent();

        }

        public void SetData(List<Point> points)
        {
            ((ScatterSeries)ChartData.Series[0]).ItemsSource = points;
        }

        public void SetData(List<double> yValues)
        {
            var points = new List<Point>();
            double space = 1.0 / yValues.Count;
            for (int i = 0; i < yValues.Count; i++)
            {
                double x = i * space;
                points.Add(new Point(x, yValues[i]));
            }

            ((ScatterSeries)ChartData.Series[0]).ItemsSource = points;
        }
        
    }
}
