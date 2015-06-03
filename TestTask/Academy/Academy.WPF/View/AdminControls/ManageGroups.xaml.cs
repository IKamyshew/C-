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
    public partial class ManageGroups : Window
    {
        Entities db;
        User LoggedInUser;

        public ManageGroups(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();
            WindowManageGroups.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            RefreshTable();
            RefreshDeleteBox();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (BoxNewGroup.Text != "")
            {
                Group newGroup = new Group();
                newGroup.Name = BoxNewGroup.Text;
                db.AddGroup(newGroup);

                RefreshTable();
                RefreshDeleteBox();

                BoxNewGroup.Text = "";
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxDeleteGroup.SelectedItem != null)
            {
                string selectedName = (string)CBoxDeleteGroup.SelectedItem;
                Group SelectedGroup = db.GetGroupByName(selectedName);
                db.DeleteGroup(SelectedGroup.Id);

                RefreshTable();
                RefreshDeleteBox();
            }
        }

        private void RefreshTable()
        {
            TableGroups.ItemsSource = db.GetAllGroups();
            TableGroups.Items.Refresh();
        }

        private void RefreshDeleteBox()
        {
            var groups = db.GetAllGroups();
            var groupsNames = new List<string>();

            foreach (var group in groups)
                groupsNames.Add(group.Name);

            CBoxDeleteGroup.ItemsSource = groupsNames;
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
