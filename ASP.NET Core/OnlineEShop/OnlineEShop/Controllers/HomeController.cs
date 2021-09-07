using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineEShop.Models;
using OnlineEShop.Services.Categories;
using OnlineEShop.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriesService categoriesService;

        public HomeController(ILogger<HomeController> logger,
            ICategoriesService categoriesService)
        {
            _logger = logger;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new CategoriesInListModel
            {
                Categories = this.categoriesService.AllCategory()
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

