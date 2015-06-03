using Academy.Model.DBAccess;
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

namespace Academy.WPF.View.AdminControls
{
    public partial class TeacherEditor : Window
    {
        private Entities db;
        private User LoggedInUser;
        private Group GroupFilter;

        public TeacherEditor(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowTeacherEditor.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            List<Group> groups = db.GetAllStudentGroups();
            List<string> groupNames = new List<string>();

            foreach (var group in groups)
                groupNames.Add(group.Name);

            CBoxChooseGroup.ItemsSource = groupNames;
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string selectedGroupName = (string)CBoxChooseGroup.SelectedItem;
            GroupFilter = db.GetGroupByName(selectedGroupName);

            RefreshTable();
            RefreshBoxes();
        }

        private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxAddTeacher.SelectedItem != null)
            {
                User user = db.GetUserByLastName((string)CBoxAddTeacher.SelectedItem);
                db.AddTeacherToGroup(GroupFilter.Id, user.Id);

                RefreshTable();
                RefreshBoxes();
            }
        }

        private void BtnRemoveTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxRemoveTeacher.SelectedItem != null)
            {
                User user = db.GetUserByLastName((string)CBoxRemoveTeacher.SelectedItem);
                db.RemoveTeacherToGroup(GroupFilter.Id, user.Id);

                RefreshTable();
                RefreshBoxes();
            }
        }

        private void RefreshBoxes()
        {

            List<User> allTeachers = db.GetAllTeachers();
            List<string> teachersInGroup = new List<string>();
            List<string> teachersOutOfGroup = new List<string>();

            foreach (var teacher in allTeachers)
            {
                if (db.IsTeacherRelatesToGroup(GroupFilter.Id,teacher.Id))
                    teachersInGroup.Add(teacher.LastName);
                else
                    teachersOutOfGroup.Add(teacher.LastName);
            }

            CBoxAddTeacher.ItemsSource = teachersOutOfGroup;
            CBoxRemoveTeacher.ItemsSource = teachersInGroup;
        }

        private void RefreshTable()
        {
            List<User> allTeachers = db.GetAllTeachers();
            if (GroupFilter != null)
            {
                List<User> teacherTable = new List<User>();
                foreach (var teacher in allTeachers)
                {
                    if (db.IsTeacherRelatesToGroup(GroupFilter.Id, teacher.Id))
                        teacherTable.Add(teacher);
                }

                TableUsers.ItemsSource = teacherTable;
                TableUsers.Items.Refresh();
            }
        }

        //side buttons
        private void BtnMngGroups_Click(object sender, RoutedEventArgs e)
        {
            TeacherManageGroups teacherEditorWin = new TeacherManageGroups(LoggedInUser);
            teacherEditorWin.Show();
            this.Close();
        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanelWin = new AdminPanel(LoggedInUser);
            adminPanelWin.Show();
            this.Close();
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Profile ProfileWindow = new Profile(LoggedInUser);
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
