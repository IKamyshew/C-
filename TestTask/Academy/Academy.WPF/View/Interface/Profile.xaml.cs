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
using Academy.WPF.View.Interface;

namespace Academy.WPF
{
    public partial class Profile : Window
    {
        private User LoggedInUser;

        public Profile(User CurrentUser)
        {
            LoggedInUser = CurrentUser;

            InitializeComponent();

            WindowProfile.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (LoggedInUser.RoleId == 3)
                BtnAdminPanel.Visibility = Visibility.Collapsed;

            if (LoggedInUser.RoleId == 1)
                BtnMarks.Visibility = Visibility.Collapsed;

            BlockFirstName.Text = "First Name: " + LoggedInUser.FirstName;
            BlockLastName.Text = "Last Name: " + LoggedInUser.LastName;
            BlockRole.Text = "Role: " + LoggedInUser.Role.Name;
            BlockGroup.Text = "Group: " + LoggedInUser.Group.Name;

        }

        //side buttons
        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanelWin = new AdminPanel(LoggedInUser);
            adminPanelWin.Show();
            this.Close();
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            Academy.WPF.View.Interface.Schedule scheduleWin = new Academy.WPF.View.Interface.Schedule(LoggedInUser);
            scheduleWin.Show();
            this.Close();
        }

        private void BtnMarks_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser.RoleId == 3) { 
                Marks MarkWin = new Marks(LoggedInUser);
                MarkWin.Show();
            } else {
                MarksTeacher MarkWin = new MarksTeacher(LoggedInUser);
                MarkWin.Show();
            }
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
