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
using System.Data.Entity;
using Academy.Model.Models;
using Academy.Model.Model;
using Academy.Model.Access;

namespace DesktopToWebApp.WPF
{

    public partial class MainWindow : Window
    {
        private AcademyContext db = new AcademyContext();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Methods model = new Methods();
            var users = model.GetAllUsers();

            Users.ItemsSource = users;
        }
    }
}
