using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy.Model.DBAccess;
using Academy.Model.Entities;

namespace Academy.ASPNET.Controllers
{
    [Authorize(Roles = "Manager, Teacher")]
    public class UserController : Controller
    {
        Entities db = new Entities();

        public ActionResult Users(int group_filter = 0, int role_filter = 0)
        {
            Entities db = new Entities();

            IEnumerable<User> allUsers = null;

            if (role_filter == 0 && group_filter == 0)
            {
                allUsers = db.GetAllUsers();
            }
            else if (role_filter == 0 && group_filter != 0)
            {
                allUsers = db.GetUsersFilteredByGroup(group_filter);
            }
            else if (role_filter != 0 && group_filter == 0)
            {
                allUsers = db.GetUsersFilteredByRole(role_filter);
            }
            else
            {
                allUsers = db.GetUsersFilteredByRoleAndGroup(role_filter, group_filter);
            }

            List<Group> groups = db.GetAllGroups();
            groups.Insert(0, new Group { Name = "All", Id = 0 });
            ViewBag.Groups = new SelectList(groups, "Id", "Name");

            List<Role> roles = db.GetAllRoles();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(allUsers);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult AddUser()
        {
            SelectList groups = new SelectList(db.GetAllGroups(), "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.GetAllRoles(), "Id", "Name");
            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult AddUser(User user)
        {
            SelectList groups = new SelectList(db.GetAllGroups(), "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.GetAllRoles(), "Id", "Name");
            ViewBag.Roles = roles;
            
            if (ModelState.IsValid)
            {
                if (db.AddUser(user)) {
                        TempData["message"] = ("User " + user.FirstName + " " + user.LastName + " was successfully added.");
                        return RedirectToAction("Users", "User");
                } else {
                     ModelState.AddModelError("", "This login already exist in database");
                }
            } else {
                 ModelState.AddModelError("", "Please fill all fields");
            }
            return View(user);
        }


        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            User user = db.GetUserByID(id);

            SelectList groups = new SelectList(db.GetAllGroups(), "Id", "Name", user.GroupId);
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.GetAllRoles(), "Id", "Name", user.RoleId);
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.UpdateUser(user))
                    TempData["message"] = "User " + user.FirstName + " " + user.LastName + " successfully updated";
                else
                    TempData["message"] = "User " + user.FirstName + " " + user.LastName + " failed to updated";
            }

            return RedirectToAction("Users", "User");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            User user = db.GetUserByID(id);
            if (db.DeleteUser(user))
                TempData["message"] = "User successfully deleted";
            else
                TempData["message"] = "User " + user.FirstName + " " + user.LastName + " failed to delete";

            return RedirectToAction("Users", "User");
        }

        [HttpGet]
        public ActionResult Subjects()
        {
            List<Subject> subjects = db.GetAllSubjects();
            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        public ActionResult AddSubject(Subject subject)
        {
            if (ModelState.IsValid) { 
                if(db.AddSubject(subject))
                    TempData["message"] = "Subject " + subject.Name + " successfully added";
                else
                    TempData["message"] = "Subject failed to add";
            }

            return RedirectToAction("Subjects");
        }

        public ActionResult DeleteSubject(int id)
        {
            if(db.DeleteSubject(id))
                TempData["message"] = "Subject successfully deleted";
            else
                TempData["message"] = "Subject failed to delete";

            return RedirectToAction("Subjects");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult GroupManager()
        {
            List<Group> groups = db.GetAllGroups();
            ViewBag.Groups = groups;
            return View();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult AddGroup(Group group)
        {
            if (db.AddGroup(group))
                TempData["message"] = "Group " + group.Name + " successfully added";
            else
                TempData["message"] = "Failed to add group " + group.Name;

            return RedirectToAction("GroupManager");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult DeleteGroup(int id)
        {
            if(db.DeleteGroup(id))
                TempData["message"] = "Group successfully deleted";
            else
                TempData["message"] = "Failed to delete group";

            return RedirectToAction("GroupManager");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult TeacherGroupManager(int group_filter = -1)
        {
            List<Group> groups = db.GetAllStudentGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Name", 2);
            if(group_filter == -1)
                group_filter = db.GetAllStudentGroups().FirstOrDefault().Id;

            List<TeachersGroups> teachers = db.GetTeachersForGroup(group_filter);
            ViewBag.Teachers = teachers;

            List<User> students = db.GetAllStudentsForGroup(group_filter);

            Session["group"] = db.GetGroupByID(group_filter);
            return View(students);
        }


        [Authorize(Roles = "Teacher")]
        public ActionResult TeacherEditor()
        {
            int group_filter = -1;

            if (Session["group"] != null)
                group_filter = ((Group)Session["group"]).Id;
            else
                group_filter = db.GetAllStudentGroups().FirstOrDefault().Id;

            List<User> allTeachers = db.GetAllTeachers();
            List<User> teachersInGroup = new List<User>();
            List<User> teachersOutOfGroup = new List<User>();

            foreach (var teacher in allTeachers)
            {
                if (db.IsTeacherRelatesToGroup(group_filter,teacher.Id))
                    teachersInGroup.Add(teacher);
                else
                    teachersOutOfGroup.Add(teacher);
            }

            ViewBag.Teachers = new SelectList(teachersOutOfGroup, "Id", "LastName");
            ViewBag.GroupTeachers = new SelectList(teachersInGroup, "Id", "LastName");
            ViewBag.GroupTeachersView = teachersInGroup;

            return View();
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult EditTeachers(int? add_filter, int? remove_filter)
        {
            Group currentGroup = null;

            if (Session["group"] != null)
                currentGroup = (Group)Session["group"];
            else { 
                TempData["message"] = "Failed to determine group. Please choose group firstly.";
                return RedirectToAction("TeacherGroupManager");
            }


            if (add_filter != null)
                db.AddTeacherToGroup(currentGroup.Id, (int)add_filter);

            if (remove_filter != null)
                db.RemoveTeacherToGroup(currentGroup.Id, (int)remove_filter);

            return RedirectToAction("TeacherEditor");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ChangeGroup(List<User> updatedStudents)
        {
            foreach (User student in updatedStudents)
                if (ModelState.IsValid)
                    db.UpdateUser(student);

            return RedirectToAction("TeacherGroupManager");
        }
    }
}
