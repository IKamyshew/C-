﻿using System;
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
                try { 
                    User userEntity = db.Users.Where(l => l.Login == user.Login).FirstOrDefault();
                    ModelState.AddModelError("", "This login already exist in database");
                } catch {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please fill all fields");
            }
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;
            return View(user);
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
            int group_filter = db.Groups.Where(gr => gr.Name != "Managers" && gr.Name != "Teachers").FirstOrDefault().Id;

            List<TeachersGroups> teachers = db.TeachersGroups.Include(t => t.Teacher).Include(g => g.Group).Where(gr => gr.GroupId == group_filter).ToList();
            ViewBag.Teachers = teachers;

            List<User> students = db.Users.Where(s => s.RoleId == 3).Where(s => s.GroupId == group_filter).ToList();

            Session["group"] = db.Groups.Find(group_filter);
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

            List<TeachersGroups> teachers = db.TeachersGroups.Include(t => t.Teacher).Include(g => g.Group).Where(gr => gr.GroupId == group_filter).ToList();
            ViewBag.Teachers = teachers;

            List<User> students = db.Users.Where(s => s.RoleId == 3).Where(s => s.GroupId == group_filter).ToList();

            Session["group"] = db.Groups.Find(group_filter);
            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult TeacherEditor()
        {
            int? group_filter = null;
            Group currentGroup = null;

            if (Session["group"] != null)
            {
                currentGroup = (Group)Session["group"];
                group_filter = currentGroup.Id;
            } else {
                group_filter = 2;
            }

            List<User> allTeachers = db.Users.Where(r => r.RoleId == 2).ToList();
            List<TeachersGroups> TeachersGroups = db.TeachersGroups.Where(gr => gr.GroupId == group_filter).ToList();
            List<User> teachersInGroup = new List<User>();
            List<User> teachersOutOfGroup = new List<User>();

            bool flag;
            foreach (var teacher in allTeachers)
            {
                flag = true;
                foreach (var teacherInGroup in TeachersGroups)
                {
                    if (teacherInGroup.TeacherId == teacher.Id) { 
                        teachersInGroup.Add(teacher);
                        flag = false;
                    }
                }
                if (flag)
                {
                    teachersOutOfGroup.Add(teacher);
                }
            }

            ViewBag.Teachers = new SelectList(teachersOutOfGroup, "Id", "LastName");
            ViewBag.GroupTeachers = new SelectList(teachersInGroup, "Id", "LastName");
            ViewBag.GroupTeachersView = teachersInGroup;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult TeacherEditor(int? add_filter, int? remove_filter)
        {
            Group currentGroup = null;

            if (Session["group"] != null)
            {
                currentGroup = (Group)Session["group"];
            }

            if (add_filter != null)
            {
                if (ModelState.IsValid) { 
                    TeachersGroups tg =  new TeachersGroups();
                    tg.TeacherId = (int) add_filter;
                    tg.GroupId = currentGroup.Id;
                    db.Entry(tg).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
            if (remove_filter != null)
            {
                TeachersGroups removeTeacher = db.TeachersGroups.Where(t => t.TeacherId == (int) remove_filter).Where(gr => gr.GroupId == currentGroup.Id).FirstOrDefault();
                db.Entry(removeTeacher).State = EntityState.Deleted;
                db.SaveChanges();
            }

            return RedirectToAction("TeacherEditor");
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
