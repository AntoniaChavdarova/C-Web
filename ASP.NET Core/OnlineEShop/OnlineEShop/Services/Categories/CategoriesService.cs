using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineEShop.Data;
using OnlineEShop.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEShop.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext data;
        private readonly IConfigurationProvider mapper;
        public CategoriesService(ApplicationDbContext data , IMapper mapper)
        {

            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<CategoriesModel> AllCategory()
        {
            return this.data.Categories.ProjectTo<CategoriesModel>(this.mapper)
                .ToList();
        }
    }
}
