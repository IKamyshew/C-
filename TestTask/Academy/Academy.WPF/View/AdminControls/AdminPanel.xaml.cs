using Academy.Model.Entities;
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
using Academy.WPF.View.AdminControls;

namespace Academy.WPF.View
{
    public partial class AdminPanel : Window
    {
        private User User;
        private Entities db;
        private List<User> users;
        private Group GroupFilter = null;
        private Role RoleFilter = null;


        public AdminPanel(User CurrentUser)
        {
            User = CurrentUser;
            db = new Entities();

            var groups = db.GetAllGroups();
            var groupsNames = new List<string>();
            groupsNames.Add("All");
            var roles = db.GetAllRoles();
            var roleNames = new List<string>();
            roleNames.Add("All");

            foreach (var group in groups)
                groupsNames.Add(group.Name);

            foreach (var role in roles)
                roleNames.Add(role.Name);

            InitializeComponent();


            WindowAdminPanel.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            RefreshTable();
            CBoxChooseGroup.ItemsSource = groupsNames;
            CBoxChooseRole.ItemsSource = roleNames;

            if (User.RoleId == 1)
            {
                BtnTeacherManageGroups.Visibility = Visibility.Collapsed;
                RefreshComboBoxes();
            }

            if (User.RoleId == 2)
            {
                BtnAddUser.Visibility = Visibility.Collapsed;
                BtnManageGroups.Visibility = Visibility.Collapsed;
                BtnManageSubject.Visibility = Visibility.Collapsed;

                BtnEdit.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
                CBoxEditUser.Visibility = Visibility.Collapsed;
                CBoxDeleteUser.Visibility = Visibility.Collapsed;

                TableUsers.Columns[4].Visibility = Visibility.Collapsed;
                TableUsers.Columns[5].Visibility = Visibility.Collapsed;
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string selectedRoleName = (string) CBoxChooseRole.SelectedItem;
            if (selectedRoleName == "All")
                RoleFilter = null;
            else
                RoleFilter = db.GetRole(selectedRoleName);

            string selectedGroupName = (string)CBoxChooseGroup.SelectedItem;
            if (selectedGroupName == "All")
                GroupFilter = null;
            else
                GroupFilter = db.GetGroupByName(selectedGroupName);
            
            RefreshTable();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxEditUser.SelectedItem != null) { 
                string selectedLastName = (string) CBoxEditUser.SelectedItem;
                User SelectedUser = db.GetUserByLastName(selectedLastName);
                Edit EditWindow = new Edit(User, SelectedUser);
                EditWindow.Show();
                this.Close();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxDeleteUser.SelectedItem != null)
            { 
                string selectedLastName = (string)CBoxDeleteUser.SelectedItem;
                User SelectedUser = db.GetUserByLastName(selectedLastName);
                db.DeleteUser(SelectedUser);

                RefreshTable();
                RefreshComboBoxes();
            }
        }

        //side buttons
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser EditWindow = new AddUser(User);
            EditWindow.Show();
            this.Close();
        }

        private void BtnManageSubject_Click(object sender, RoutedEventArgs e)
        {
            ManageSubjects MSWindow = new ManageSubjects();
            MSWindow.Show();
            this.Close();
        }

        private void BtnTeacherManageGroups_Click(object sender, RoutedEventArgs e)
        {
            TeacherManageGroups TMGWindow = new TeacherManageGroups();
            TMGWindow.Show();
            this.Close();
        }
        
        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Profile ProfileWindow = new Profile(User);
            ProfileWindow.Show();
            this.Close();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow logInWin = new MainWindow();
            logInWin.Show();
            this.Close();
        }

        //Methods
        private void RefreshTable()
        {
            if (GroupFilter == null && RoleFilter == null)
                TableUsers.ItemsSource = db.GetAllUsers();
            else
                if (GroupFilter == null && RoleFilter != null)
                    TableUsers.ItemsSource = db.GetUsersFilteredByRole(RoleFilter.Id);
                else
                    if (GroupFilter != null && RoleFilter == null)
                        TableUsers.ItemsSource = db.GetUsersFilteredByGroup(GroupFilter.Id);
                    else
                        TableUsers.ItemsSource = db.GetUsersFilteredByRoleAndGroup(RoleFilter.Id, GroupFilter.Id);
            TableUsers.Items.Refresh();
        }

        public void RefreshComboBoxes()
        {
            users = db.GetAllUsers();
            var usersLastNames = new List<string>();

            foreach (var user in users)
                usersLastNames.Add(user.LastName);
            CBoxEditUser.ItemsSource = usersLastNames;
            CBoxDeleteUser.ItemsSource = usersLastNames;
        }
    }
}
