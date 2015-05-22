using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Academy.ASPNET.Models;
using Academy.Model.Entities;
using Academy.Model.DBAccess;

namespace Academy.ASPNET.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private CurrentUser CurrentUser = new CurrentUser();

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["message"] = "You don't have access to this page. Please, contact your administrator.";
                return RedirectToAction("ProfilePage", "Interface");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (CurrentUser.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ProfilePage", "Interface");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            CurrentUser.LogOff();
            return RedirectToAction("Login", "Account");
        }
    }
}
