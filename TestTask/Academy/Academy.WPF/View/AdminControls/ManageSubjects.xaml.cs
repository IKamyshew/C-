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

namespace Academy.WPF.View
{
    public partial class ManageSubjects : Window
    {
        Entities db;
        User LoggedInUser;

        public ManageSubjects(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowManageSubjects.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            RefreshTable();
            RefreshDeleteBox();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (BoxNewSubject.Text != "")
            {
                Subject newSubject = new Subject();
                newSubject.Name = BoxNewSubject.Text;
                db.AddSubject(newSubject);

                RefreshTable();
                RefreshDeleteBox();

                BoxNewSubject.Text = "";
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxDeleteSubject.SelectedItem != null)
            {
                string selectedName = (string)CBoxDeleteSubject.SelectedItem;
                Subject SelectedSubject = db.GetSubjectByName(selectedName);
                db.DeleteSubject(SelectedSubject.Id);

                RefreshTable();
                RefreshDeleteBox();
            }
        }

        private void RefreshTable()
        {
            TableSubjects.ItemsSource = db.GetAllSubjects();
            TableSubjects.Items.Refresh();
        }

        private void RefreshDeleteBox()
        {
            var allSubjects = db.GetAllSubjects();
            var subjectsNames = new List<string>();

            foreach (var subject in allSubjects)
                subjectsNames.Add(subject.Name);

            CBoxDeleteSubject.ItemsSource = subjectsNames;
        }

        //Side Btns
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
