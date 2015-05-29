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
using System.Windows.Shapes;
using Academy.Model.DBAccess;
using Academy.Model.Entities;
using Academy.WPF.View;

namespace Academy.WPF
{
    public partial class Profile : Window
    {
        private User User;

        public Profile(User CurrentUser)
        {
            User = CurrentUser;

            InitializeComponent();

            WindowProfile.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (User.RoleId == 3)
                BtnAdminPanel.Visibility = Visibility.Collapsed;

            BlockFirstName.Text = "First Name: " + User.FirstName;
            BlockLastName.Text = "Last Name: " + User.LastName;
            BlockRole.Text = "Role: " + User.Role.Name;
            BlockGroup.Text = "Group: " + User.Group.Name;

        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanelWin = new AdminPanel(User);
            adminPanelWin.Show();
            this.Close();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow logInWin = new MainWindow();
            logInWin.Show();
            this.Close();
        }
    }
}
