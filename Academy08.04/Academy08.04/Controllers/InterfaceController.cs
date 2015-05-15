using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy08._04.Models;
using System.Web.UI;

namespace Academy08._04.Controllers
{
    public class InterfaceController : Controller
    {
        private AcademyContext db = new AcademyContext();

        [Authorize]
        public ActionResult PersonalPage()
        {
            string userName = HttpContext.User.Identity.Name;
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            User user = users.FirstOrDefault(i => i.Login == userName);
            return View(user);
        }

        [Authorize]
        public ActionResult Schedule(DateTime? date_filter = null, int group_filter = -1)
        {
            if (!HttpContext.User.IsInRole("Student"))
            {
                //get groups
                List<Group> groups = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
                ViewBag.Groups = new SelectList(groups, "Id", "Name");

                //get subjects
                List<Subject> subjects = db.Subjects.ToList();
                ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

                //getting current group
                if (group_filter == -1)
                    group_filter = groups[0].Id;
            } else {
                //get current group for student
                string userName = HttpContext.User.Identity.Name;
                var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
                User CurrentUser = users.FirstOrDefault(i => i.Login == userName);
                group_filter = (int) CurrentUser.GroupId;
            }

            //date for this view
            if (!date_filter.HasValue)
                date_filter = (DateTime?)db.Schedule.Where(s => s.GroupId == group_filter).ToList().FirstOrDefault().Date;

            //get dates
            List<Schedule> scheduleDate = db.Schedule.Include(s => s.Subject).Where(g => g.GroupId == group_filter)
                                                     .GroupBy(d => d.Date).Select(day => day.FirstOrDefault()).ToList();
            ViewBag.Dates = new SelectList(scheduleDate, "Date", "Date");

           
            // get model
            List<Schedule> ScheduleList = db.Schedule.Include(u => u.Subject).Where(sch => sch.Date == date_filter).Where(gr => gr.GroupId == group_filter).ToList();
            if (ScheduleList == null)
                date_filter = (DateTime?)db.Schedule.Where(s => s.GroupId == group_filter).ToList().FirstOrDefault().Date;

            ScheduleList = db.Schedule.Include(u => u.Subject).Where(sch => sch.Date == date_filter).Where(gr => gr.GroupId == group_filter).ToList();
            
            ViewBag.CurrentGroup = group_filter;

            return View(ScheduleList);
        }

        public ActionResult addSchedules(List<Schedule> sl)
        {
            bool flag = false;
            int group = sl[0].GroupId;
            DateTime date = sl[0].Date;
            List<Schedule> modifiedEntities = new List<Schedule>(Academy08._04.Models.Schedule.MaxLessonsPerDay);

            List<Schedule> schedules = db.Schedule.GroupBy(d => d.Date).Select(e => e.FirstOrDefault()).ToList();
            
            foreach (var schedule in schedules)
            {
                if (schedule.Date == sl[0].Date) { 
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                modifiedEntities = db.Schedule.Where(gr => gr.GroupId == group).Where(d => d.Date == date).OrderBy(l => l.Lesson).ToList();
                for (int i = 0; i < sl.Capacity; i++)
                {
                    modifiedEntities[i].Classroom = sl[i].Classroom;
                    modifiedEntities[i].SubjectId = sl[i].SubjectId;
                    db.Entry(modifiedEntities[i]).State = EntityState.Modified;
                }
             } else {
                 foreach (var schedule in sl) {
                     schedule.Date = date;
                     db.Entry(schedule).State = EntityState.Added;
                 }
             }
            db.SaveChanges();
            return RedirectToAction("Schedule");
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult Marks()
        {
            //getting current user
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            string userName = HttpContext.User.Identity.Name;
            User user = users.FirstOrDefault(i => i.Login == userName);

            // geting model (subjects and marks)
            var subjects = db.Subjects.ToList();
            var marks = db.Marks.OrderBy(s => s.SubjectId).OrderBy(d => d.Date).ToList();
            ViewBag.Subjects = subjects;

            //getting header dates
            IEnumerable<Academy08._04.Models.Marks> dates = db.Marks.GroupBy(d => d.Date).Select(date => date.FirstOrDefault()).ToList();
            ViewBag.DataHeader = dates;
            ViewBag.DataFilter = new SelectList(dates, "Date", "Date");

            return View(marks);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult Marks(DateTime date_from, DateTime date_to)
        {
            //getting current user
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            string userName = HttpContext.User.Identity.Name;
            User user = users.FirstOrDefault(i => i.Login == userName);

            // getting subjects
            var subjects = db.Subjects.ToList();
            ViewBag.Subjects = subjects;

            //getting header dates
            List<Academy08._04.Models.Marks> dates = db.Marks.GroupBy(d => d.Date).Select(date => date.FirstOrDefault()).ToList();
            List<Academy08._04.Models.Marks> filteredDates = new List<Academy08._04.Models.Marks>();
            foreach (Marks date in dates)
            {
                if (date.Date.CompareTo(date_from.Date) >= 0 && date.Date.CompareTo(date_to.Date) <= 0)
                {
                    filteredDates.Add(date);
                }
            }
            ViewBag.DataHeader = filteredDates;
            ViewBag.DataFilter = new SelectList(dates, "Date", "Date");

            var marks = db.Marks.OrderBy(s => s.SubjectId).OrderBy(d => d.Date).ToList();
            var filteredMarks = new List<Marks>();
            foreach (Marks mark in marks)
            {
                if (mark.Date.CompareTo(date_from.Date) >= 0 && mark.Date.CompareTo(date_to.Date) <= 0)
                {
                    filteredMarks.Add(mark);
                }
            }
            return View(filteredMarks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult MarksTeacher()
        {
            //Filter
            List<Group> groups = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

            //View
            IEnumerable<Academy08._04.Models.User> students = db.Users.Where(r => r.RoleId == 3).Where(g => g.GroupId == 2).ToList();
            var marks = db.Marks.Where(s => s.SubjectId == 1).ToList();
            ViewBag.StudentsFilter = new SelectList(students, "Id", "LastName");
            ViewBag.Students = students;

            List<Academy08._04.Models.Marks> dates = db.Marks.Where(s => s.SubjectId == 1).GroupBy(d => d.Date).Select(date => date.FirstOrDefault()).ToList();
            List<Academy08._04.Models.Marks> filteredDates = new List<Academy08._04.Models.Marks>();
            foreach (var date in dates)
            {
                foreach (var student in students)
                {
                    if (date.StudentId == student.Id) { 
                        filteredDates.Add(date);
                        break;
                    }
                }
            }
            ViewBag.DataHeader = filteredDates;
            ViewBag.DataFilter = new SelectList(filteredDates, "Date", "Date");

            //For new marks
            ViewBag.Group = groups[1];
            ViewBag.Subject = subjects[1];

            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult MarksTeacher(int? group_filter, int? subject_filter, DateTime? date_from, DateTime? date_to)
        {
            if (group_filter == null)
                group_filter = 2;
            if (subject_filter == null)
                subject_filter = 1;
            if (date_from == null)
                date_from = DateTime.MinValue;
            if (date_to == null)
                date_to = DateTime.MaxValue;
            //Filter
            List<Group> groups = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

            //View
            IEnumerable<Academy08._04.Models.User> students = db.Users.Where(r => r.RoleId == 3).Where(g => g.GroupId == group_filter).ToList();
            var marks = db.Marks.Where(s => s.SubjectId == subject_filter).ToList();
            ViewBag.StudentsFilter = new SelectList(students, "Id", "LastName");
            ViewBag.Students = students;

            List<Academy08._04.Models.Marks> dates = db.Marks.Where(s => s.SubjectId == subject_filter).GroupBy(d => d.Date).Select(date => date.FirstOrDefault()).ToList();
            List<Academy08._04.Models.Marks> filteredDates = new List<Academy08._04.Models.Marks>();
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

            //For new marks
            ViewBag.Group = db.Groups.Find(group_filter);
            ViewBag.Subject = db.Subjects.Find(subject_filter);

            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult AddMark(List<int> GroupSubject)
        {
            int groupId;
            int subjectId;
            try {
                groupId = GroupSubject[0];
            } catch (NullReferenceException) {
                groupId = 2;
            }
            try {
                subjectId = GroupSubject[1];
            } catch (NullReferenceException) {
                subjectId = 1;
            }

            IEnumerable<Academy08._04.Models.User> students = db.Users.Where(r => r.RoleId == 3).Where(g => g.GroupId == groupId).ToList();

            ViewBag.Students = new SelectList(students, "Id", "LastName");
            ViewBag.Marks = new SelectList(new List<int> { 1, 2, 3, 4, 5 }, "Mark");
            ViewBag.Subject = db.Subjects.Find(subjectId);

            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddMark(Marks mark)
        {
            if (ModelState.IsValid)
            {
                db.Marks.Add(mark);
                db.SaveChanges();
            }
            return RedirectToAction("MarksTeacher");
        }

    }
}
