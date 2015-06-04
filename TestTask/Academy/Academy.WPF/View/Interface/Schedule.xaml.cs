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
            WindowSchedule.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            List<Group> allGroups = db.GetAllStudentGroups();
            List<string> groupNames = new List<string>();
            foreach(var group in allGroups)
                groupNames.Add(group.Name);
            CBoxChooseGroup.ItemsSource = groupNames;

            if (LoggedInUser.RoleId == 3)
            {
                CBoxChooseGroup.SelectedItem = LoggedInUser.Group.Name;
                CBoxChooseGroup.Focusable = false;
                CBoxChooseGroup.IsHitTestVisible = false;
            }

            if (LoggedInUser.RoleId != 1)
            {
                TableNewSchedule.Visibility = Visibility.Collapsed;
                BoxChooseNewDate.Visibility = Visibility.Collapsed;
                LabelAddEdit.Visibility = Visibility.Collapsed;
                BtnAdd.Visibility = Visibility.Collapsed;

            } else { 
                List<Subject> allSubjects = db.GetAllSubjects();
                List<string> allSubjectNames = new List<string>();

                foreach (var subj in allSubjects)
                    allSubjectNames.Add(subj.Name);

                CBoxScheduleSubj1.ItemsSource = allSubjectNames;
                CBoxScheduleSubj2.ItemsSource = allSubjectNames;
                CBoxScheduleSubj3.ItemsSource = allSubjectNames;
                CBoxScheduleSubj4.ItemsSource = allSubjectNames;
                CBoxScheduleSubj5.ItemsSource = allSubjectNames;
                CBoxScheduleSubj6.ItemsSource = allSubjectNames;
                CBoxScheduleSubj7.ItemsSource = allSubjectNames;
                CBoxScheduleSubj8.ItemsSource = allSubjectNames;
            }
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
        }

        private void BtnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact((string)BoxChooseNewDate.Text, "MM/dd/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                {
                    ErrorMsg.Content = "Please fill date field in MM/dd/yyyy format.";
                    return;
                }
                throw;
            }

            List<Academy.Model.Entities.Schedule> newSchedule = new List<Academy.Model.Entities.Schedule>(Academy.Model.Entities.Schedule.MaxLessonsPerDay);
            if (GroupFilter == null) { 
                ErrorMsg.Content = "Please choose group.";
                return;
            }
            if (db.IsSchedulesContainsDateForGroup(myDate, GroupFilter.Id))
            {
                newSchedule = (List<Academy.Model.Entities.Schedule>)TableSchedule.ItemsSource;

                try { 
                    newSchedule[0].Classroom = Int32.Parse(TBoxScheduleClass1.Text);
                    newSchedule[1].Classroom = Int32.Parse(TBoxScheduleClass2.Text);
                    newSchedule[2].Classroom = Int32.Parse(TBoxScheduleClass3.Text);
                    newSchedule[3].Classroom = Int32.Parse(TBoxScheduleClass4.Text);
                    newSchedule[4].Classroom = Int32.Parse(TBoxScheduleClass5.Text);
                    newSchedule[5].Classroom = Int32.Parse(TBoxScheduleClass6.Text);
                    newSchedule[6].Classroom = Int32.Parse(TBoxScheduleClass7.Text);
                    newSchedule[7].Classroom = Int32.Parse(TBoxScheduleClass8.Text);
                } catch (Exception ex) {
                    if (ex is NullReferenceException || ex is FormatException)
                    {
                        ErrorMsg.Content = "Please fill all classrooms with numbers.";
                        return;
                    }
                    throw;
                }

                try {
                    newSchedule[0].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj1.SelectedItem)).Id;
                    newSchedule[1].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj2.SelectedItem)).Id;
                    newSchedule[2].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj3.SelectedItem)).Id;
                    newSchedule[3].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj4.SelectedItem)).Id;
                    newSchedule[4].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj5.SelectedItem)).Id;
                    newSchedule[5].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj6.SelectedItem)).Id;
                    newSchedule[6].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj7.SelectedItem)).Id;
                    newSchedule[7].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj8.SelectedItem)).Id;
                } catch (NullReferenceException) {
                    ErrorMsg.Content = "Please choose subjects to all lessons.";
                    return;
                }


                foreach (var schedule in newSchedule)
                    db.UpdateSchedule(schedule);

                if (DateFilter != null && GroupFilter != null)
                TableSchedule.ItemsSource = db.GetSchedulesForGroupAndDate(GroupFilter.Id, DateFilter.Value);
            } else {
                for (int i = 0; i < newSchedule.Capacity; i++)
                {
                    try {
                        Academy.Model.Entities.Schedule sche = new Academy.Model.Entities.Schedule();
                        sche.Date = myDate;
                        sche.Lesson = i + 1;
                        sche.GroupId = GroupFilter.Id;
                        newSchedule.Add(sche);
                    } catch (NullReferenceException) {
                        if (myDate == null)
                            ErrorMsg.Content = "Please set correct date.";
                        return;
                    }
                }

                try { 
                    newSchedule[0].Classroom = Int32.Parse(TBoxScheduleClass1.Text);
                    newSchedule[1].Classroom = Int32.Parse(TBoxScheduleClass2.Text);
                    newSchedule[2].Classroom = Int32.Parse(TBoxScheduleClass3.Text);
                    newSchedule[3].Classroom = Int32.Parse(TBoxScheduleClass4.Text);
                    newSchedule[4].Classroom = Int32.Parse(TBoxScheduleClass5.Text);
                    newSchedule[5].Classroom = Int32.Parse(TBoxScheduleClass6.Text);
                    newSchedule[6].Classroom = Int32.Parse(TBoxScheduleClass7.Text);
                    newSchedule[7].Classroom = Int32.Parse(TBoxScheduleClass8.Text);
                } catch (Exception ex) {
                    if (ex is NullReferenceException || ex is FormatException)
                    {
                        ErrorMsg.Content = "Please fill all classrooms with numbers.";
                        return;
                    }
                    throw;
                }

                try {
                    newSchedule[0].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj1.SelectedItem)).Id;
                    newSchedule[1].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj2.SelectedItem)).Id;
                    newSchedule[2].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj3.SelectedItem)).Id;
                    newSchedule[3].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj4.SelectedItem)).Id;
                    newSchedule[4].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj5.SelectedItem)).Id;
                    newSchedule[5].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj6.SelectedItem)).Id;
                    newSchedule[6].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj7.SelectedItem)).Id;
                    newSchedule[7].SubjectId = (db.GetSubjectByName((string)CBoxScheduleSubj8.SelectedItem)).Id;
                }
                catch (NullReferenceException)
                {
                    ErrorMsg.Content = "Please choose subjects to all lessons.";
                    return;
                }

                foreach (var schedule in newSchedule)
                    db.AddSchedule(schedule);
            }
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
