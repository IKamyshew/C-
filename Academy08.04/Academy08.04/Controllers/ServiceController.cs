using Academy08._04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace Academy08._04.Controllers
{
    [Authorize(Roles="Manager")]
    public class ServiceController : Controller
    {

        AcademyContext db = new AcademyContext();

        [HttpGet]
        public ActionResult Groups()
        {
            ViewBag.Groups = db.Groups;
            return View();
        }

        [HttpPost]
        public ActionResult Groups(Group group)
        {
            if (!db.Groups.ToList().Contains(group)) // does't work! :(
            {
                db.Groups.Add(group);
                db.SaveChanges();
            }

            return View();
        }
    }
}
