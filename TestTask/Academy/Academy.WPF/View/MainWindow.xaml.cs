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
        private string Login { get; set; }
        private CurrentUser User;

        public MainWindow()
        {
            InitializeComponent();

            User = new CurrentUser();
            WindowAuthentication.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnLogin(object sender, RoutedEventArgs e)
        {
            Login = LoginBox.Text;
            string Password = PassBox.Password;
            if (User.ValidateUser(Login, Password))
            {
                Profile profileWin = new Profile(User.GetCurrentUserByLogin(Login));
                profileWin.Show();
                this.Close();
            } else {
                ErrorMsg.Content = "Incorrect login or password";
            }
        }

        private void BtnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
