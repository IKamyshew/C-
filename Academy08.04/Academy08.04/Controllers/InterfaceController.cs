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

            List<Schedule> shedule = db.Schedule.Include(u => u.Subject).ToList();
            

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(shedule);
        }
    }
}
