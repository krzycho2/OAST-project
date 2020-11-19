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

namespace Mm1Sim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double[] paramsRo = new double[] { 0.25, 0.5, 0.75 };
            foreach(double ro in paramsRo)
            {
                var queue = new Mm1Queue(ro, 0.1);
                queue.GenerateEpisodes();
                double mi = queue.DepartureParamMi;
                double lam = queue.ArrivalParamLambda;
                double expET = 1.0 / (mi - lam);
                double ET = queue.MeanServiceTime;
                double ETerr = Math.Abs(ET - expET) / expET;
                double expEN = lam / (mi - lam);
                double EN = queue.MeanClientsCount;
                double ENerr = Math.Abs(EN - expEN) / expEN;
                Console.WriteLine($"Ro: {ro}, Mi: {mi}, Lambda: {lam}");
                Console.WriteLine($"E[T]: {ET}, oczekiwane E[T]: {expET}, błąd: {ETerr}");
                Console.WriteLine($" E[N]: {EN}, oczekiwane E[N]: {expEN}, błąd: {ENerr}");
                Console.WriteLine();
            }
            

            //var chartMaker = new ChartMaker();
            //chartMaker.SetData(timesOfArival.ToList());
            //chartMaker.Show();

        }
    }
}
