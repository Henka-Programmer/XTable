using System.Windows;
using System.Windows.Controls;

namespace XTable.Demo.Views
{
    /// <summary>
    /// Interaction logic for LaptopsTable.xaml
    /// </summary>
    public partial class LaptopsTable : UserControl
    {
        public LaptopsTable()
        {
            InitializeComponent();
            Loaded += LaptopsTable_Loaded;
        }

        private void LaptopsTable_Loaded(object sender, RoutedEventArgs e)
        {
            table.ItemsSource = DataProvider.GetLaptops();
        }
    }
}