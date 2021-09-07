namespace OnlineEShop.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(DataConstants.FullNameMaxLength)]
        public string FullName { get; set; }



    }
}
