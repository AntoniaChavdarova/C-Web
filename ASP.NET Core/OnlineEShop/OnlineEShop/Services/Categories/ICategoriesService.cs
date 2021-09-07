using OnlineEShop.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEShop.Services.Categories
{
    public interface ICategoriesService
    {
        IEnumerable<CategoriesModel> AllCategory();
    }
}
