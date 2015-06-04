using Academy.Model.DBAccess;
using Academy.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Academy.WPF.View.Interface
{
    public partial class AddMark : Window
    {
        private Entities db;
        private User LoggedInUser;
        private Group GroupFilter = null;
        private Subject SubjectFilter = null;

        public AddMark(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowAddMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //CBoxes groups and subjects
            List<Group> groups = db.GetAllStudentGroups();
            List<Subject> subjects = db.GetAllSubjects();

            List<string> groupsNames = new List<string>();
            List<string> subjectsNames = new List<string>();

            foreach (var group in groups)
                groupsNames.Add(group.Name);

            foreach (var subject in subjects)
                subjectsNames.Add(subject.Name);

            CBoxChooseGroup.ItemsSource = groupsNames;
            CBoxChooseSubject.ItemsSource = subjectsNames;

            CBoxChooseMark.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
        }

        private void GroupAndSubject_Choosed(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxChooseGroup.SelectedItem == null || CBoxChooseSubject.SelectedItem == null)
                return;

            GroupFilter = db.GetGroupByName((string)CBoxChooseGroup.SelectedItem);
            SubjectFilter = db.GetSubjectByName((string)CBoxChooseSubject.SelectedItem);

            // CBox students
            List<User> students = db.GetAllStudentsForGroup(GroupFilter.Id);
            List<string> studentNames = new List<string>();
            foreach (var student in students)
                studentNames.Add(student.LastName);
            CBoxChooseStudent.ItemsSource = studentNames;
        }

        private void BtnAddMark_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxChooseStudent.SelectedItem != null || CBoxChooseSubject.SelectedItem != null || CBoxChooseMark.SelectedItem != null) { 
                Mark newMark = new Mark();
                newMark.StudentId = (db.GetUserByLastName((string)CBoxChooseStudent.SelectedItem)).Id;
                newMark.SubjectId = (db.GetSubjectByName((string)CBoxChooseSubject.SelectedItem)).Id;
                try { 
                    newMark.Date = DateTime.ParseExact((string)TBoxChooseDate.Text, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                } catch (FormatException) {
                    ErrorMsg.Foreground = new SolidColorBrush(Colors.Red);
                    ErrorMsg.Content = "Please fill \"Data\" field in MM/dd/yyyy format";
                    return;
                }
                newMark.MarkValue = (int)CBoxChooseMark.SelectedItem;
            
                db.AddMark(newMark);

                ErrorMsg.Foreground = new SolidColorBrush(Colors.Green);
                ErrorMsg.Content = "Mark successfully added";
            } else {
                ErrorMsg.Foreground = new SolidColorBrush(Colors.Red);
                ErrorMsg.Content = "Please fill all fields";
            }

        }

        //Side Btns
        private void BtnMarks_Click(object sender, RoutedEventArgs e)
        {
            MarksTeacher MarksWindow = new MarksTeacher(LoggedInUser);
            MarksWindow.Show();
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
