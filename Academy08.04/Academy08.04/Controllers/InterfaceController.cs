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

        public ActionResult Index()
        {
            return View();
        }

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
            IEnumerable<Schedule> allSchedules = null;
            if (date_filter == null)
            {
                RedirectToAction("Schedule");
            }
            else
            {
                allSchedules = from schedulesDB in db.Schedule.Include(u => u.Subject)
                               where schedulesDB.Date == date_filter
                               select schedulesDB;
            }

            List<Schedule> schedules = db.Schedule.ToList();
            ViewBag.Dates = new SelectList(schedules, "Date", "Date");

            return View(allSchedules.OrderBy(u => u.Lesson).ToList());
        }
    }
}
