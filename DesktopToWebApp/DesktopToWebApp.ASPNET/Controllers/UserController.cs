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

        public ActionResult UsersList(int group_filter = 0, int role_filter = 0)
        {
            IEnumerable<User> allUsers = null;
            if (role_filter == 0 && group_filter == 0)
            {
                allUsers = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            } else if (role_filter == 0 && group_filter != 0)
            {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.GroupId == group_filter
                           select user;
            } else if (role_filter != 0 && group_filter == 0)
            {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.RoleId == role_filter
                           select user;
            } else {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.GroupId == group_filter && user.RoleId == role_filter
                           select user;
            }

            List<Group> groups = db.Groups.ToList();
            groups.Insert(0, new Group { Name = "All", Id = 0 });
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(allUsers.ToList());
        }
    }
}
