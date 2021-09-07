namespace OnlineEShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class Order 
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public int Id { get; init; }

        public virtual ApplicationUser User { get; set; }

        public int UserId { get; set; }

        [Required]
        [MinLength(DataConstants.TownNameMinLength)]        
        public string Townn { get; set; }

        [Required]
        [MinLength(DataConstants.AddressTextMinLength)]
        public string Address { get; set; }

        public string Notes { get; set; }

        [Range(1, Double.MaxValue)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderProduct> Products { get; set; }

    }
}
