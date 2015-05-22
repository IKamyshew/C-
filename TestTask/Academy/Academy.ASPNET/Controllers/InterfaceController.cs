using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Academy.Model.DBAccess;
using Academy.Model.Entities;
using System.Web.Security;

namespace Academy.ASPNET.Controllers
{
    [Authorize]
    public class InterfaceController : Controller
    {
        public ActionResult ProfilePage()
        {
            string UserName = System.Web.HttpContext.Current.User.Identity.Name;
            CurrentUser CurrentUser = new CurrentUser();
            User user = CurrentUser.GetCurrentUserByLogin(UserName);

            return View(user);
        }

    }
}
