using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineEShop.Data;
using OnlineEShop.Data.Models;
using OnlineEShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEShop.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext data;
        private readonly IConfigurationProvider mapper;

        public ProductsService(ApplicationDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<CreateProductInputModel> AllProductsInCategory(int id)
        {
            return this.data.Products.Where(x => x.Category.Id == id)
                .ProjectTo<CreateProductInputModel>(this.mapper)
                .ToList();
        }

        public CreateProductInputModel ById(int id)
        {
            return this.data.Products.Where(x => x.Id == id)
                .ProjectTo<CreateProductInputModel>(this.mapper)
                .FirstOrDefault();
        }
        public async Task CreateAsync(CreateProductInputModel input)
        {
            var recipe = new Product
            {
                CategoryId = input.CategoryId,                
                Description = input.Description,
                Name = input.Name,
                Price = input.Price,
                ImageUrl = input.ImageUrl,
                
            };

            await this.data.Products.AddAsync(recipe);
            await this.data.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.data.Categories
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        
    }
}
