namespace OnlineEShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product 
    {
        public Product()
        {
            this.Votes = new HashSet<Vote>();
            this.Orders = new HashSet<OrderProduct>();
        }
        public int Id { get; init; }

        [Required]
        [MaxLength(DataConstants.NameMaxLength)]
        public string Name { get; set; }

        [Range(1, Double.MaxValue)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        [MinLength(DataConstants.DescriptionMinLength)]
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }


    }
}
