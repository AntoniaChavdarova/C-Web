using Microsoft.AspNetCore.Mvc;
using MyRecipes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRecipes.Web.Controllers
{
    public class GatherRecipiesController : BaseController
    {
        private readonly IGotvachBgScraperService gotvachBgScraperService;

        public GatherRecipiesController(IGotvachBgScraperService gotvachBgScraperService)
        {
            this.gotvachBgScraperService = gotvachBgScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.gotvachBgScraperService.PopulateDbWithRecipesAsync(200);

            return this.RedirectToAction("Index");
        }
    }
}
