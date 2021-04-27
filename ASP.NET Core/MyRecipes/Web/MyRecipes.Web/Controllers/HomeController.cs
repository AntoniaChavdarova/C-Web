namespace MyRecipes.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using MyRecipes.Services.Data;
    using MyRecipes.Web.ViewModels;
    using MyRecipes.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IGetCountsService service;
        private readonly IRecipesService recipesService;

        public HomeController(IGetCountsService service, IRecipesService recipesService)
        {
            this.service = service;
            this.recipesService = recipesService;
        }

        public IActionResult Index()
        {
            var viewModel = this.service.GetCount();
            viewModel.RandomRecipe = this.recipesService.GetRandom<IndexPageRecipeViewModel>(10);

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
