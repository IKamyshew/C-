using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class BookShopController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}