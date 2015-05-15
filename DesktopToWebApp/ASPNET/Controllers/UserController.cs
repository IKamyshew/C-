using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DesktopToWebApp.Model.Models;
using System.Web.UI;

namespace ASPNET.Controllers
{
    public class UserController : Controller
    {
        private AcademyContext db = new AcademyContext();

        [HttpGet]
        public ActionResult UsersList()
        {
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();

            List<Group> groups = db.Groups.ToList();
            groups.Insert(0, new Group { Name = "All", Id = 0 });
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(users);
        }
    }
}
