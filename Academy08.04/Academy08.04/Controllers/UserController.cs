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
    // 
    
    public class UserController : Controller
    {
        private AcademyContext db = new AcademyContext();

        [HttpGet]
        [Authorize(Roles = "Manager, Teacher")]
        public ActionResult Index()
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

        [HttpPost]
        [Authorize(Roles = "Manager, Teacher")]
        public ActionResult Index(int group_filter, int role_filter)
        {
            IEnumerable<User> allUsers = null;
            if (role_filter == 0 && group_filter == 0)
            {
                return RedirectToAction("Index");
            }
            if (role_filter == 0 && group_filter != 0)
            {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.GroupId == group_filter
                           select user;
            }
            else if (role_filter != 0 && group_filter == 0)
            {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.RoleId == role_filter
                           select user;
            }
            else
            {
                allUsers = from user in db.Users.Include(u => u.Group).Include(u => u.Role)
                           where user.GroupId == group_filter && user.RoleId == role_filter
                           select user;
            }

            //Добавляем в список возможность выбора всех
            List<Group> groups = db.Groups.ToList();
            groups.Insert(0, new Group { Name = "All", Id = 0 });
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(allUsers.ToList());
        }

        [HttpGet]
        [Authorize(Roles="Manager")]
        public ActionResult Create()
        {
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Manager")]
        public ActionResult Create(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles="Manager")]
        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            SelectList groups = new SelectList(db.Groups, "Id", "Name", user.GroupId);
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles="Manager")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }

        [Authorize(Roles="Manager")]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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

            var shedule = db.Schedule.ToList();

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(shedule);
        }
    }
}
