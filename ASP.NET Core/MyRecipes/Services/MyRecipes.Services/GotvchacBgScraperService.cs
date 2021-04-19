namespace MyRecipes.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;


    using HtmlAgilityPack;
    using MyRecipes.Data.Common.Repositories;
    using MyRecipes.Data.Models;
    using MyRecipes.Services.Models;

    public class GotvchacBgScraperService : IGotvachBgScraperService
    {
        private readonly HtmlWeb web;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingridientsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipiesRepository;
        private readonly IRepository<RecipeIngredient> recipeIngredientsRepository;
        private readonly IRepository<Image> imagesRepository;

        public GotvchacBgScraperService(
           IDeletableEntityRepository<Category> categoriesRepository,
           IDeletableEntityRepository<Ingredient> ingridientsRepository,
           IDeletableEntityRepository<Recipe> recipiesRepository,
           IRepository<RecipeIngredient> recipeIngredientsRepository,
           IRepository<Image> imagesRepository
          )
        {
            this.categoriesRepository = categoriesRepository;
            this.ingridientsRepository = ingridientsRepository;
            this.recipiesRepository = recipiesRepository;
            this.recipeIngredientsRepository = recipeIngredientsRepository;
            this.imagesRepository = imagesRepository;

            this.web = new HtmlWeb();
        }

        public async Task PopulateDbWithRecipesAsync(int recipesCount)
        {
            var concurentBag = new ConcurrentBag<RecipeDto>();
            HttpStatusCode statusCode = HttpStatusCode.OK;


            this.web.PostResponse += (request, response) =>
            {
                if (response != null)
                {
                    statusCode = response.StatusCode;
                }
            };

            Parallel.For(1, recipesCount, (i) =>
            {
                try
                {
                    var recipe = this.GetRecipe(i);
                    concurentBag.Add(recipe);
                }
                catch
                {
                }
            });

            foreach (var recipe in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(recipe.CategoryName);

                var recipeExists = this.recipiesRepository
                    .AllAsNoTracking()
                    .Any(x => x.Name == recipe.RecipeName);

                if (recipeExists)
                {
                    continue;
                }

                var newRecipe = new Recipe()
                {
                    Name = recipe.RecipeName,
                    Instructions = recipe.Instrucitons,
                    PreparatingTime = recipe.PreparationTime,
                    CookingTime = recipe.CookingTime,
                    PortionsCount = recipe.PortionsCount,
                    //OriginalUrl = recipe.OriginalUrl,
                    CategoryId = categoryId,
                };

                await this.recipiesRepository.AddAsync(newRecipe);
                await this.recipiesRepository.SaveChangesAsync();

                foreach (var item in recipe.Ingredients)
                {
                    var ingr = item.Split(" - ", 2);

                    if (ingr.Length < 2)
                    {
                        continue;
                    }

                    var ingridientId = await this.GetOrCreateIngridientAsync(ingr[0].Trim());
                    var qty = ingr[1].Trim();

                    var recipeIngridient = new RecipeIngredient
                    {
                        IngredientId = ingridientId,
                        RecipeId = newRecipe.Id,
                        Quantity = qty,
                    };

                    await this.recipeIngredientsRepository.AddAsync(recipeIngridient);
                    await this.recipeIngredientsRepository.SaveChangesAsync();
                }

                var image = new Image
                {
                    Extension = recipe.OriginalUrl,
                    RecipeId = newRecipe.Id,
                };

                await this.imagesRepository.AddAsync(image);
                await this.imagesRepository.SaveChangesAsync();
            }
        }

        private async Task<int> GetOrCreateIngridientAsync(string name)
        {
            var ingridient = this.ingridientsRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name);

            if (ingridient == null)
            {
                ingridient = new Ingredient
                {
                    Name = name,
                };

                await this.ingridientsRepository.AddAsync(ingridient);
                await this.ingridientsRepository.SaveChangesAsync();
            }

            return ingridient.Id;
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var catogory = this.categoriesRepository
                                .AllAsNoTracking()
                                .FirstOrDefault(x => x.Name == categoryName);

            if (catogory == null)
            {
                catogory = new Category()
                {
                    Name = categoryName,
                };

                await this.categoriesRepository.AddAsync(catogory);
                await this.categoriesRepository.SaveChangesAsync();
            }

            return catogory.Id;
        }

        private RecipeDto GetRecipe(int id)
        {
            var html = $"https://recepti.gotvach.bg/r-{id}";
            var htmlDoc = this.web.Load(html);

            if (this.web.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException();
            }

            var recipe = new RecipeDto();

            recipe.CategoryName = GetCategoryName(htmlDoc);
            recipe.RecipeName = GetRecipeName(htmlDoc);

            var instrudctionsToLoad = htmlDoc
          .DocumentNode
          .SelectNodes(@"//div[@class='text']/p")
          .Select(x => x.InnerText)
         .ToList();

            var sb = new StringBuilder();
            foreach (var item in instrudctionsToLoad)
            {
                sb.AppendLine(item);
            }

            recipe.Instrucitons = sb.ToString().TrimEnd();

            var timing = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='feat small']");

            if (timing.Count == 2)
            {
                var preparation = timing[0].InnerText.Replace("Приготвяне", "").Split(" ").FirstOrDefault();
                recipe.PreparationTime = TimeSpan.FromMinutes(int.Parse(preparation));

                var cooking = timing[1].InnerText.Replace("Готвене", "").Split(" ").FirstOrDefault();
                recipe.CookingTime = TimeSpan.FromMinutes(int.Parse(cooking));
            }
            else if (timing.Count == 1)
            {
                if (timing[0].InnerText.Contains("Приготвяне"))
                {
                    var preparation = timing[0].InnerText.Replace("Приготвяне", "").Split(" ").FirstOrDefault();
                    recipe.PreparationTime = TimeSpan.FromMinutes(int.Parse(preparation));

                }
                else
                {
                    var cooking = timing[1].InnerText.Replace("Готвене", "").Split(" ").FirstOrDefault();
                    recipe.CookingTime = TimeSpan.FromMinutes(int.Parse(cooking));
                }
            }

            var portions = htmlDoc
               .DocumentNode
               .SelectNodes(@"//div[@class='feat']/span");
            var portionsCount = 0;
            if (portions != null)
            {
                int.TryParse(portions.Select(x => x.InnerHtml)
                    .LastOrDefault().Replace("Порции", string.Empty)
                       .Replace("бр", string.Empty)
                       .Replace("бр.", string.Empty)
                       .Replace("броя", string.Empty)
                       .Replace("бройки", string.Empty), out portionsCount);
            }

            recipe.PortionsCount = portionsCount;

            var photos = new List<string>();
            var photosUrls = htmlDoc
                .DocumentNode
                .SelectNodes(@"//a[@class='morebtn']");

            var urlPhoros = photosUrls
                .FirstOrDefault()
                ?.GetAttributeValue("href", "unknown");

            var link = web.Load(urlPhoros);
            var photosUrlsToLoad = link
                .DocumentNode
                .SelectNodes(@"//div[@class='main']/div/img");

            if (photosUrlsToLoad != null)
            {
                var picturesUrls = photosUrlsToLoad.ToList();

                if (picturesUrls[0].GetAttributeValue("src", "unknown") ==
                    "https://recepti.gotvach.bg/files/recipes/photos/")
                {
                    picturesUrls.Clear();
                }
                else
                {
                    photos.AddRange(picturesUrls.Select(p => p.GetAttributeValue("src", "unknown")));
                }
            }

            recipe.OriginalUrl = photos.FirstOrDefault();


            var ingredientsParse = htmlDoc
         .DocumentNode
         .SelectNodes(@"//section[@class='products new']/ul/li");

            var ingredients = new List<string>();
            ingredients.AddRange(ingredientsParse
                    .Select(li => li.InnerText)
                    .ToList());

            foreach (var item in ingredients)
            {
                recipe.Ingredients.Add(item);
            }

            return recipe;
        }

        private static string GetCategoryName(HtmlDocument htmlDoc)
        {
            var category = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='breadcrumb']");
            if (category != null)
            {
                return category
                    .Select(x => x.InnerText)
                    .FirstOrDefault()
                   ?.Split(" »")
                    .Reverse()
                    .ToList()[1];
            }

            return string.Empty;
        }

        private static string GetRecipeName(HtmlDocument htmlDoc)
        {
            var name = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='combocolumn mr']/h1");

            if (name != null)
            {
                return name.Select(r => r.InnerText)
                    .FirstOrDefault().ToString();
            }

            return string.Empty;
        }

    }
}
