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

        private void OnStart(object sender, RoutedEventArgs e)
        {
            var assignemnet = new Assignment
            {
                Do1_1 = (bool)C1.IsChecked,
                Do1_2 = (bool)C2.IsChecked,
                Do2_1 = (bool)C3.IsChecked,
                Do2_2 = (bool)C4.IsChecked,
                Do2_3 = (bool)C5.IsChecked,
                OutputToFile = (bool)C6.IsChecked,
            };

            assignemnet.Run();
        }


    }
}
