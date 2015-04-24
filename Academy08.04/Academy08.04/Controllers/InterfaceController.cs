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
        [HttpGet]
        public ActionResult PersonalPage()
        {
            string userName = HttpContext.User.Identity.Name;
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            User user = users.FirstOrDefault(i => i.Login == userName);
            return View(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Schedule()
        {
            string userName = HttpContext.User.Identity.Name;
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            User user = users.FirstOrDefault(i => i.Login == userName);

            List<Schedule> schedule = db.Schedule.Include(s => s.Subject).Where(g => g.GroupId == user.GroupId).ToList();
            List<Schedule> scheduleDate = schedule.GroupBy(d => d.Date).Select(day => day.FirstOrDefault()).ToList();
            ViewBag.Dates = new SelectList(scheduleDate, "Date", "Date");

            return View(schedule);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Schedule(DateTime date_filter)
        {
            string userName = HttpContext.User.Identity.Name;
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            User user = users.FirstOrDefault(i => i.Login == userName);

            List<Schedule> schedule = db.Schedule.Include(s => s.Subject).Where(g => g.GroupId == user.GroupId).ToList();
            List<Schedule> scheduleDate = schedule.GroupBy(d => d.Date).Select(day => day.FirstOrDefault()).ToList();
            ViewBag.Dates = new SelectList(scheduleDate, "Date", "Date");

            IEnumerable<Schedule> allSchedules = null;
            if (date_filter == null)
            {
                RedirectToAction("Schedule");
            }
            else
            {
                allSchedules = from schedulesDB in db.Schedule.Include(u => u.Subject)
                               where schedulesDB.Date == date_filter 
                               where schedulesDB.GroupId == user.GroupId
                               select schedulesDB;
            }

            return View(allSchedules.OrderBy(u => u.Lesson).ToList());
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

            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult MarksTeacher()
        {
            //Filter
            List<Group> groups = db.Groups.ToList();
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

            //For new marks
            ViewBag.Group = groups[2];
            ViewBag.Subject = subjects[1];

            return View(marks);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult MarksTeacher(int group_filter, int subject_filter)
        {
            //Filter
            List<Group> groups = db.Groups.ToList();
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
                foreach (var student in students)
                {
                    if (date.StudentId == student.Id)
                    {
                        filteredDates.Add(date);
                        break;
                    }
                }
            }
            ViewBag.DataHeader = filteredDates;

            //For new marks
            ViewBag.Group = groups[group_filter];
            ViewBag.Subject = subjects[subject_filter];

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
            ViewBag.Marks = new SelectList(new List<int> {1, 2, 3, 4, 5}, "Id");
            ViewBag.Subject = db.Subjects.Find(subjectId);

            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult AddMark(Marks mark)
        {
            if (mark != null)
            {
                db.Marks.Add(mark);
                db.SaveChanges();
            }
            return RedirectToAction("MarksTeacher");
        }

    }
}
