namespace MyRecipes.Web.ViewModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using MyRecipes.Data.Models;
    using MyRecipes.Services.Mapping;

    public class RecipeIngredientInputModel
    {
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        public string Quantity { get; set; }
    }
}
