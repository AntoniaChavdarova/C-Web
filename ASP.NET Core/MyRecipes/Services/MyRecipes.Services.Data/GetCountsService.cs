namespace MyRecipes.Services.Data
{
 using System.Linq;

 using MyRecipes.Data.Common.Repositories;
 using MyRecipes.Data.Models;
 using MyRecipes.Web.ViewModels.Home;

 public class GetCountsService : IGetCountsService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientRepository;
        private readonly IRepository<Image> imageRepository;

        public GetCountsService(
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<Recipe> recipeRepository,
            IDeletableEntityRepository<Ingredient> ingredientRepository,
            IRepository<Image> imageRepository)
        {
            this.categoryRepository = categoryRepository;
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.imageRepository = imageRepository;
        }

        public IndexViewModel GetCount()
        {
            var data = new IndexViewModel
            {
                CategoriesCount = this.categoryRepository.All().Count(),
                IngredientsCount = this.ingredientRepository.All().Count(),
                ImagesCount = this.imageRepository.All().Count(),
                RecipesCount = this.recipeRepository.All().Count(),
            };

            return data;
        }
    }
}
