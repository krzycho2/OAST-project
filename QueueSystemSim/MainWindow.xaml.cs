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

namespace QueueSystemSim
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
            double[] roValues = new double[] { 0.25, 0.5, 0.75 }; 
            double paramLambda = 4; 

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);
                queueSystem.IsContinuous = true;

                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();

                Console.WriteLine("Symulacja dla Ro := " + paramRo);
                Console.WriteLine($"Lambda: {paramLambda}, Mi: {paramMi}");
                Console.WriteLine($"ET: oczekiwane: {queueSystem.ExpectedET}, doświadczalne: {simulator.GlobalStats.ET}, błąd: {error(queueSystem.ExpectedET, simulator.GlobalStats.ET)}");
                Console.WriteLine($"EN: oczekiwane: {queueSystem.ExpectedEN}, doświadczalne: {simulator.GlobalStats.EN}, błąd: {error(queueSystem.ExpectedEN, simulator.GlobalStats.EN)}");
                Console.WriteLine($"EW: oczekiwane: {queueSystem.ExpectedEW}, doświadczalne: {simulator.GlobalStats.EW}, błąd: {error(queueSystem.ExpectedEW, simulator.GlobalStats.EW)}");
                Console.WriteLine($"EQ: oczekiwane: {queueSystem.ExpectedEQ}, doświadczalne: {simulator.GlobalStats.EQ}, błąd: {error(queueSystem.ExpectedEQ, simulator.GlobalStats.EQ)}");
                Console.WriteLine();
            }

        }

        double error(double expected, double real)
        {
            return Math.Abs(expected - real) / expected;
        }
    }
}
