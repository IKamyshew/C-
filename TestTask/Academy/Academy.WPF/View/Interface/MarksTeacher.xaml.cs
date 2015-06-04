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
    public partial class MarksTeacher : Window
    {
        private Entities db;
        private User LoggedInUser;
        private List<DateTime> TableHeaders { get; set; }
        private Group GroupFilter = null;
        private Subject SubjectFilter = null;
        private List<Mark> headersMarks;

        public MarksTeacher(User user)
        {
            LoggedInUser = user;
            db = new Entities();
            headersMarks = new List<Mark>();

            InitializeComponent();
            WindowMarksTeacher.WindowStartupLocation = WindowStartupLocation.CenterScreen;

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
        }

        private void GroupAndSubject_Choosed(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxChooseGroup.SelectedItem == null || CBoxChooseSubject.SelectedItem == null)
                return;

            GroupFilter = db.GetGroupByName((string)CBoxChooseGroup.SelectedItem);
            SubjectFilter = db.GetSubjectByName((string)CBoxChooseSubject.SelectedItem);

            List<User> students = db.GetAllStudentsForGroup(GroupFilter.Id);
            List<Mark> Dates = db.GetMarksWithDistinctDateForSubject(SubjectFilter.Id);

            headersMarks.Clear();
            foreach (var date in Dates)
            {
                foreach (var student in students)
                {
                    if (date.StudentId == student.Id)
                    {
                        headersMarks.Add(date);
                        break;
                    }
                }
            }

            List<DateTime> headersDatesOnLoad = new List<DateTime>();

            foreach (var mark in headersMarks)
                headersDatesOnLoad.Add(mark.Date);

            CBoxDateFrom.ItemsSource = headersDatesOnLoad;
            CBoxDateTo.ItemsSource = headersDatesOnLoad;

            RefrashTable();
        }

        private void BtnFilterMarks_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxDateFrom.SelectedItem == null || CBoxDateTo.SelectedItem == null || CBoxChooseGroup.SelectedItem == null || CBoxChooseSubject.SelectedItem == null)
                return;

            GroupFilter = db.GetGroupByName((string)CBoxChooseGroup.SelectedItem);
            List<User> students = db.GetAllStudentsForGroup(GroupFilter.Id);
            List<Mark> Dates = db.GetMarksWithDistinctDateForSubject(SubjectFilter.Id);

            headersMarks.Clear();
            foreach (var date in Dates)
            {
                foreach (var student in students)
                {
                    if (date.Date.CompareTo((DateTime)CBoxDateFrom.SelectedItem) >= 0 && date.Date.CompareTo((DateTime)CBoxDateTo.SelectedItem) <= 0)
                    {
                        if (date.StudentId == student.Id)
                        {
                            headersMarks.Add(date);
                            break;
                        }
                    }
                }
            }

            RefrashTable();
        }


        private void RefrashTable()
        {
            TableMarks.ItemsSource = null;
            TableMarks.Columns.Clear();
            TableMarks.Items.Clear();
            TableMarks.Items.Refresh();

            List<DateTime> headersDates = new List<DateTime>();

            foreach (var mark in headersMarks)
                headersDates.Add(mark.Date);

            TableHeaders = headersDates;

            DataGridTextColumn textColumn0 = new DataGridTextColumn();
            textColumn0.Header = "First Name";
            textColumn0.Binding = new Binding("FirstName");
            TableMarks.Columns.Add(textColumn0);

            DataGridTextColumn textColumn1 = new DataGridTextColumn();
            textColumn1.Header = "Last Name";
            textColumn1.Binding = new Binding("LastName");
            TableMarks.Columns.Add(textColumn1);

            int index = 0;
            foreach (var header in TableHeaders)
            {
                TableMarks.Columns.Add(new DataGridTextColumn
                {
                    Header = header.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Binding = new Binding(string.Format("Marks[{0}].MarkValue", index++))
                });
            }

            List<User> allStudents = db.GetUsersFilteredByGroup(GroupFilter.Id);
            List<TableView> rows = new List<TableView>();

            double totalAVG = 0;
            int countAVG = 0;
            foreach (var student in allStudents)
            {
                TableView row = new TableView();
                row.FirstName = student.FirstName;
                row.LastName = student.LastName;
                row.Marks = db.GetMarksForSubjectAndStudent(SubjectFilter.Id, student.Id);
                if (row.Marks.Count != 0)
                {
                    double sum = 0;
                    foreach (var mark in row.Marks)
                        sum += mark.MarkValue;
                    row.Average = Math.Round(sum / row.Marks.Count(), 2);
                    totalAVG += row.Average;
                    countAVG++;
                }
                rows.Add(row);
            }

            TableMarks.ItemsSource = rows;

            DataGridTextColumn textColumnAverage = new DataGridTextColumn();
            textColumnAverage.Header = "Average";
            textColumnAverage.Binding = new Binding("Average");
            TableMarks.Columns.Add(textColumnAverage);

            double resultAverage = 0;

            if (countAVG != 0)
                resultAverage = totalAVG / countAVG;

            BoxTotalAverage.Text = String.Format("{0:F2}", resultAverage);
        }

        class TableView
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public List<Mark> Marks { get; set; }
            public double Average { get; set; }
        }

        //Side Btns
        private void BtnAddMark_Click(object sender, RoutedEventArgs e)
        {
            AddMark AddMarkWindow = new AddMark(LoggedInUser);
            AddMarkWindow.Show();
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
