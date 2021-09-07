using OnlineEShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEShop.ViewModels
{
    public class CreateProductInputModel
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(DataConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(1, Double.MaxValue)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [MinLength(DataConstants.DescriptionMinLength)]
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

    }
}
