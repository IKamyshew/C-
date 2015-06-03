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
    public partial class TeacherManageGroups : Window
    {
        private Entities db;
        private User LoggedInUser;
        private Group GroupFilter = null;

        public TeacherManageGroups(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowTGroupManager.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            List<Group> groups = db.GetAllStudentGroups();
            List<string> groupNames = new List<string>();

            foreach (var group in groups)
                groupNames.Add(group.Name);

            CBoxChooseGroup.ItemsSource = groupNames;
            CBoxChangeGroup.ItemsSource = groupNames;
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            string selectedGroupName = (string)CBoxChooseGroup.SelectedItem;
            GroupFilter = db.GetGroupByName(selectedGroupName);

            RefreshTable();
        }

        private void BtnChangeGroup_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxChangeGroup.SelectedItem != null && CBoxChangeStudent.SelectedItem != null)
            {
                User user = db.GetUserByLastName((string)CBoxChangeStudent.SelectedItem);
                Group group = db.GetGroupByName((string)CBoxChangeGroup.SelectedItem);
                user.GroupId = group.Id;
                db.UpdateUser(user);
                RefreshTable();
            }
        }

        private void RefreshTable()
        {
            if (GroupFilter != null) { 
                TableUsers.ItemsSource = db.GetUsersFilteredByRoleAndGroup(3, GroupFilter.Id);
                TableUsers.Items.Refresh();

                RefreshStudentBox();

                List<TeachersGroups> teachers = db.GetTeachersForGroup(GroupFilter.Id);
                string teachersString = "Teachers: ";

                foreach (TeachersGroups teacher in teachers)
                    teachersString += teacher.Teacher.FirstName + " " + teacher.Teacher.LastName + ";";

                LabelTeachers.Content = teachersString;
            }
        }

        private void RefreshStudentBox()
        {
            if (GroupFilter != null) {
                var students = db.GetUsersFilteredByRoleAndGroup(3, GroupFilter.Id);
                var studentsLastNames = new List<string>();

                foreach (var student in students)
                    studentsLastNames.Add(student.LastName);

                CBoxChangeStudent.ItemsSource = studentsLastNames;
            }
        }

        //side buttons
        private void BtnMngTeachers_Click(object sender, RoutedEventArgs e)
        {
            TeacherEditor teacherEditorWin = new TeacherEditor(LoggedInUser);
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
