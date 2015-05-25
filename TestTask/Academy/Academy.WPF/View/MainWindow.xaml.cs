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
using Academy.Model.DBAccess;

namespace Academy.WPF
{
    public partial class MainWindow : Window
    {
        public string Login { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            WindowAuthentication.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnLogin(object sender, RoutedEventArgs e)
        {
            Login = LoginBox.Text;
            string Password = PassBox.Password;
            CurrentUser user = new CurrentUser();
            if (user.ValidateUser(Login, Password))
            {
                Profile profileWin = new Profile(Login);
                profileWin.Show();
                this.Close();
            }
        }

        private void BtnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
