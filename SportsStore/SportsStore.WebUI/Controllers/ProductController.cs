using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepozitory repository;

        public ProductController(IProductRepozitory productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List()
        {
            System.Diagnostics.Debugger.NotifyOfCrossThreadDependency();
            return View(repository.Products);
        }
    }
}
