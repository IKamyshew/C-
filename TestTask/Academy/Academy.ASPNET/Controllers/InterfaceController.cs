using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy.Model.DBAccess;
using Academy.Model.Entities;
using System.Web.Security;

namespace Academy.ASPNET.Controllers
{
    [Authorize]
    public class InterfaceController : Controller
    {
        private Entities db = new Entities();
        private CurrentUser CurrentUser = new CurrentUser();

        public ActionResult ProfilePage()
        {
            string UserName = System.Web.HttpContext.Current.User.Identity.Name;
            CurrentUser CurrentUser = new CurrentUser();
            User user = CurrentUser.GetCurrentUserByLogin(UserName);

            return View(user);
        }

        public ActionResult Schedule(DateTime? date_filter = null, int group_filter = -1)
        {
            if (HttpContext.User.IsInRole("Student"))
            {
                //get current group for student
                string userName = HttpContext.User.Identity.Name;
                User user = CurrentUser.GetCurrentUserByLogin(userName);
                group_filter = user.GroupId;
            } else {
                //get groups
                List<Group> groups = db.GetAllStudentGroups();
                ViewBag.Groups = new SelectList(groups, "Id", "Name");

                //get subjects
                List<Subject> subjects = db.GetAllSubjects();
                ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

                //getting current group
                if (group_filter == -1)
                    group_filter = groups[0].Id;
            }

            //get dates
            List<Schedule> scheduleDate = db.GetSchedulesForGroupDistinctByDate(group_filter);
            ViewBag.Dates = new SelectList(scheduleDate, "Date", "Date");

            // get model
            List<Schedule> ScheduleList = new List<Schedule>(8);
            if (date_filter.HasValue)
                ScheduleList = db.GetSchedulesForGroupAndDate(group_filter, (DateTime)date_filter);
                
            ViewBag.CurrentGroup = group_filter;

            return View(ScheduleList);
        }

        public ActionResult addSchedules(List<Schedule> sl)
        {
            int group = sl[0].GroupId;
            DateTime date = sl[0].Date;

            List<Schedule> modifiedEntities = new List<Schedule>(Academy.Model.Entities.Schedule.MaxLessonsPerDay);

            bool DateExist = db.IsSchedulesContainsDateForGroup(date, group);

            if (DateExist)
            {
                modifiedEntities = db.GetSchedulesForGroupAndDate(group, date);
                for (int i = 0; i < sl.Capacity; i++)
                {
                    modifiedEntities[i].Classroom = sl[i].Classroom;
                    modifiedEntities[i].SubjectId = sl[i].SubjectId;

                    db.UpdateSchedule(modifiedEntities[i]);
                }
            } else {
                foreach (var schedule in sl)
                {
                    schedule.Date = date;
                    db.AddSchedule(schedule);
                }
            }
            return RedirectToAction("Schedule");
        }

        [Authorize(Roles = "Student")]
        public ActionResult Marks(DateTime? date_from = null, DateTime? date_to = null)
        {
            //getting current user
            string userName = HttpContext.User.Identity.Name;
            User user = CurrentUser.GetCurrentUserByLogin(userName);

            // geting subjects
            var subjects = db.GetAllSubjects(); ;
            ViewBag.Subjects = subjects;

            //getting dates for filter
            List<Mark> dates = db.GetMarksWithDistinctDate();
            ViewBag.DataFilter = new SelectList(dates, "Date", "Date");

            //getting marks
            List<Mark> marks = new List<Mark>();

            if (date_from != null && date_to != null) {
                marks = db.GetMarksBetweenDatesForStudent(user.Id, (DateTime)date_from, (DateTime)date_to);
                ViewBag.DataHeader = db.GetMarksWithDistinctDatesForStudentInPeriod(user.Id, (DateTime)date_from, (DateTime)date_to);
            }
            else
            {
                marks = db.GetMarksOrderedByDateForStudent(user.Id);
                ViewBag.DataHeader = db.GetMarksWithDistinctDateForStudent(user.Id);
            }
            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult MarksTeacher(int? group_filter, int? subject_filter, DateTime? date_from, DateTime? date_to)
        {
            //Filter
            List<Group> groups = db.GetAllStudentGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Subject> subjects = db.GetAllSubjects();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

            if (!group_filter.HasValue)
                try {
                    group_filter = (int)TempData["CurrentGroup"];
                } catch (NullReferenceException) {
                    group_filter = groups[0].Id;
                }
            if (!subject_filter.HasValue)
                try {
                    subject_filter = (int) TempData["CurrentSubject"];
                } catch (NullReferenceException) { 
                    subject_filter = subjects[0].Id;
                }
            if (date_from == null)
                date_from = DateTime.MinValue;
            if (date_to == null)
                date_to = DateTime.MaxValue;

            //View
            List<User> students = db.GetAllStudentsForGroup((int)group_filter);
            ViewBag.StudentsFilter = new SelectList(students, "Id", "LastName");
            ViewBag.Students = students;

            List<Mark> dates = db.GetMarksWithDistinctDateForSubject((int)subject_filter);
            List<Mark> filteredDates = new List<Mark>();
            foreach (var date in dates)
            {
                if (date.Date.CompareTo(date_from) >= 0 && date.Date.CompareTo(date_to) <= 0)
                {
                    foreach (var student in students)
                    {
                        if (date.StudentId == student.Id)
                        {
                            filteredDates.Add(date);
                            break;
                        }
                    }
                }
            }
            ViewBag.DataHeader = filteredDates;
            ViewBag.DataFilter = new SelectList(dates, "Date", "Date");

            var marks = db.GetMarksForSubject((int)subject_filter);
            //For new marks
            ViewBag.Group = db.GetGroupByID((int)group_filter);
            ViewBag.Subject = db.GetSubjectByID((int)subject_filter);

            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult AddMark(List<int> GroupSubject)
        {
            int groupId;
            int subjectId;
            try {
                groupId = (int)TempData["CurrentGroup"];
            } catch (NullReferenceException) {
                groupId = 3;
            } 
            
            try {
                subjectId = (int)TempData["CurrentSubject"];
            }
            catch (NullReferenceException)
            {
                subjectId = 1;
            }

            List<User> students = db.GetUsersFilteredByGroup(groupId);

            ViewBag.Students = new SelectList(students, "Id", "LastName");
            ViewBag.Marks = new SelectList(new List<int> { 1, 2, 3, 4, 5 }, "Mark");
            ViewBag.Subject = db.GetSubjectByID(subjectId);

            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddMark(Mark mark)
        {
            if (ModelState.IsValid)
            {
                if(db.AddMark(mark))
                    TempData["message"] = "Mark successfully added";
                else
                    TempData["message"] = "Mark failed to add";
            }
            return RedirectToAction("MarksTeacher");
        }
    }
}
