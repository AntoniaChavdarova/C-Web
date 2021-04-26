namespace MyRecipes.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using MyRecipes.Services.Data;
    using MyRecipes.Web.ViewModels.Recipes;

    public class RecipesController : Controller
    {
        private readonly IRecipesService recipesService;
        private readonly ICategoriesService categoriesService;
        private readonly IWebHostEnvironment environment;

        public RecipesController(
            IRecipesService recipesService,
            ICategoriesService categoriesService,
            IWebHostEnvironment environment)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
            this.environment = environment;
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateRecipeInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecipeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            //moje i chres usermanager : var user = await this.userManager.GetUserAsync(this.User);

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
            await this.recipesService.CreateAsync(input, userId, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            return this.Redirect("/");
        }

        // inache gurmi s tova che id ako ne e podaden e nula

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int ItemsPerPage = 12;
            var viewModel = new RecipesListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                RecipesCount = this.recipesService.GetCount(),
                Recipes = this.recipesService.GetAll<RecipeInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var recipe = this.recipesService.GetById<SingleRecipeViewModel>(id);
            return this.View(recipe);
        }
    }
}
