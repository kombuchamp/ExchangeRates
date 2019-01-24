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
using WpfExchangeRates.FinanceData;

namespace WpfExchangeRates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO: WPF interface is currently work in progress

            // Create bank instance
            var cbr = new CBR(QueryLanguage.RUS);

            var smth = cbr[1]; // Get currency by index in collection
            var smth2 = cbr["USD"]; // Get currency by charcode
            var smth3 = cbr["US__D"]; // Misspell charcode (returns null)

            // Load currency dynamics between two days
            var dyn = cbr.LoadDynamics(smth, new DateTime(2017, 04, 14), DateTime.Now); 

        }
    }
}
