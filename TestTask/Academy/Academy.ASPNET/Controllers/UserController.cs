using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy.Model.DBAccess;
using Academy.Model.Entities;

namespace Academy.ASPNET.Controllers
{
    public class UserController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            List<User> users = db.GetAllUsers();
            return View();
        }

    }
}
