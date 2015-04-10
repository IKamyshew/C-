using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy08._04.Models;

namespace Academy08._04.Controllers
{
    // 
    [Authorize(Roles = "Manager, Teacher")]
    public class UserController : Controller
    {
        private AcademyContext db = new AcademyContext();

        [HttpGet]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Group).Include(u => u.Role).ToList();
            return View(users);
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
    }
}
