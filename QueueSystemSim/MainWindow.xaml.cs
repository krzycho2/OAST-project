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
            //double[] roValues = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9};
            double[] roValues = new double[] { 0.25, 0.5, 0.75 }; 
            double paramLambda = 1; 

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);
                //var queueSystem = new Mm1ContinuousService(paramMi, paramLambda);
                var simulator = new MultiSimulator(queueSystem);

                simulator.RunSim();

                Console.WriteLine("Symulacja dla Ro := " + paramRo);
                Console.WriteLine($"Lambda: {paramLambda}, Mi: {paramMi}");
                Console.WriteLine($"ET: oczekiwane: {queueSystem.ExpectedET}, doświadczalne: {queueSystem.Stats.ET}, błąd: {error(queueSystem.ExpectedET, queueSystem.Stats.ET)}");
                Console.WriteLine($"EN: oczekiwane: {queueSystem.ExpectedEN}, doświadczalne: {queueSystem.Stats.EN}, błąd: {error(queueSystem.ExpectedEN, queueSystem.Stats.EN)}");
                Console.WriteLine($"EW: oczekiwane: {queueSystem.ExpectedEW}, doświadczalne: {queueSystem.Stats.EW}, błąd: {error(queueSystem.ExpectedEW, queueSystem.Stats.EW)}");
                Console.WriteLine($"EQ: oczekiwane: {queueSystem.ExpectedEQ}, doświadczalne: {queueSystem.Stats.EQ}, błąd: {error(queueSystem.ExpectedEQ, queueSystem.Stats.EQ)}");
                Console.WriteLine();
            }
            

            
        }

        double error(double expected, double real)
        {
            return Math.Abs(expected - real) / expected;
        }
    }
}
