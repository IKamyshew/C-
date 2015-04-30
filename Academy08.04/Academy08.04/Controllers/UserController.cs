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
    [Authorize(Roles = "Manager, Teacher")]
    public class UserController : Controller
    {
        private AcademyContext db = new AcademyContext();

        [HttpGet]
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
            if (ModelState.IsValid) 
            { 
                db.Users.Add(user);
                db.SaveChanges();
            }

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

        [HttpGet]
        public ActionResult Groups()
        {
            IEnumerable<Academy08._04.Models.Group> groups = db.Groups.ToList();
            ViewBag.Groups = groups;
            return View();
        }

        [HttpPost]
        public ActionResult Groups(Group group)
        {
            bool flag = true;
            foreach (var existedGroups in db.Groups)
            {
                if (existedGroups.Name.Equals(group.Name)) 
                {
                    flag = false;
                }
            }

            if (flag) { 
                db.Groups.Add(group);
                db.SaveChanges();
            }

            IEnumerable<Academy08._04.Models.Group> groups = db.Groups.ToList();
            ViewBag.Groups = groups;
            return View();
        }

        public ActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group != null) { 
                db.Groups.Remove(group);
                db.SaveChanges();
            }
            return RedirectToAction("Groups");
        }

        [HttpGet]
        public ActionResult Subjects()
        {
            IEnumerable<Academy08._04.Models.Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        public ActionResult Subjects(Subject subject)
        {
            bool flag = true;
            foreach (var existedGroups in db.Subjects)
            {
                if (existedGroups.Name.Equals(subject.Name))
                {
                    flag = false;
                }
            }

            if (flag)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
            }

            IEnumerable<Academy08._04.Models.Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = subjects;
            return View();
        }

        public ActionResult DeleteSubject(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject != null)
            {
                db.Subjects.Remove(subject);
                db.SaveChanges();
            }
            return RedirectToAction("Subjects");
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult GroupManager()
        {
            IEnumerable<Group> groups = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
            ViewBag.Groups = new SelectList(groups, "Id", "Name", 2);

            List<User> students = db.Users.Where(s => s.RoleId == 3).Where(s => s.GroupId == 2).ToList();

            return View(students);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult GroupManager(int? group_filter)
        {
            if (group_filter == null)
            {
                group_filter = 2;
            }
            IEnumerable<Group> groups = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").ToList();
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<User> students = db.Users.Where(s => s.RoleId == 3).Where(s => s.GroupId == group_filter).ToList();

            return View(students);
        }

        public ActionResult ChangeGroup(List<User> updatedStudents)
        {
            foreach (User student in updatedStudents)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("GroupManager");
        }
    }
}
