namespace OnlineEShop.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OnlineEShop.Data.Models;

    public class Category 
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; init; }

        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}