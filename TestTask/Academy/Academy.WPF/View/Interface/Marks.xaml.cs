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
    public partial class Marks : Window
    {
        private Entities db;
        private User LoggedInUser;
        private List<DateTime> TableHeaders { get; set; }
        List<Mark> headersMarks;

        public Marks(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            headersMarks = db.GetMarksWithDistinctDateForStudent(LoggedInUser.Id);

            List<DateTime> headersDatesOnLoad = new List<DateTime>();

            foreach (var mark in headersMarks)
                headersDatesOnLoad.Add(mark.Date);

            CBoxDateFrom.ItemsSource = headersDatesOnLoad;
            CBoxDateTo.ItemsSource = headersDatesOnLoad;

            RefrashTable();
        }

        private void BtnFilterMarks_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxDateFrom.SelectedItem == null || CBoxDateTo.SelectedItem == null)
                return;

            headersMarks = db.GetMarksWithDistinctDatesForStudentInPeriod(LoggedInUser.Id, (DateTime)CBoxDateFrom.SelectedItem, (DateTime)CBoxDateTo.SelectedItem);

            RefrashTable();
        }
            

        private void RefrashTable() {
            TableMarks.ItemsSource = null;
            TableMarks.Columns.Clear();
            TableMarks.Items.Clear();
            TableMarks.Items.Refresh();

            List<DateTime> headersDates = new List<DateTime>();

            foreach(var mark in headersMarks)
                headersDates.Add(mark.Date);

            TableHeaders = headersDates;

            DataGridTextColumn textColumn = new DataGridTextColumn(); 
            textColumn.Header = "Subjects"; 
            textColumn.Binding = new Binding("Subject"); 
            TableMarks.Columns.Add(textColumn);

            int index = 0;
            foreach (var header in TableHeaders)
            {
                TableMarks.Columns.Add(new DataGridTextColumn
                {
                    Header = header.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Binding = new Binding(string.Format("Marks[{0}].MarkValue", index++))
                });
            }

            List<Subject> allSubjects = db.GetAllSubjects();
            List<TableView> rows = new List<TableView>();

            double totalAVG = 0;
            int countAVG = 0;
            foreach (var subj in allSubjects)
            {
                TableView row = new TableView();
                row.Subject = subj.Name;
                row.Marks = db.GetMarksForSubjectAndStudent(subj.Id, LoggedInUser.Id);
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

        //Side Btns
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

        class TableView
        {
            public string Subject { get; set; }
            public List<Mark> Marks { get; set; }
            public double Average { get; set; }
        }
    }
}
