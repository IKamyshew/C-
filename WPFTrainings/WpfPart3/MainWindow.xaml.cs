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

namespace WpfPart3
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
            sld.Value += 5;
        }

        private void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Binding bind = new Binding();
            bind.Source = sld;
            bind.Path = new PropertyPath("Value");
            bind.Mode = BindingMode.TwoWay;
            txb.SetBinding(TextBlock.FontSizeProperty, bind);
        }
    }
}
