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

namespace Academy.WPF.View.Interface
{
    public partial class Marks : Window
    {
        Entities db;
        User LoggedInUser;

        public Marks(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void RefrashTable() {
            TableView table = new TableView();
            List<Mark> headersMarks = db.GetMarksWithDistinctDateForStudent(LoggedInUser.Id);
            List<DateTime> headersDates = new List<DateTime>();
            foreach(var mark in headersMarks)
                headersDates.Add(mark.Date);
            table.TableHeaders = headersDates;

            DataGridTextColumn textColumn = new DataGridTextColumn(); 
            textColumn.Header = "Subjects"; 
            textColumn.Binding = new Binding("Subject"); 
            TableMarks.Columns.Add(textColumn); 

            List<Subject> allSubjects = db.GetAllSubjects();
        }

        private void AddControls(int controlNumber)
        {
            TextBox newTextbox = new TextBox();

            // textbox needs a unique id to maintain state information
            newTextbox.Name = "TextBox_" + controlNumber;


            // add the label and textbox to the panel, then add the panel to the form
            newPanel.Controls.Add(newTextbox);
            form1.Controls.Add(newPanel);
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
    }

    private class TableView
    {
        public List<DateTime> TableHeaders { get; set; }
        public string Subject { get; set; }
        public List<Mark> marks { get; set; }
    }
}
