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
    public partial class Schedule : Window
    {
        private User LoggedInUser;
        private Entities db;
        private Group GroupFilter = null;
        private DateTime? DateFilter = null;

        public Schedule(User user)
        {
            LoggedInUser = user;
            db = new Entities();

            InitializeComponent();

            List<Group> allGroups = db.GetAllStudentGroups();
            List<string> groupNames = new List<string>();
            foreach(var group in allGroups)
                groupNames.Add(group.Name);
            CBoxChooseGroup.ItemsSource = groupNames;

            if (LoggedInUser.RoleId != 1)
            {
                TableNewSchedule.Visibility = Visibility.Collapsed;
                BoxChooseNewDate.Visibility = Visibility.Collapsed;
                LabelAddEdit.Visibility = Visibility.Collapsed;
                BtnAdd.Visibility = Visibility.Collapsed;
            }
        }

        public Schedule()
        {
            // TODO: Complete member initialization
        }

        private void Group_Choosed(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxChooseGroup.SelectedItem != null) {
                GroupFilter = db.GetGroupByName((string)CBoxChooseGroup.SelectedItem);

                List<Academy.Model.Entities.Schedule> schedulesForGroup = db.GetSchedulesForGroupDistinctByDate(GroupFilter.Id);
                List<DateTime> datesForGroup = new List<DateTime>();
                foreach(var date in schedulesForGroup)
                    datesForGroup.Add(date.Date);
                CBoxChooseDate.ItemsSource = datesForGroup;
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxChooseDate.SelectedItem != null)
                DateFilter = (DateTime)CBoxChooseDate.SelectedItem;

            if (DateFilter != null && GroupFilter != null)
                TableSchedule.ItemsSource = db.GetSchedulesForGroupAndDate(GroupFilter.Id, DateFilter.Value);

            if (LoggedInUser.RoleId == 1 && TableSchedule.ItemsSource != null)
                LoadNewSchedule();
        }

        private void BtnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            List<Academy.Model.Entities.Schedule> newSchedule = (List<Academy.Model.Entities.Schedule>)TableNewSchedule.ItemsSource;
            List<Academy.Model.Entities.Schedule> modifiedEntities = new List<Academy.Model.Entities.Schedule>(Academy.Model.Entities.Schedule.MaxLessonsPerDay);

            DateTime myDate = DateTime.ParseExact((string)BoxChooseNewDate.Text, "MM/dd/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            if (db.IsSchedulesContainsDateForGroup(myDate, GroupFilter.Id))
            {
                modifiedEntities = db.GetSchedulesForGroupAndDate(GroupFilter.Id, myDate);
                for (int i = 0; i < newSchedule.Capacity; i++)
                {
                    modifiedEntities[i].Classroom = newSchedule[i].Classroom;
                    modifiedEntities[i].SubjectId = newSchedule[i].SubjectId;

                    db.UpdateSchedule(modifiedEntities[i]);
                }
            } else {
                foreach (var schedule in newSchedule)
                {
                    Academy.Model.Entities.Schedule scheduleNew = new Academy.Model.Entities.Schedule();
                    scheduleNew = schedule;
                    scheduleNew.Date = myDate;
                    db.AddSchedule(scheduleNew);
                }
            }
        }


        private void Edit_Cell(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void LoadNewSchedule()
        {
            TableNewSchedule.ItemsSource = TableSchedule.ItemsSource;
        }

        //side buttons
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
