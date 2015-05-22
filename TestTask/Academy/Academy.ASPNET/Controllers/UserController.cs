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

        [Authorize(Roles = "Manager")]
        public ActionResult Create(User user)
        {
            SelectList groups = new SelectList(db.GetAllGroups(), "Id", "Name");
            ViewBag.Groups = groups;
            SelectList roles = new SelectList(db.GetAllRoles(), "Id", "Name");
            ViewBag.Roles = roles;

            if (user.RoleId != 0)
            {
                if (ModelState.IsValid)
                {
                    if (db.AddUser(user)) {
                        TempData["message"] = ("User " + user.FirstName + " " + user.LastName + " was successfully added.");
                        return RedirectToAction("Users", "User");
                    } else { 
                        ModelState.AddModelError("", "This login already exist in database");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please fill all fields");
                }

                return View(user);
            }
            else
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
    }
}
