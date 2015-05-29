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
using Academy.Model.Entities;
using Academy.Model.DBAccess;

namespace Academy.WPF.View
{
    public partial class AddUser : Window
    {
        Entities db;
        User CurrentUser;

        public AddUser(User user)
        {
            CurrentUser = user;
            db = new Entities();

            var groups = db.GetAllGroups();
            var groupsNames = new List<string>();
            var roles = db.GetAllRoles();
            var roleNames = new List<string>();

            foreach (var group in groups)
                groupsNames.Add(group.Name);

            foreach (var role in roles)
                roleNames.Add(role.Name);

            InitializeComponent();

            WindowAddUser.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            CBoxGroupValue.ItemsSource = groupsNames;
            CBoxRoleValue.ItemsSource = roleNames;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            if (BoxFirstNameValue.Text != "" && BoxLastNameValue.Text != "" && BoxLoginValue.Text != ""
                && BoxPasswordValue.Text != "" && CBoxRoleValue.SelectedItem != null && CBoxGroupValue.SelectedItem != null)
            {
                user.FirstName = BoxFirstNameValue.Text;
                user.LastName = BoxLastNameValue.Text;
                user.Login = BoxLoginValue.Text;
                user.Password = BoxPasswordValue.Text;
                user.RoleId = db.GetRole((string)CBoxRoleValue.SelectedItem).Id;
                user.GroupId = db.GetGroupByName((string)CBoxGroupValue.SelectedItem).Id;

                if (db.AddUser(user))
                {
                    ErrorMsg.Foreground = new SolidColorBrush(Colors.Green);
                    ErrorMsg.Content = "User successfully added.";
                }
                else
                {
                    ErrorMsg.Foreground = new SolidColorBrush(Colors.Red);
                    ErrorMsg.Content = "Such Login already exist.";
                }
            } else {
                ErrorMsg.Foreground = new SolidColorBrush(Colors.Red);
                ErrorMsg.Content = "Please fill all fields.";
            }
        }

        //Side Btns
        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanelWin = new AdminPanel(CurrentUser);
            adminPanelWin.Show();
            this.Close();
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Profile ProfileWindow = new Profile(CurrentUser);
            ProfileWindow.Show();
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
