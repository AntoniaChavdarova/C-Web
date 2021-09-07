namespace OnlineEShop.Infrastructure
{
    using AutoMapper;
    using OnlineEShop.Data;
    using OnlineEShop.Data.Models;
    using OnlineEShop.ViewModels;
    using OnlineEShop.ViewModels.Categories;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Product, CreateProductInputModel>();

            this.CreateMap<Category, CategoriesModel>();
            //this.CreateMap<CarDetailsServiceModel, CarFormModel>();

            //this.CreateMap<Car, CarServiceModel>()
            //    .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));

            //this.CreateMap<Car, CarDetailsServiceModel>()
            //    .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
            //    .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}
