namespace MyRecipes.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MyRecipes.Data.Common.Repositories;
    using MyRecipes.Data.Models;
    using MyRecipes.Services.Mapping;
    using MyRecipes.Web.ViewModels.Recipes;

    public class RecipeService : IRecipesService
    {
        private readonly string[] alloewdExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRespository;

        public RecipeService(
            IDeletableEntityRepository<Recipe> recipesRepository,
            IDeletableEntityRepository<Ingredient> ingredientsRespository)
        {
            this.recipesRepository = recipesRepository;
            this.ingredientsRespository = ingredientsRespository;
        }

        public async Task CreateAsync(CreateRecipeInputModel input, string userId ,string imagePath)
        {
            var recipe = new Recipe
            {
                CategoryId = input.CategoryId,
                CookingTime = TimeSpan.FromMinutes(input.CookingTime),
                Instructions = input.Instructions,
                Name = input.Name,
                PortionsCount = input.PortionsCount,
                PreparatingTime = TimeSpan.FromMinutes(input.PreparatingTime),
                AddedByUserId = userId,
            };

            foreach (var inputIngredient in input.Ingredients)
            {
                var ingredient = this.ingredientsRespository.All().FirstOrDefault(x => x.Name == inputIngredient.Name);
                if (ingredient == null)
                {
                    ingredient = new Ingredient { Name = inputIngredient.Name };
                }

                recipe.Ingredients.Add(new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Quantity = inputIngredient.Quantity,
                });
            }

            // /wwwroot/images/recipes/{id}.ext
            Directory.CreateDirectory($"{imagePath}/recipes/");
            foreach (var image in input.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!this.alloewdExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new Image
                {
                    AddedByUserId = userId,
                    Extension = extension,
                };

                recipe.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/recipes/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.recipesRepository.AddAsync(recipe);
            await this.recipesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            var recipes = this.recipesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>().ToList();
            return recipes;
        }

        public T GetById<T>(int id)
        {
            var recipe = this.recipesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return recipe;
        }

        public IEnumerable<T> GetByIngredients<T>(IEnumerable<int> ingredientIds)
        {
            var query = this.recipesRepository.All().AsQueryable();
            foreach (var id in ingredientIds)
            {
                query = query.Where(x => x.Ingredients.Any(y => y.Id == id));
            }

            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.recipesRepository.All().Count();
        }

        public IEnumerable<T> GetRandom<T>(int count)
        {
            return this.recipesRepository.All().OrderBy(x => Guid.NewGuid())
                .Take(count)
                .To<T>().ToList();

        }

        public async Task UpdateAsync(int id, EditRecipeInputModel input)
        {
            var recipe = this.recipesRepository.All().FirstOrDefault(x => x.Id == id);
            recipe.Name = input.Name;
            recipe.Instructions = input.Instructions;
            recipe.CookingTime = TimeSpan.FromMinutes(input.CookingTime);
            recipe.PreparatingTime = TimeSpan.FromMinutes(input.PreparatingTime);
            recipe.PortionsCount = input.PortionsCount;
            recipe.CategoryId = input.CategoryId;
            await this.recipesRepository.SaveChangesAsync();

        }
    }
}
